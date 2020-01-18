using Autofac;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.WindowsApi;
using ImageViewer.Collections;
using ImageViewer.DataContracts;
using ImageViewer.Events;
using ImageViewer.Library.CustomAttributes;
using ImageViewer.Library.EventHandlers;
using ImageViewer.Models;
using ImageViewer.Models.UserInteraction;
using ImageViewer.Properties;
using ImageViewer.Services;
using ImageViewer.UserControls;
using ImageViewer.Utility;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer
{
    [NotificationId("MainForm")]
    public partial class FormMain : Form
    {
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly BookmarkService _bookmarkService;
        private readonly FormAddBookmark _formAddBookmark;
        private readonly FormSettings _formSettings;
        private readonly FormState _formState = new FormState();
        private readonly ImageCacheService _imageCacheService;
        private readonly ImageLoaderService _imageLoaderService;
        private readonly List<FormImageView> _imageViewFormList;
        private readonly UserInteractionService _interactionService;
        private readonly PictureBox _pictureBoxAnimation = new PictureBox();
        private readonly ILifetimeScope _scope;
        private readonly string _windowTitle;
        private ImageViewApplicationSettings.ChangeImageAnimation _changeImageAnimation;
        private bool _dataReady;
        private FormBookmarks _formBookmarks;
        private FormThumbnailView _formThumbnailView;
        private FormWindows _formWindows;
        private bool _fullScreen;
        private ImageReferenceCollection _imageReferenceCollection;
        private bool _imageTransitionRunning;
        private int _imageViewFormIdCnt = 1;
        private bool _winKeyDown;
        private DateTime cursorMovedTime = DateTime.Now;
        private bool _cursorVisible = true;
        private int _hideCursorDelay;
        private Point cursorPosition = Point.Empty;
        private Rectangle pointerBox = new Rectangle(Point.Empty, new Size(25, 25));
        private readonly object _lockObj = new object();

        public FormMain(FormAddBookmark formAddBookmark, BookmarkService bookmarkService, FormSettings formSettings, ApplicationSettingsService applicationSettingsService, ImageCacheService imageCacheService, ImageLoaderService imageLoaderService,
            ILifetimeScope scope, UserInteractionService interactionService)
        {
            _formAddBookmark = formAddBookmark;
            _bookmarkService = bookmarkService;
            _formSettings = formSettings;
            _applicationSettingsService = applicationSettingsService;
            _applicationSettingsService.LoadSettings();

            _imageCacheService = imageCacheService;
            _imageLoaderService = imageLoaderService;
            _scope = scope;
            _interactionService = interactionService;

            InitializeComponent();
            _imageViewFormList = new List<FormImageView>();
            _windowTitle = "Image Viewer - " + Application.ProductVersion;
        }

        private bool ImageSourceDataAvailable => _dataReady && _imageLoaderService.ImageReferenceList != null;

        private bool CursorVisible
        {
            get => _cursorVisible;
            set
            {
                bool __lockWasTaken = false;
                try
                {
                    Monitor.Enter(_lockObj, ref __lockWasTaken);

                    if (!value && cursorMovedTime.AddMilliseconds(_hideCursorDelay) < DateTime.Now)
                    {
                        _cursorVisible = false;
                    }
                    else
                    {
                        _cursorVisible = true;
                    }
                }
                finally
                {
                    if (__lockWasTaken)
                        Monitor.Exit(_lockObj);
                }
            }
        }

        private void DisplaySlideshowStatus()
        {
            string tooltipText = timerSlideShow.Enabled ? $"Slideshow started with a delay of {_applicationSettingsService.Settings.SlideshowInterval / 1000} seconds per image." : "Slideshow stopped";

            toolTipSlideshowState.Active = true;
            toolTipSlideshowState.InitialDelay = 150;
            toolTipSlideshowState.AutoPopDelay = 2000;
            toolTipSlideshowState.AutomaticDelay = 500;
            toolTipSlideshowState.UseFading = true;
            toolTipSlideshowState.UseAnimation = true;
            toolTipSlideshowState.IsBalloon = true;
            toolTipSlideshowState.SetToolTip(pictureBox1, tooltipText);
            toolTipSlideshowState.Show(tooltipText, this);
            toolTipSlideshowState.Popup += ToolTipSlideshowState_Popup;
        }

        private bool ToggleSlideshow()
        {
            if (!ImageSourceDataAvailable)
            {
                return false;
            }

            if (timerSlideShow.Enabled)
            {
                timerSlideShow.Stop();
            }
            else
            {
                timerSlideShow.Interval = _applicationSettingsService.Settings.SlideshowInterval;
                timerSlideShow.Start();
            }

            //SyncUserControlStateWithAppSettings();
            ToggleSlideshowMenuState();

            return true;
        }

        private bool AllowApplicationExit()
        {
            if (!_applicationSettingsService.Settings.ConfirmApplicationShutdown)
            {
                return true;
            }

            var confirmExitUserControl = new ConfirmExitUserControl(_applicationSettingsService);
            var exitDialogForm = FormFactory.CreateModalSimpleDialog(confirmExitUserControl);

            return exitDialogForm.ShowDialog(this) == DialogResult.OK;
        }

        private void HandleImportDataComplete()
        {
            addBookmarkToolStripMenuItem.Enabled = true;
            SetImageReferenceCollection();
        }

        private void SyncUserControlStateWithAppSettings()
        {
            var settings = _applicationSettingsService.Settings;

            if (TopMost != settings.AlwaysOntop)
            {
                TopMost = settings.AlwaysOntop;
            }

            _hideCursorDelay = settings.AutoHideCursorDelay;
            topMostToolStripMenuItem.Checked = settings.AlwaysOntop;
            timerCursorVisible.Interval = settings.AutoHideCursorDelay;

            _changeImageAnimation = settings.NextImageAnimation;
            autoLoadPreviousFolderToolStripMenuItem.Enabled = settings.EnableAutoLoadFunctionFromMenu &&
                                                              !string.IsNullOrWhiteSpace(settings.LastFolderLocation);
            if (settings.MainWindowBackgroundColor != null)
            {
                pictureBox1.BackColor = settings.MainWindowBackgroundColor.ToColor();
                BackColor = settings.MainWindowBackgroundColor.ToColor();
            }

            if (settings.ExtendedAppSettings.FormStateDictionary.ContainsKey(this.GetType().Name))
            {
                var formState = settings.ExtendedAppSettings.FormStateDictionary[this.GetType().Name];
                RestoreFormState.SetFormSizeAndPosition(this, formState);
            }
        }

        private void ChangeImage(bool next)
        {
            if (_imageTransitionRunning)
            {
                return;
            }

            if (!ImageSourceDataAvailable || _imageReferenceCollection.ImageCount == 0)
            {
                return;
            }

            ImageReferenceElement imgRef;

            //Reset timer
            if (timerSlideShow.Enabled)
            {
                timerSlideShow.Stop();
                timerSlideShow.Start();
            }

            //Go Forward
            if (next)
            {
                _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideLeft;
                imgRef = _imageReferenceCollection.GetNextImage();
            }
            else
            {
                _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.SlideRight;
                imgRef = _imageReferenceCollection.GetPreviousImage();
            }

            LoadNewImageFile(imgRef);
            AddNextImageToCache(_imageReferenceCollection.PeekNextImage().CompletePath);
        }

        private void LoadNewImageFile(ImageReferenceElement imgRefElement)
        {
            try
            {
                pictureBox1.SizeMode = (PictureBoxSizeMode)_applicationSettingsService.Settings.PrimaryImageSizeMode;

                if (_applicationSettingsService.Settings.NextImageAnimation == ImageViewApplicationSettings.ChangeImageAnimation.None)
                {
                    _changeImageAnimation = ImageViewApplicationSettings.ChangeImageAnimation.None;
                }

                _pictureBoxAnimation.ImageLocation = null;

                if (pictureBox1.Image != null && _changeImageAnimation != ImageViewApplicationSettings.ChangeImageAnimation.None)
                {
                    _pictureBoxAnimation.Image = _imageCacheService.GetImageFromCache(imgRefElement.CompletePath);
                    _pictureBoxAnimation.Refresh();
                }
                else
                {
                    pictureBox1.Image = _imageCacheService.GetImageFromCache(imgRefElement.CompletePath);
                    pictureBox1.Refresh();
                }

                Text = _windowTitle + @" | " + GeneralConverters.GetFileNameFromPath(imgRefElement.CompletePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "FormMain.LoadNewImageFile(string imagePath) Error when trying to load file: {CompletePath} : {Message}", imgRefElement.CompletePath, ex.Message);
            }
        }

        private void AddNextImageToCache(string imagePath)
        {
            _imageCacheService.GetImageFromCache(imagePath);
        }

        private void SetImageReferenceCollection()
        {
            bool randomizeImageCollection = _applicationSettingsService.Settings.AutoRandomizeCollection;
            if (!_imageLoaderService.IsRunningImport && _imageLoaderService.ImageReferenceList != null)
            {
                _imageReferenceCollection = _imageLoaderService.GenerateImageReferenceCollection(randomizeImageCollection);
                if (_imageLoaderService.ImageReferenceList.Count > 0)
                {
                    _dataReady = true;
                }
            }
        }

        private async Task PerformImageTransition(Image currentImage, Image nextImage, ImageViewApplicationSettings.ChangeImageAnimation animation, int animationTime)
        {
            const int sleepTime = 1;
            _imageTransitionRunning = true;
            await Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds <= animationTime)
                {
                    long elapsedTime = stopwatch.ElapsedMilliseconds;

                    float factor = stopwatch.ElapsedMilliseconds / (float)animationTime;
                    Image transitionImage;
                    switch (animation)
                    {
                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideLeft:
                            transitionImage = ImageTransform.OffsetImagesHorizontal(currentImage, nextImage,
                                new Size(pictureBox1.Width, pictureBox1.Height), factor, false);
                            break;

                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideRight:
                            transitionImage = ImageTransform.OffsetImagesHorizontal(nextImage, currentImage,
                                new Size(pictureBox1.Width, pictureBox1.Height), factor, true);
                            break;

                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideDown:
                            transitionImage = ImageTransform.OffsetImagesVertical(nextImage, currentImage,
                                new Size(nextImage.Width, nextImage.Height), factor, true);
                            break;

                        case ImageViewApplicationSettings.ChangeImageAnimation.SlideUp:
                            transitionImage = ImageTransform.OffsetImagesVertical(currentImage, nextImage,
                                new Size(Math.Max(nextImage.Width, currentImage.Width), nextImage.Height), factor, false);
                            break;

                        case ImageViewApplicationSettings.ChangeImageAnimation.FadeIn:
                            int width = nextImage.Width;
                            int height = nextImage.Height;
                            var nextImageBitmap = new Bitmap(nextImage, new Size(width, height));
                            transitionImage = ImageTransform.SetImageOpacity(nextImageBitmap, factor);
                            break;

                        default:
                            return;
                    }

                    if (!Visible)
                    {
                        return;
                    }

                    try
                    {
                        pictureBox1.Image = transitionImage.Clone() as Image;
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, Resources.Failed_to_set_transition_image_over_current_image_);
                        MessageBox.Show(Resources.Failed_to_set_transition_image_over_current_image_,
                            Resources.Error_loading_new_image, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        _imageTransitionRunning = false;
                        return;
                    }
                    finally
                    {
                        transitionImage.Dispose();
                    }

                    elapsedTime = stopwatch.ElapsedMilliseconds - elapsedTime;

                    if (sleepTime - elapsedTime > 0)
                    {
                        Thread.Sleep(Convert.ToInt32(sleepTime - elapsedTime));
                    }
                }

                stopwatch.Stop();
                Log.Verbose("Image transition finished after " + stopwatch.ElapsedMilliseconds + " ms");
                Invoke(new EventHandler(ImageLoadComplete), this, new EventArgs());
            });

            pictureBox1.Image = nextImage.Clone() as Image;
            _imageTransitionRunning = false;
        }

        private void ToggleFullscreen()
        {
            if (_fullScreen)
            {
                _formState.Restore(this);
                menuStrip1.Visible = true;
                BackColor = _applicationSettingsService.Settings.MainWindowBackgroundColor.ToColor();
                ScreenSaver.Enable();
                Cursor.Show();
                CursorVisible = true;
            }
            else
            {
                _formState.Save(this);
                _formState.Maximize(this);
                menuStrip1.Visible = false;

                BackColor = Color.Black;
                Cursor.Hide();
                CursorVisible = false;
                ScreenSaver.Disable();
            }

            _fullScreen = !_fullScreen;
        }

        private void AutoArrangeOnSingleScreen()
        {
            if (_imageViewFormList.Count == 0)
            {
                return;
            }

            var screen = Screen.PrimaryScreen;
            int widthPerScreen = screen.WorkingArea.Width / _imageViewFormList.Count;
            int offset = 0;

            foreach (var formImage in _imageViewFormList)
            {
                formImage.Width = widthPerScreen;
                formImage.Height = screen.WorkingArea.Height;
                formImage.Left = offset;
                formImage.Top = 0;
                formImage.Focus();
                offset += widthPerScreen;
                formImage.ResetZoomAndRepaint();
            }
        }

        private void AutoArrangeOnMultipleScreens()
        {
            int index = 0;
            int windowsPerScreen = 2;

            int widthOffset = _applicationSettingsService.Settings.ScreenWidthOffset;
            int minXOffset = _applicationSettingsService.Settings.ScreenMinXOffset;

            foreach (var screen in Screen.AllScreens.OrderBy(s => s.Bounds.X))
            {
                if (screen.Primary)
                {
                    continue;
                }

                for (int i = 0; i < windowsPerScreen; i++)
                {
                    if (index >= _imageViewFormList.Count)
                    {
                        break;
                    }

                    var imageWindow = _imageViewFormList[index++];
                    if (imageWindow == null || imageWindow.IsDisposed)
                    {
                        continue;
                    }

                    imageWindow.Margin = new Padding(0);

                    int screenWidth = screen.Bounds.Width + widthOffset;
                    int screenMinx = screen.WorkingArea.X - minXOffset;

                    if (i == 0)
                    {
                        imageWindow.SetDesktopBounds(screenMinx, screen.WorkingArea.Y, screenWidth / 2,
                            screen.WorkingArea.Height);
                    }
                    else
                    {
                        imageWindow.SetDesktopBounds(screenMinx + screen.WorkingArea.Width / 2, screen.WorkingArea.Y,
                            screenWidth / 2, screen.WorkingArea.Height);
                    }

                    imageWindow.WindowState = FormWindowState.Normal;
                    imageWindow.ResetZoomAndRepaint();
                    imageWindow.Show();
                    imageWindow.Focus();
                }
            }
        }

        private void OpenImageInDefaultApp()
        {
            if (ImageSourceDataAvailable)
            {
                string currentFile = _imageReferenceCollection.CurrentImage.CompletePath;
                try
                {
                    Process.Start(currentFile);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in MainForm open image in default app");
                }
            }
        }

        private delegate void NativeThreadFunction();

        private delegate void NativeThreadFunctionUserInfo(object sender, UserInformationEventArgs e);

        private delegate void NativeThreadFunctionUserQuestion(object sender, UserQuestionEventArgs e);

        #region Form Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            DoubleBuffered = true;
            _applicationSettingsService.OnSettingsSaved += Instance_OnSettingsSaved;
            _applicationSettingsService.OnRegistryAccessDenied += Instance_OnRegistryAccessDenied;
            _imageLoaderService.OnImportComplete += Instance_OnImportComplete;
            _imageLoaderService.OnImageWasDeleted += _imageLoaderService_OnImageWasDeleted;

            _pictureBoxAnimation.LoadCompleted += pictureBoxAnimation_LoadCompleted;
            bool settingsLoaded = _applicationSettingsService.LoadSettings();

            if (!settingsLoaded)
            {
                // Problem. Settings could not be loaded due to deserialization error because the decryption used an invalid key which made protobuffer try to deserialize garbage.
                _applicationSettingsService.SaveSettings();
                _applicationSettingsService.LoadSettings();
                SyncUserControlStateWithAppSettings();
            }
            else
            {
                SyncUserControlStateWithAppSettings();

                try
                {
                    var fileConfig = _applicationSettingsService.Settings.ExtendedAppSettings;
                    if (fileConfig.FormStateDictionary.ContainsKey(GetType().Name))
                    {
                        var formState = fileConfig.FormStateDictionary[GetType().Name];
                        RestoreFormState.SetFormSizeAndPosition(this, formState);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                _changeImageAnimation = _applicationSettingsService.Settings.NextImageAnimation;
                timerSlideShow.Interval = _applicationSettingsService.Settings.SlideshowInterval;

                addBookmarkToolStripMenuItem.Enabled = false;
                Text = _windowTitle;
                ToggleSlideshowMenuState();
            }

            //Notification Service
            _interactionService.Initialize(this);
            _interactionService.UserInformationReceived += InteractionServiceUserInformationReceived;
            _interactionService.UserQuestionReceived += InteractionServiceUserQuestionReceived;
        }

        private void FormThumbnailView_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is Form form)
            {
                form.Dispose();
            }

            _formThumbnailView = null;
            GC.Collect();
        }

        private void Instance_OnImportComplete(object sender, ProgressEventArgs e)
        {
            Invoke(new NativeThreadFunction(HandleImportDataComplete));
        }

        private void _imageLoaderService_OnImageWasDeleted(object sender, ImageRemovedEventArgs e)
        {
        }

        private void Instance_OnRegistryAccessDenied(object sender, EventArgs e)
        {
        }

        private void Instance_OnSettingsSaved(object sender, EventArgs e)
        {
            SyncUserControlStateWithAppSettings();
        }

        private void InteractionServiceUserQuestionReceived(object sender, UserQuestionEventArgs e)
        {
            Invoke(new NativeThreadFunctionUserQuestion(ShowQuestionOnNativeThread), this, e);
        }

        private void InteractionServiceUserInformationReceived(object sender, UserInformationEventArgs e)
        {
            Invoke(new NativeThreadFunctionUserInfo(ShowInfoMessageOnNativeThread), this, e);
        }

        private void ShowInfoMessageOnNativeThread(object sender, UserInformationEventArgs e)
        {
            MessageBox.Show(e.UserInformation.Message, e.UserInformation.Label, e.UserInformation.Buttons, e.UserInformation.Icon);
        }

        private void ShowQuestionOnNativeThread(object sender, UserQuestionEventArgs e)
        {
            var result = MessageBox.Show(e.UserQuestion.Message, e.UserQuestion.Label, e.UserQuestion.Buttons, e.UserQuestion.Icon);

            if (result == DialogResult.OK)
            {
                e.UserQuestion.OkResponse.Invoke();
            }
            else
            {
                e.UserQuestion.CancelResponse.Invoke();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowApplicationExit())
            {
                e.Cancel = true;
                return;
            }

            if (!ScreenSaver.ScreenSaverEnabled)
            {
                ScreenSaver.Enable();
            }

            timerSlideShow.Enabled = false;
            _bookmarkService.SaveBookmarks();
            _applicationSettingsService.UpdateOrInsertFormState(RestoreFormState.GetFormState(this));
            _applicationSettingsService.SetSettingsStateModified();
            _applicationSettingsService.SaveSettings();
        }

        private void imageViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _imageViewFormList.Remove(sender as FormImageView);
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _fullScreen)
            {
                ToggleFullscreen();
            }
            else if (e.Alt && e.KeyCode == Keys.Enter)
            {
                ToggleFullscreen();
            }

            //if (e.KeyCode == Keys.F11)
            //{
            //    ToggleFullscreen();
            //    e.Handled = true;
            //    return;
            //}

            if (_imageTransitionRunning)
            {
                return;
            }

            if (_winKeyDown || !ImageSourceDataAvailable)
            {
                return;
            }

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Enter)
            {
                ChangeImage(true);
            }
            else if (e.KeyCode == Keys.Left)
            {
                ChangeImage(false);
            }
            else if (e.KeyCode == Keys.Space)
            {
                ToggleSlideshow();
                DisplaySlideshowStatus();
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            _winKeyDown = e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var settings = _applicationSettingsService.Settings;
            switch (e.Button)
            {
                case MouseButtons.XButton1:
                case MouseButtons.Left:
                    ChangeImage(true);
                    break;

                case MouseButtons.Right:
                case MouseButtons.XButton2:
                    ChangeImage(false);
                    break;

                case MouseButtons.Middle:
                    if (settings.ToggleSlideshowWithThirdMouseButton && ToggleSlideshow())
                    {
                        DisplaySlideshowStatus();
                    }

                    break;

                case MouseButtons.None:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ToolTipSlideshowState_Popup(object sender, PopupEventArgs e)
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1500);
                toolTipSlideshowState.Active = false;
            });
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            _formWindows?.RestoreFocusToMainForm();
        }

        #endregion Form Events

        #region Form Controls Events

        private void openInDefaultApplicationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenImageInDefaultApp();
        }

        private void DeleteImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ImageSourceDataAvailable)
            {
                return;
            }

            bool slideshowEnabled = timerSlideShow.Enabled;
            if (slideshowEnabled)
            {
                timerSlideShow.Stop();
                timerSlideShow.Enabled = false;
            }
            string currentFilePath = _imageReferenceCollection.CurrentImage.CompletePath;

            var result = MessageBox.Show($@"Are you sure you want to delete the current image? 
Image path: { currentFilePath } ", @"Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ImageReferenceElement imgRefElement = _imageReferenceCollection.CurrentImage;
                bool imgRemoved = _imageLoaderService.PermanentlyRemoveFile(imgRefElement);
                if (!imgRemoved)
                {
                    MessageBox.Show($@"Unable to delete file {imgRefElement.CompletePath}", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    timerSlideShow.Enabled = slideshowEnabled;
                    return;
                }

                // A cached copy of the image remains independent of file deletion. so we can delete the file while still displaying it.
                // The image cache service will be notified of the deletion and remove it from cache
                // It will also be removed from the image collection set so moving forwards and backwards will not create any problems.
                // Same with FormImageView instances.

                _pictureBoxAnimation.SizeMode = PictureBoxSizeMode.CenterImage;
                _pictureBoxAnimation.Image = Resources.No_Camera_icon;

                if (slideshowEnabled)
                {
                    timerSlideShow.Start();
                }
                else
                {
                    DelayOperation.DelayAction(SkipToNextImageImage, 500);
                }
            }
        }

        private void SkipToNextImageImage()
        {
            ChangeImage(true);
        }

        private void imageDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pictureBox1.SizeMode = (PictureBoxSizeMode)_applicationSettingsService.Settings.PrimaryImageSizeMode;
        }

        private async void pictureBoxAnimation_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (_imageTransitionRunning)
            {
                return;
            }

            var settings = _applicationSettingsService.Settings;
            var currentImage = pictureBox1.Image.Clone() as Image;
            var nextImage = _pictureBoxAnimation.Image.Clone() as Image;
            _pictureBoxAnimation.Image = null;

            int animationTime = settings.ImageTransitionTime;
            await PerformImageTransition(currentImage, nextImage, settings.NextImageAnimation, animationTime);

            currentImage?.Dispose();

            nextImage?.Dispose();

            if (_pictureBoxAnimation.Image != null)
            {
                _pictureBoxAnimation.Image.Dispose();
                _pictureBoxAnimation.Image = null;
            }

            SyncUserControlStateWithAppSettings();
            GC.Collect();
        }

        private void timerSlideShow_Tick(object sender, EventArgs e)
        {
            if (_imageTransitionRunning)
            {
                return;
            }

            timerSlideShow.Interval = _applicationSettingsService.Settings.SlideshowInterval;
            LoadNewImageFile(_imageReferenceCollection.GetNextImage());
        }

        private void formSetSlideshowInterval_OnIntervalChanged(object sender, IntervalEventArgs e)
        {
            timerSlideShow.Interval = e.Interval * 1000;
            _applicationSettingsService.Settings.SlideshowInterval = timerSlideShow.Interval;
        }

        #endregion Form Controls Events

        #region Main Menu Functions

        private void openInDefaultApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenImageInDefaultApp();
        }

        private void autoLoadPreviousFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formLoad = new FormLoad(_imageLoaderService, _interactionService);
            formLoad.SetBasePath(_applicationSettingsService.Settings.LastFolderLocation);
            formLoad.ShowDialog(this);
        }

        private async void menuItemLoadBookmarkedImages_Click(object sender, EventArgs e)
        {
            bool result = await _imageLoaderService.RunBookmarkImageImport();

            if (result)
            {
                MessageBox.Show(@"Loaded all bookmarks as source images", @"Import complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Log.Warning("RunBookmarkImageImport returned false");
                MessageBox.Show(@"Failed to load bookmarked images as source images", @"Import failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void topMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _applicationSettingsService.Settings.AlwaysOntop = !_applicationSettingsService.Settings.AlwaysOntop;
            _applicationSettingsService.SaveSettings();
            SyncUserControlStateWithAppSettings();
        }

        private void showFullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        private void setImageScalingModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formImageSizeModes = new FormImageSizeModes(_applicationSettingsService);

            if (formImageSizeModes.ShowDialog(this) == DialogResult.OK)
            {
                _pictureBoxAnimation.SizeMode = formImageSizeModes.ImageSizeMode;
            }
        }

        private async void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var updateService = _scope.Resolve<UpdateService>();
                var isLatest = await updateService.IsLatestVersion();
                if (isLatest)
                {
                    MessageBox.Show(@"This is the latest version available", @"Update check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (MessageBox.Show(@"There is a newer version available, do you want to update?", @"Update", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    await updateService.DownloadAndRunLatestVersionInstaller();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception while checking for updates");
                MessageBox.Show(ex.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileBrowser = _scope.Resolve<FileBrowser>();
            fileBrowser.ShowDialog(this);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ImageSourceDataAvailable)
            {
                return;
            }

            SyncUserControlStateWithAppSettings();
            timerSlideShow.Interval = _applicationSettingsService.Settings.SlideshowInterval;
            timerSlideShow.Start();
            ToggleSlideshowMenuState();
        }

        private void ToggleSlideshowMenuState()
        {
            startToolStripMenuItem.Enabled = !timerSlideShow.Enabled;
            stopToolStripMenuItem.Enabled = timerSlideShow.Enabled;
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timerSlideShow.Enabled = false;
            ToggleSlideshowMenuState();
        }

        private void setIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formSetSlideshowInterval = new FormSetSlideshowInterval(timerSlideShow.Interval / 1000);
            formSetSlideshowInterval.OnIntervalChanged += formSetSlideshowInterval_OnIntervalChanged;
            formSetSlideshowInterval.ShowDialog(this);
        }

        private void openSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _formSettings.ShowDialog(this);

            foreach (var imageView in _imageViewFormList)
            {
                imageView.ReloadSettings();
            }
        }

        private void randomizeCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetImageReferenceCollection();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        private void copyFilepathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ImageSourceDataAvailable)
            {
                Clipboard.Clear();
                Clipboard.SetText(_imageReferenceCollection.CurrentImage.CompletePath);
            }
        }

        private void copyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                Clipboard.SetImage(pictureBox1.Image);
            }
        }

        private void addBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_bookmarkService.BookmarkManager == null)
            {
                MessageBox.Show(Resources.Please_unlock_bookmarks_first);
                return;
            }

            if (ImageSourceDataAvailable)
            {
                if (_fullScreen)
                {
                    Cursor.Show();
                }

                var starupPosition = new Point(Location.X, Location.Y);
                starupPosition.X += addBookmarkToolStripMenuItem.Width;
                starupPosition.Y += addBookmarkToolStripMenuItem.Height + (Height - ClientSize.Height);

                if (_imageReferenceCollection.CurrentImage != null)
                {
                    _formAddBookmark.Init(starupPosition, _imageReferenceCollection.CurrentImage);
                    _formAddBookmark.ShowDialog(this);
                    if (_fullScreen)
                    {
                        Cursor.Hide();
                    }
                }
            }
        }

        private void openBookmarksToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (_formBookmarks == null)
            {
                _formBookmarks = new FormBookmarks(_bookmarkService, _bookmarkService.BookmarkManager, _applicationSettingsService);
                _formBookmarks.Show();
                _formBookmarks.Focus();
                _formBookmarks.Closed += (o, args) =>
                {
                    _formBookmarks = null;
                    GC.Collect();
                };
            }
            else
            {
                _formBookmarks.WindowState = FormWindowState.Normal;
                _formBookmarks.Show();
                _formBookmarks.Focus();
            }
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var imageViewForm = new FormImageView(_imageViewFormIdCnt++, _formAddBookmark, _bookmarkService.BookmarkManager, _applicationSettingsService, _imageCacheService, _imageLoaderService);
            _imageViewFormList.Add(imageViewForm);
            imageViewForm.FormClosed += imageViewForm_FormClosed;
            imageViewForm.Show();

            if (_formWindows != null && !_formWindows.IsDisposed)
            {
                _formWindows.Subscribe(imageViewForm);
            }

            Focus();
        }

        private void windowsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_formWindows == null || _formWindows.IsDisposed)
            {
                _formWindows = new FormWindows(_applicationSettingsService);
                _formWindows.SubscribeToList(_imageViewFormList);
            }

            if (!_formWindows.Visible)
            {
                _formWindows.Show(this);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoArrangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length == 1)
            {
                AutoArrangeOnSingleScreen();
            }
            else
            {
                AutoArrangeOnMultipleScreens();
            }

            Show();
            Focus();
        }

        private void showAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var imageWindow in _imageViewFormList)
            {
                if (imageWindow == null || imageWindow.IsDisposed)
                {
                    continue;
                }

                imageWindow.Show();
                imageWindow.Focus();
            }

            Show();
            Focus();
        }

        private void hideAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var imageWindow in _imageViewFormList)
            {
                if (imageWindow != null && !imageWindow.IsDisposed)
                {
                    imageWindow.Hide();
                }
            }
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_imageViewFormList.Count == 0)
            {
                return;
            }

            if (MessageBox.Show(this, Resources.Are_you_sure_you_want_to_close_all_windows_,
                    Resources.Close_all_windows_, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK)
            {
                var imageWindowQueue = new Queue<FormImageView>(_imageViewFormList);
                while (imageWindowQueue.Count > 0)
                {
                    var imageWindow = imageWindowQueue.Dequeue();
                    imageWindow.Close();
                }
            }
        }

        private void ImageLoadComplete(object sender, EventArgs e)
        {
            if (!timerSlideShow.Enabled)
            {
                return;
            }

            if (_formWindows == null)
            {
                _formWindows = new FormWindows(_applicationSettingsService);
                _formWindows.SubscribeToList(_imageViewFormList);
            }

            _formWindows.RestoreFocusOnPreviouslyActiveImageForm();
        }

        private void menuItemOpenImage_Click(object sender, EventArgs e)
        {
            // TODO fix bad code when opening a single image
            openFileDialog1.Filter = Resources.ImageFormatFilter;
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    string fileName = openFileDialog1.FileName;
                    var imgRefElement = _imageLoaderService.ImportSingleImage(fileName, ref _imageReferenceCollection);
                    LoadNewImageFile(imgRefElement);
                    if (pictureBox1.Image != null)
                    {
                        addBookmarkToolStripMenuItem.Enabled = true;
                    }

                    _dataReady = true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Open Single Image failed");
                }
            }
        }

        private void openThumbnailsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (_formThumbnailView == null)
            {
                //_formThumbnailView = new FormThumbnailView(_formAddBookmark, _applicationSettingsService, _imageCacheService,);
                _formThumbnailView = _scope.Resolve<FormThumbnailView>();
                _formThumbnailView.Show();
                _formThumbnailView.FormClosed += FormThumbnailView_FormClosed;
            }
            else
            {
                _formThumbnailView.Focus();
            }
        }

        private void thumbnailDBSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var thumbnailService = _scope.Resolve<ThumbnailService>();
            var form = new FormThumbnailSettings(thumbnailService);
            form.ShowDialog(this);
        }

        #endregion Main Menu Functions

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (CursorVisible)
            {
                return;
            }


            if (cursorPosition.X != e.X || cursorPosition.Y != e.Y)
            {
                cursorPosition.X = e.X;
                cursorPosition.Y = e.Y;

                pointerBox.X = cursorPosition.X - pointerBox.Width / 2;
                pointerBox.Y = cursorPosition.Y + pointerBox.Height / 2;

                Rectangle intersercionRect = new Rectangle(cursorPosition, new Size(1, 1));
                if (!pointerBox.IntersectsWith(intersercionRect))
                {
                    //Console.WriteLine($"pictureBox1_MouseMoved, locations is: X: {e.X}, Y: {e.Y}");
                    CursorVisible = true;
                    UpdateCursorState();
                }
            }
        }

        private void UpdateCursorState()
        {
            if (CursorVisible)
            {
                Cursor.Show();
            }
            else
            {
                Cursor.Hide();
            }

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            if (!timerCursorVisible.Enabled)
            {
                //timerCursorVisible.Enabled = true;
            }
        }

        private void timerCursorVisible_Tick(object sender, EventArgs e)
        {
            if (!CursorVisible)
            {
                return;
            }

            if (cursorMovedTime.AddMilliseconds(_hideCursorDelay) < DateTime.Now)
            {
                CursorVisible = false;
                UpdateCursorState();
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            timerCursorVisible.Enabled = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            timerCursorVisible.Enabled = false;
        }
    }
}