using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageViewer.DataContracts;
using ImageViewer.Events;
using ImageViewer.Models;
using ImageViewer.Properties;
using ImageViewer.Services;
using ImageViewer.UserControls;
using ImageViewer.Utility;
using Serilog;

namespace ImageViewer
{
    public partial class FormThumbnailView : Form
    {
        private readonly ThumbnailService _thumbnailService;
        private readonly ImageLoaderService _imageLoaderService;
        private string _maximizedImgFilename;
        private int _maxThumbnails;
        private List<Control> _pictureBoxList;
        private ThumbnailScanDirectory _thumbnailScan;
        private int _thumbnailSize;
        private readonly FormAddBookmark _formAddBookmark;
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly ImageCacheService _imageCacheService;

        public FormThumbnailView(FormAddBookmark formAddBookmark, ApplicationSettingsService applicationSettingsService, ImageCacheService imageCacheService, ThumbnailService thumbnailService, ImageLoaderService imageLoaderService)
        {
            _formAddBookmark = formAddBookmark;
            _applicationSettingsService = applicationSettingsService;
            _imageCacheService = imageCacheService;
            _thumbnailService = thumbnailService;
            _imageLoaderService = imageLoaderService;
            _thumbnailSize = ValidateThumbnailSize(_applicationSettingsService.Settings.ThumbnailSize);
            _maxThumbnails = _applicationSettingsService.Settings.MaxThumbnails;
            _applicationSettingsService.OnSettingsSaved += _applicationSettingsService_OnSettingsSaved;
            _thumbnailService.LoadThumbnailDatabase();
            InitializeComponent();
        }

        private void _applicationSettingsService_OnSettingsSaved(object sender, EventArgs e)
        {
            _applicationSettingsService.LoadSettings();
            ImageViewApplicationSettings appSettings = _applicationSettingsService.Settings;
            flowLayoutPanel1.BackColor = appSettings.MainWindowBackgroundColor.ToColor();
            picBoxMaximized.BackColor = appSettings.MainWindowBackgroundColor.ToColor();
        }

        private void FormThumbnailView_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            ImageViewApplicationSettings appSettings = _applicationSettingsService.Settings;
            if (appSettings.ThumbnailFormSize != null && appSettings.ThumbnailFormLocation != null)
            {
                RestoreFormState.SetFormSizeAndPosition(this, appSettings.ThumbnailFormSize.ToSize(), appSettings.ThumbnailFormLocation.ToPoint(), Screen.PrimaryScreen.WorkingArea);
            }

            Closing += FormThumbnailView_Closing;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            flowLayoutPanel1.BackColor = appSettings.MainWindowBackgroundColor.ToColor();
            picBoxMaximized.BackColor = appSettings.MainWindowBackgroundColor.ToColor();
            UpdateStyles();
        }

        private void FormThumbnailView_Closing(object sender, CancelEventArgs e)
        {
            ImageViewApplicationSettings appSettings = _applicationSettingsService.Settings;
            appSettings.ThumbnailFormLocation = PointDataModel.CreateFromPoint(Location);
            appSettings.ThumbnailFormSize = SizeDataModel.CreateFromSize(Size);
            _applicationSettingsService.SaveSettings();
            Controls.Clear();
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            //if (_imageLoaderService.ImageReferenceList == null)
            //    return;



            HideMaximizedView();
            SetUpdateDatabaseEnabledState(false);
            flowLayoutPanel1.Controls.Clear();

            btnGenerate.Enabled = false;

            try
            {
                await Task.Factory.StartNew(BindAndLoadThumbnails);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error(ex, "Error in generate thumbnails");
            }

            btnGenerate.Enabled = true;
            Refresh();
        }

        private void BindAndLoadThumbnails()
        {
            Invoke(new EventHandler(CleanupPictureControlObjects));
            _pictureBoxList = GenerateThumbnails();

            if (!IsDisposed)
                Invoke(new EventHandler(UpdatePictureBoxList));

            _thumbnailService.SaveThumbnailDatabase();
            GC.Collect();
        }

        private void CleanupPictureControlObjects(object sender, EventArgs e)
        {
            if (_pictureBoxList != null)
            {
                _pictureBoxList.ForEach(x =>
                {
                    for (var i = x.Controls.Count - 1; i >= 0; i--)
                    {
                        x.Controls[i].Dispose();
                    }
                    x.Dispose();
                });
                _pictureBoxList.Clear();
            }
            GC.Collect();
        }

        private void UpdatePictureBoxList(object sender, EventArgs e)
        {
            if (_pictureBoxList == null) return;
            flowLayoutPanel1.Controls.AddRange(_pictureBoxList.ToArray());
            SetUpdateDatabaseEnabledState(true);

            GC.Collect();
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

        private List<Control> GenerateThumbnails()
        {
            var backColor = _applicationSettingsService.Settings.MainWindowBackgroundColor.ToColor();
            var pictureBoxes = new List<Control>();
            bool randomizeImageCollection = _applicationSettingsService.Settings.AutoRandomizeCollection;
            var imgRefList = _imageLoaderService.GenerateThumbnailList(randomizeImageCollection);
            int items = 0;
            foreach (ImageReferenceElement element in imgRefList)
            {
                var pictureBox = new PictureBox
                {
                    Image = _thumbnailService.GetThumbnail(element.CompletePath),
                    Width = _thumbnailSize,
                    Height = _thumbnailSize,
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = backColor,
                    Tag = element.CompletePath
                };

                if (pictureBox.Image == null)
                    continue;

                pictureBox.MouseClick += PictureBox_MouseClick;
                pictureBoxes.Add(pictureBox);

                items++;
                if (items > _maxThumbnails)
                    return pictureBoxes;
            }
            return pictureBoxes;
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!(sender is PictureBox pictureBox) || e.Button != MouseButtons.Left)
                return;

            if (pictureBox.Tag is string filename)
            {
                Image fullScaleImage = _imageCacheService.GetImageFromCache(filename);
                _maximizedImgFilename = filename;
                picBoxMaximized.Image = fullScaleImage;
            }

            picBoxMaximized.Visible = true;
            flowLayoutPanel1.Visible = false;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Form frmSettings = FormFactory.CreateSettingsForm(new ThumbnailSettings(_applicationSettingsService));
            if (frmSettings.ShowDialog(this) == DialogResult.OK)
            {
                _maxThumbnails = _applicationSettingsService.Settings.MaxThumbnails;
                _thumbnailSize = ValidateThumbnailSize(_applicationSettingsService.Settings.ThumbnailSize);
                _applicationSettingsService.SaveSettings();
            }
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
            _thumbnailService.SaveThumbnailDatabase();
            _thumbnailScan = new ThumbnailScanDirectory(_thumbnailService);
            Form frmDirectoryScan = FormFactory.CreateModalForm(_thumbnailScan);
            frmDirectoryScan.FormClosed += FrmDirectoryScan_FormClosed;

            frmDirectoryScan.ShowDialog(this);
            _thumbnailScan = null;
            GC.Collect();
            _thumbnailService.LoadThumbnailDatabase();
        }

        private void FrmDirectoryScan_FormClosed(object sender, FormClosedEventArgs e)
        {
            _thumbnailScan?.OnFormClosed();
            _thumbnailScan = null;
        }

        private void FormThumbnailView_FormClosing(object sender, FormClosingEventArgs e)
        {
            _thumbnailService.SaveThumbnailDatabase();
            flowLayoutPanel1.Controls.Clear();
            if (_pictureBoxList != null)
            {
                foreach (var control in _pictureBoxList)
                {
                    control.Dispose();
                }
            }
            _pictureBoxList = null;
            GC.Collect();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuItemBookmark_Click(object sender, EventArgs e)
        {
            var fi = new FileInfo(_maximizedImgFilename);
            var imgRef = new ImageReferenceElement
            {
                CompletePath = _maximizedImgFilename,
                Size = fi.Length,
                CreationTime = fi.CreationTime,
                LastAccessTime = fi.LastAccessTime,
                LastWriteTime = fi.LastWriteTime,
                FileName = fi.Name,
                Directory = fi.DirectoryName
            };

            _formAddBookmark.Init(contextMenuFullSizeImg.Location, imgRef);
            _formAddBookmark.ShowDialog(this);
        }

        private void menuItemCopyPath_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(_maximizedImgFilename);
        }

        private async void btnOptimize_Click(object sender, EventArgs e)
        {
            SetUpdateDatabaseEnabledState(false);

            await Task.Run(async () =>
            {
                await _thumbnailService.OptimizeDatabaseAsync();
                if (!IsDisposed)
                    Invoke(new EventHandler(OptimizeDatabaseComplete));
            });
        }
    }
}