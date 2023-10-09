using System.ComponentModel;
using System.Threading;
using ImageViewer.Delegates;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Services;
using ImageViewer.UserControls;
using ImageViewer.Utility;

namespace ImageViewer;

public partial class FormThumbnailView : Form, IDisposable
{
    private readonly ApplicationSettingsService _applicationSettings;
    private readonly ImageLoaderService _imageLoaderService;
    private readonly ILifetimeScope _scope;
    private readonly ThumbnailFormState _state = new();
    private readonly ThumbnailService _thumbnailService;
    private readonly ManualResetEvent _manualResetEvent;
    private EventWaitHandle _eventWaitHandle;
    private static long threadCount = 0;

    // Flag: Has Dispose already been called?
    private bool _disposed;
    private bool _formIsDisposing;
    private string _maximizedImgFilename;
    private int _maxThumbnails;
    private ThumbnailScanDirectory _thumbnailScan;
    private int _thumbnailSize;
    private readonly CancellationTokenSource _tokenSource;
    private UpdatePicBoxListEventHandler _updatePicBoxList;

    public FormThumbnailView(ApplicationSettingsService applicationSettings, ThumbnailService thumbnailService, ImageLoaderService imageLoaderService, ILifetimeScope scope)
    {
        _applicationSettings = applicationSettings;
        _thumbnailService = thumbnailService;
        _imageLoaderService = imageLoaderService;
        _scope = scope;
        _updatePicBoxList += UpdatePicBoxList;
        _tokenSource = new CancellationTokenSource();

        if (applicationSettings == null)
        {
            throw new NullReferenceException(Resources.Language.AppApplicationSettingsServiceNull);
        }

        if (_thumbnailService == null)
        {
            throw new NullReferenceException(Resources.Language.ThumbnailServiceNull);
        }

        _eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        _manualResetEvent = new ManualResetEvent(false);
        _thumbnailSize = ValidateThumbnailSize(applicationSettings.Settings.ThumbnailSize);
        _maxThumbnails = AppSettings.MaxThumbnails;
        _applicationSettings.OnSettingsSaved += ApplicationSettingsOnSettingsSaved;


        InitializeComponent();
    }


    private ApplicationSettingsModel AppSettings => _applicationSettings?.Settings;

    private void ApplicationSettingsOnSettingsSaved(object sender, EventArgs e)
    {
        _applicationSettings.LoadSettings();
        var appSettings = _applicationSettings.Settings;
        flowLayoutPanel1.BackColor = appSettings.MainWindowBackgroundColor;
        picBoxMaximized.BackColor = appSettings.MainWindowBackgroundColor;
    }

    private async void FormThumbnailView_Load(object sender, EventArgs e)
    {
        if (DesignMode)
        {
            return;
        }

        FormStateManager.RestoreFormState(AppSettings, this);
        await _thumbnailService.LoadThumbnailDatabaseAsync();

        SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        flowLayoutPanel1.BackColor = AppSettings.MainWindowBackgroundColor;
        picBoxMaximized.BackColor = AppSettings.MainWindowBackgroundColor;
        UpdateStyles();
    }

    private void FormThumbnailView_Closing(object sender, CancelEventArgs e)
    {
        if (!_state.CanClose)
        {
            e.Cancel = true;
            return;
        }

        Hide();

        Task.Factory.StartNew(async () => { await _thumbnailService.SaveThumbnailDatabase(); }).Wait();

        _formIsDisposing = true;
        var appSettings = AppSettings;
        FormStateManager.SaveFormState(appSettings, this);
        _applicationSettings.SaveSettings();
    }

    private IEnumerable<Control> GetControlTree(Control root)
    {
        var controls = new List<Control> { root };

        if (!root.HasChildren)
        {
            return controls;
        }

        for (int i = 0; i < root.Controls.Count; i++)
        {
            var controlList = GetControlTree(root.Controls[i]);
            controls.AddRange(controlList);
        }

        return controls;
    }


    // Generate Thumbnail list
    private void btnGenerate_Click(object sender, EventArgs e)
    {
        if (_imageLoaderService.ImageReferenceList == null)
        {
            MessageBox.Show(@"Cant generate thumbnails without any image source loaded.", "No Images loaded!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (btnGenerate.Tag is CancellationTokenSource source)
        {
            source.Token.WaitHandle.WaitOne(2000, true);
            btnGenerate.Tag = null;
            btnGenerate.Enabled = false;
            _manualResetEvent.Set();
            _eventWaitHandle.Set();
            return;
        }

        if (_state.IsGeneratingThumbnails)
        {
            return;
        }

        if (Interlocked.Read(ref threadCount) == 0)
        {
            _manualResetEvent.Set();
            _eventWaitHandle.Set();

        }
        else
        {
            return;
        }

        HideMaximizedView();
        SetUpdateDatabaseEnabledState(false);
        flowLayoutPanel1.Controls.Clear();
        _state.StartingThumbnailScan();


        try
        {
            Interlocked.Increment(ref threadCount);
            _manualResetEvent.Reset();
            Task.Factory.StartNew(async () =>
            {
                await BindAndLoadThumbnailsAsync();
                _manualResetEvent.Set();
            }).ConfigureAwait(true).GetAwaiter().OnCompleted(() => { _manualResetEvent.Set();});


        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, Resources.Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Log.Error(ex, "Error in generate thumbnails");
        }
        finally
        {
            Interlocked.Decrement(ref threadCount);
        }

        btnGenerate.Enabled = true;
        if (_tokenSource.IsCancellationRequested)
        {
            _tokenSource.TryReset();
            _manualResetEvent.Set();
        }



        // Dont execute before BindAndLoadThumbnails is completed
        Task.Factory.StartNew(async () =>
        {
            _manualResetEvent.WaitOne(2500, true);
            await _thumbnailService.SaveThumbnailDatabase().ConfigureAwait(true);
        }).ConfigureAwait(true);


        Refresh();
        GC.Collect(0, GCCollectionMode.Optimized);
    }

    private async Task BindAndLoadThumbnailsAsync()
    {
        var modelList = GenerateThumbnailModels().ToList();
        var collection = new SynchronizedCollection<PictureBoxModel>();
        foreach (var model in modelList)
            collection.Add(model);


        // Generate thumbnails while keeping the UI 100% responsive
        await Task.Factory.StartNew(async () =>
        {
            try
            {
                var options = new ParallelOptions
                {
                    CancellationToken = _tokenSource.Token,
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };
                var parallelOperation = Parallel.ForEachAsync(collection, options, async (model, token) =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        var picBox = CreatePictureBox(model);
                        picBox.Image = await LoadAndResizeImage(model.SourceImagePath).ConfigureAwait(true);
                        Invoke(new UpdatePicBoxListEventHandler(UpdatePicBoxList), this, new UpdatePicBoxEventArgs(picBox));
                    }
                }).ConfigureAwait(true);
                parallelOperation.GetAwaiter().OnCompleted(() => { _manualResetEvent.Set(); });

                await parallelOperation;
                _manualResetEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BindAndLoadThumbnailsAsync was interrupted");
            }
            finally
            {
                _eventWaitHandle.Set();
            }

            await Task.Yield();
        }, _tokenSource.Token).ConfigureAwait(true);

        _eventWaitHandle.Reset();
        _eventWaitHandle.WaitOne(TimeSpan.FromMinutes(1));

        if (!_formIsDisposing)
        {
            Invoke(new EventHandler(ThumbnailGenerationCompleted));
        }
    }

    private async Task<Image> LoadAndResizeImage(string imagePath)
    {
        var image = await _thumbnailService.GetThumbnailAsync(imagePath);
        return image;
    }

    private PictureBox CreatePictureBox(PictureBoxModel model)
    {
        var picBox = new PictureBox
        {
            Width = model.ThumbnailSize.Width,
            Height = model.ThumbnailSize.Height,
            BorderStyle = model.BorderStyle,
            SizeMode = model.SizeMode
        };

        picBox.Tag = model.SourceImagePath;
        picBox.MouseClick += PictureBox_MouseClick;
        picBox.ControlRemoved += PictureBox_ControlRemoved;

        return picBox;
    }

    private void ThumbnailGenerationCompleted(object sender, EventArgs e)
    {
        _state.ThumbnailGenerationCompleted();
        SetUpdateDatabaseEnabledState(true);
        GC.Collect();
    }


    private void UpdatePicBoxList(object sender, UpdatePicBoxEventArgs e)
    {
        if (e?.PictureBoxModel != null)
        {
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Add(e.PictureBoxModel);

            flowLayoutPanel1.ResumeLayout(true);
        }
    }

    private void SetUpdateDatabaseEnabledState(bool enabled)
    {
        btnOptimize.Enabled = enabled;
        btnScanDirectory.Enabled = enabled;

        if (enabled)
        {
            btnGenerate.Text = "Generate";
            btnGenerate.Tag = null;
            btnGenerate.Enabled = true;
        }
        else
        {
            btnGenerate.Text = "Cancel";
            btnGenerate.Tag = _tokenSource;
            btnGenerate.Enabled = true;
        }
    }

    private void OptimizeDatabaseComplete(object sender, EventArgs e)
    {
        SetUpdateDatabaseEnabledState(true);
    }

    private IEnumerable<PictureBoxModel> GenerateThumbnailModels()
    {
        var backColor = AppSettings.MainWindowBackgroundColor;
        var modelList = new List<PictureBoxModel>();

        bool randomizeImageCollection = AppSettings.AutoRandomizeCollection;
        var imgRefList = _imageLoaderService.GenerateThumbnailList(randomizeImageCollection);

        if (imgRefList.Count > _maxThumbnails)
        {
            imgRefList = imgRefList.Take(_maxThumbnails).ToList();
        }

        foreach (var element in imgRefList)
        {
            var model = new PictureBoxModel
            {
                BackColor = backColor,
                SourceImagePath = element.CompletePath,
                ThumbnailSize = new Size(_thumbnailSize, _thumbnailSize),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            modelList.Add(model);
        }

        return modelList;
    }

    private void PictureBox_ControlRemoved(object sender, ControlEventArgs e)
    {
        if (sender is PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
                pictureBox.Dispose();
            }

            pictureBox?.Dispose();
        }
    }

    private void PictureBox_MouseClick(object sender, MouseEventArgs e)
    {
        if (!(sender is PictureBox pictureBox) || e.Button != MouseButtons.Left)
        {
            return;
        }

        if (pictureBox.Tag is string filename)
        {
            var fullScaleImage = _thumbnailService.GetFullScaleImage(filename);
            _maximizedImgFilename = filename;
            picBoxMaximized.Image = fullScaleImage;
        }

        picBoxMaximized.Visible = true;
        flowLayoutPanel1.Visible = false;
    }

    private void btnSettings_Click(object sender, EventArgs e)
    {
        var frmSettings = FormFactory.CreateSettingsForm(new ThumbnailSettings(_applicationSettings));
        if (frmSettings.ShowDialog(this) == DialogResult.OK)
        {
            _maxThumbnails = AppSettings.MaxThumbnails;
            _thumbnailSize = ValidateThumbnailSize(AppSettings.ThumbnailSize);
            _applicationSettings.SaveSettings();
        }

        frmSettings.Dispose();
        GC.Collect();
    }

    private int ValidateThumbnailSize(int size)
    {
        const int defVal = 256;
        const int minVal = 64;
        const int maxVal = 512;

        if (size < minVal || size > maxVal)
        {
            return defVal;
        }

        int index = minVal;

        while (index < maxVal)
        {
            if (size - index == 0)
            {
                return size;
            }

            index <<= 1;
        }

        return defVal;
    }

    private void HideMaximizedView()
    {
        picBoxMaximized.Visible = false;
        flowLayoutPanel1.Visible = true;
    }

    private void btnScanDirectory_Click(object sender, EventArgs e)
    {
        _thumbnailScan = new ThumbnailScanDirectory(_thumbnailService);
        var frmDirectoryScan = FormFactory.CreateModalForm(_thumbnailScan);
        frmDirectoryScan.FormClosed += FrmDirectoryScan_FormClosed;


        frmDirectoryScan.ShowDialog(this);
        frmDirectoryScan.Dispose();
        GC.Collect();
    }

    private void FrmDirectoryScan_FormClosed(object sender, FormClosedEventArgs e)
    {
        _thumbnailScan?.OnFormClosed();
        _thumbnailScan = null;
    }

    private void picBoxMaximized_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            HideMaximizedView();
        }
        else if (e.Button == MouseButtons.Right)
        {
            var menuPos = e.Location;
            contextMenuFullSizeImg.Show(picBoxMaximized, menuPos);
        }
    }

    private void menuItemOpenInDefApp_Click(object sender, EventArgs e)
    {
        if (!ApplicationIOHelper.OpenImageInDefaultAplication(_maximizedImgFilename))
        {
            MessageBox.Show($"Failed to open file: {_maximizedImgFilename}", Resources.Language.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void menuItemBookmark_Click(object sender, EventArgs e)
    {
        using (var scope = _scope.BeginLifetimeScope())
        {
            var fi = new FileInfo(_maximizedImgFilename);
            var imgRef = new ImageReference
            {
                CompletePath = _maximizedImgFilename,
                Size = fi.Length,
                CreationTime = fi.CreationTime,
                LastAccessTime = fi.LastAccessTime,
                LastWriteTime = fi.LastWriteTime,
                FileName = fi.Name,
                Directory = fi.DirectoryName
            };

            var formAddBookmark = scope.Resolve<FormAddBookmark>();
            formAddBookmark.Init(contextMenuFullSizeImg.Location, imgRef);
            formAddBookmark.ShowDialog(this);
        }
    }

    private void menuItemCopyPath_Click(object sender, EventArgs e)
    {
        Clipboard.Clear();
        Clipboard.SetText(_maximizedImgFilename);
    }

    private void btnOptimize_Click(object sender, EventArgs e)
    {
        Log.Debug("Optimize Clicked but not yet implemented");

        //SetUpdateDatabaseEnabledState(false);
        //await _thumbnailService.OptimizeDatabaseAsync().ConfigureAwait(false);
        //if (!IsDisposed)
        //    Invoke(new EventHandler(OptimizeDatabaseComplete));
    }

    // Instantiate a SafeHandle instance.


    // Protected implementation of Dispose pattern.
    protected override void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _formIsDisposing = true;
            _state.IsDisposing = true;
            // Free any other managed objects here.
            //
        }

        // Free any unmanaged objects here.
        //

        _disposed = true;

        // Call base class implementation.
        base.Dispose(disposing);
    }

    private class ThumbnailFormState
    {
        public bool IsDisposing { get; set; }
        public bool IsGeneratingThumbnails { get; private set; }

        public bool CanClose => !IsGeneratingThumbnails;

        public void StartingThumbnailScan()
        {
            IsGeneratingThumbnails = true;
        }

        public void ThumbnailGenerationCompleted()
        {
            IsGeneratingThumbnails = false;
        }
    }
}