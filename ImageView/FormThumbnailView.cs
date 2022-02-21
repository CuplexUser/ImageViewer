using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using ImageProcessor;
using ImageViewer.DataContracts;
using ImageViewer.Delegates;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Properties;
using ImageViewer.Services;
using ImageViewer.UserControls;
using ImageViewer.Utility;
using Serilog;

namespace ImageViewer
{
    public partial class FormThumbnailView : Form, IDisposable
    {
        private readonly ApplicationSettingsService _applicationSettings;
        private readonly ImageLoaderService _imageLoaderService;
        private readonly ILifetimeScope _scope;
        private readonly ThumbnailService _thumbnailService;
        private string _maximizedImgFilename;
        private int _maxThumbnails;
        private ThumbnailScanDirectory _thumbnailScan;
        private int _thumbnailSize;
        private bool formIsDisposing;
        private readonly ThumbnailFormState _state = new ThumbnailFormState();


        // Flag: Has Dispose already been called?
        private bool disposed;
        private UpdatePicBoxListEventHandler _updatePicBoxList;


        private ApplicationSettingsModel AppSettings
        {
            get => _applicationSettings?.Settings;
        }

        private class ThumbnailFormState
        {
            public bool IsDisposing { get; set; }
            public bool IsGeneratingThumbnails { get; private set; }
            public bool CanClose
            {
                get { return !IsGeneratingThumbnails; }
            }



            public ThumbnailFormState()
            {

            }

            public void StartingThumbnailScan()
            {
                IsGeneratingThumbnails = true;
            }

            public void ThumbnailGenerationCompleted()
            {
                IsGeneratingThumbnails = false;
            }


        }

        public FormThumbnailView(ApplicationSettingsService applicationSettings, ThumbnailService thumbnailService, ImageLoaderService imageLoaderService, ILifetimeScope scope)
        {
            _applicationSettings = applicationSettings;
            _thumbnailService = thumbnailService;
            _imageLoaderService = imageLoaderService;
            _scope = scope;
            _updatePicBoxList += UpdatePicBoxList;


            if (applicationSettings == null)
            {
                throw new NullReferenceException(Resources.AppApplicationSettingsServiceNull);
            }

            if (_thumbnailService == null)
            {
                throw new NullReferenceException(Resources.ThumbnailServiceNull);
            }

            _thumbnailSize = ValidateThumbnailSize(applicationSettings.Settings.ThumbnailSize);
            _maxThumbnails = AppSettings.MaxThumbnails;
            _applicationSettings.OnSettingsSaved += ApplicationSettingsOnSettingsSaved;
            _thumbnailService.LoadThumbnailDatabase();

            InitializeComponent();
        }

        private void ApplicationSettingsOnSettingsSaved(object sender, EventArgs e)
        {
            _applicationSettings.LoadSettings();
            var appSettings = _applicationSettings.Settings;
            flowLayoutPanel1.BackColor = appSettings.MainWindowBackgroundColor;
            picBoxMaximized.BackColor = appSettings.MainWindowBackgroundColor;
        }

        private void FormThumbnailView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            FormStateManager.RestoreFormState(AppSettings, this);

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

            formIsDisposing = true;
            var appSettings = AppSettings;
            FormStateManager.SaveFormState(appSettings, this);
            _applicationSettings.SaveSettings();
        }

        private IEnumerable<Control> GetControlTree(Control root)
        {
            var controls = new List<Control> { root };

            if (!root.HasChildren) return controls;
            for (int i = 0; i < root.Controls.Count; i++)
            {
                var controlList = GetControlTree(root.Controls[i]);
                controls.AddRange(controlList);
            }

            return controls;
        }


        // Generate Thumbnail list
        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            if (_imageLoaderService.ImageReferenceList == null)
            {
                MessageBox.Show(@"Cant generate thumbnails without any image source loaded.", "No Images loaded!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            HideMaximizedView();
            SetUpdateDatabaseEnabledState(false);
            flowLayoutPanel1.Controls.Clear();

            btnGenerate.Enabled = false;
            _state.StartingThumbnailScan();

            try
            {
                await BindAndLoadThumbnailsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(ex, "Error in generate thumbnails");
            }

            btnGenerate.Enabled = true;
            Refresh();
        }

        private async Task BindAndLoadThumbnailsAsync()
        {
            var modelList = GenerateThumbnailModels();
            ImageFactory imgFactory = new ImageFactory(MetaDataMode.None);

            // Generate thumbnails
            await Task.Factory.StartNew(() =>
            {
                var picBoxList = new List<PictureBox>();
                foreach (PictureBoxModel model in modelList)
                {
                    var picBox = CreatePictureBox(model);

                    // Perform thumbnail generation
                    picBox.Image = imgFactory.Load(model.SourceImagePath).Resize(model.ThumbnailSize).Image;
                    //picBoxList.Add(picBox);

                    if (formIsDisposing)
                        break;

                    if (!formIsDisposing)
                        Invoke(new UpdatePicBoxListEventHandler(UpdatePicBoxList), this, new UpdatePicBoxEventArgs(picBox));
                }

                if (!formIsDisposing)
                    Invoke(new EventHandler(ThumbnailGenerationCompleted));

            });

            if (!formIsDisposing)
            {
                await _thumbnailService.SaveThumbnailDatabaseAsync();
                GC.Collect();
            }

        }

        private PictureBox CreatePictureBox(PictureBoxModel model)
        {
            var picBox = new PictureBox
            {
                Width = model.ThumbnailSize.Width,
                Height = model.ThumbnailSize.Height,
                BorderStyle = model.BorderStyle,
                SizeMode = model.SizeMode,
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
            if (e == null) return;

            flowLayoutPanel1.Controls.Add(e.PictureBoxModel);
        }

        private void SetUpdateDatabaseEnabledState(bool enabled)
        {
            btnOptimize.Enabled = enabled;
            btnScanDirectory.Enabled = enabled;
        }

        private void OptimizeDatabaseComplete(object sender, EventArgs e)
        {
            SetUpdateDatabaseEnabledState(true);
        }

        private List<PictureBoxModel> GenerateThumbnailModels()
        {
            var backColor = AppSettings.MainWindowBackgroundColor;
            var modelList = new List<PictureBoxModel>();

            bool randomizeImageCollection = AppSettings.AutoRandomizeCollection;
            var imgRefList = _imageLoaderService.GenerateThumbnailList(randomizeImageCollection);

            if (imgRefList.Count > _maxThumbnails)
            {
                imgRefList = imgRefList.Take(_maxThumbnails).ToList();
            }

            foreach (ImageReference element in imgRefList)
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

        //private List<Control> GenerateThumbnails()
        //{
        //    var backColor = AppSettings.MainWindowBackgroundColor;
        //    var pictureBoxes = new List<Control>();
        //    bool randomizeImageCollection = AppSettings.AutoRandomizeCollection;
        //    var imgRefList = _imageLoaderService.GenerateThumbnailList(randomizeImageCollection);
        //    int items = 0;
        //    foreach (ImageReference element in imgRefList)
        //    {
        //        var pictureBox = new PictureBox
        //        {
        //            Width = _thumbnailSize,
        //            Height = _thumbnailSize,
        //            BorderStyle = BorderStyle.FixedSingle,
        //            SizeMode = PictureBoxSizeMode.Zoom,
        //            BackColor = backColor,
        //            Tag = element.CompletePath
        //        };

        //        //var x = new ImageFactory().Load(element.CompletePath).Resize(new Size(512, 512)).Image;
        //        //Task<Image>.Factory.StartNew(() => _thumbnailService.GetThumbnail(element.CompletePath));
        //        //pictureBox.Image =  _thumbnailService.GetThumbnail(element.CompletePath);
        //        pictureBox.ControlRemoved += PictureBox_ControlRemoved;

        //        if (pictureBox.Image == null)
        //        {
        //            pictureBox.Dispose();
        //            continue;
        //        }

        //        pictureBox.MouseClick += PictureBox_MouseClick;
        //        pictureBoxes.Add(pictureBox);


        //        items++;
        //        if (items > _maxThumbnails)
        //            return pictureBoxes;
        //    }

        //    return pictureBoxes;
        //}

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
                return;

            if (pictureBox.Tag is string filename)
            {
                Image fullScaleImage = _thumbnailService.GetFullScaleImage(filename); //_imageCacheService.GetImageFromCache(filename);
                _maximizedImgFilename = filename;
                picBoxMaximized.Image = fullScaleImage;
            }

            picBoxMaximized.Visible = true;
            flowLayoutPanel1.Visible = false;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Form frmSettings = FormFactory.CreateSettingsForm(new ThumbnailSettings(_applicationSettings));
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
                return defVal;

            int index = minVal;

            while (index < maxVal)
            {
                if (size - index == 0)
                    return size;

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
            Form frmDirectoryScan = FormFactory.CreateModalForm(_thumbnailScan);
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
                Point menuPos = e.Location;
                contextMenuFullSizeImg.Show(picBoxMaximized, menuPos);
            }
        }

        private void menuItemOpenInDefApp_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(_maximizedImgFilename);
            }
            catch (Win32Exception ex)
            {
                Log.Error(ex, "Win32Exception was thrown when trying to open {_maximizedImgFilename}", _maximizedImgFilename);
                MessageBox.Show(ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException ex)
            {
                Log.Error(ex, "ObjectDisposedException was thrown when trying to open {_maximizedImgFilename}", _maximizedImgFilename);
                MessageBox.Show(ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, "FileNotFoundException was thrown when trying to open {_maximizedImgFilename}", _maximizedImgFilename);
                MessageBox.Show(ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuItemBookmark_Click(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = _scope.BeginLifetimeScope())
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

        private async void btnOptimize_Click(object sender, EventArgs e)
        {
            SetUpdateDatabaseEnabledState(false);

            await _thumbnailService.OptimizeDatabaseAsync().ConfigureAwait(false);
            if (!IsDisposed)
                Invoke(new EventHandler(OptimizeDatabaseComplete));
        }

        // Instantiate a SafeHandle instance.


        // Protected implementation of Dispose pattern.
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                formIsDisposing = true;
                _state.IsDisposing = true;
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //

            disposed = true;

            // Call base class implementation.
            base.Dispose(disposing);
        }
    }
}