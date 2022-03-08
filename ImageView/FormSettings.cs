using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using ImageViewer.InputForms;
using ImageViewer.Models;
using ImageViewer.Services;
using ImageViewer.Utility;
using Serilog;

namespace ImageViewer
{
    public partial class FormSettings : Form
    {
        private const int TrackbarDivider = 1048576;
        private readonly ApplicationSettingsService _applicationSettingsService;
        private readonly ImageCacheService _imageCacheService;
        private long _selectedCacheSize;
        private readonly ApplicationSettingsModel _originalSettings;

        public FormSettings(ApplicationSettingsService applicationSettingsService, ImageCacheService imageCacheService)
        {
            _applicationSettingsService = applicationSettingsService;
            bool settingsLoaded = _applicationSettingsService.LoadSettings();
            if (!settingsLoaded)
            {
                Log.Warning("FormSettings constructor failed to load settings");
            }

            _originalSettings = applicationSettingsService.Settings;
            _imageCacheService = imageCacheService;
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            ApplicationSettingsModel settings = _applicationSettingsService.Settings;
            chkAutoRandomize.Checked = settings.AutoRandomizeCollection;
            chkPasswordProtectBookmarks.Checked = settings.PasswordProtectBookmarks;
            chkShowSwitchImgButtons.Checked = settings.ShowSwitchImageButtons;
            chkEnableAutoload.Checked = settings.EnableAutoLoadFunctionFromMenu;
            chkConfirmExit.Checked = settings.ConfirmApplicationShutdown;
            ChkAutomaticallyCheckForUpdates.Checked = settings.AutomaticUpdateCheck;
            chkToggleSlidshowWithThirdMouseButton.Checked = settings.ToggleSlideshowWithThirdMouseButton;
            chkAutohideCursor.Checked = settings.AutoHideCursor;

            _selectedCacheSize = _imageCacheService.CacheSize;

            if (settings.ShowNextPrevControlsOnEnterWindow)
            {
                rbOverWindow.Checked = true;
                rbOverControlArea.Checked = false;
            }
            else
            {
                rbOverWindow.Checked = false;
                rbOverControlArea.Checked = true;
            }

            rbImgTransformNone.Checked = false;
            switch (settings.NextImageAnimation)
            {
                case ApplicationSettingsModel.ChangeImageAnimation.None:
                    rbImgTransformNone.Checked = true;
                    break;
                case ApplicationSettingsModel.ChangeImageAnimation.FadeIn:
                    rbImgTransformFadeIn.Checked = true;
                    break;
                default:
                    rbImgTransformNone.Checked = true;
                    break;
            }

            if (settings.ImageTransitionTime < 10 || settings.ImageTransitionTime > 5000)
            {
                settings.ImageTransitionTime = 1000;
            }

            numericScreenMinOffset.Value = settings.ScreenMinXOffset;
            numericScreenWidthOffset.Value = settings.ScreenWidthOffset;

            if (settings.AutoHideCursorDelay < numericAutohideCursorDelay.Minimum)
            {
                settings.AutoHideCursorDelay = 2000;
            }

            numericAutohideCursorDelay.Value = settings.AutoHideCursorDelay;

            trackBarFadeTime.Value = settings.ImageTransitionTime;
            lblFadeTime.Text = trackBarFadeTime.Value + " ms";

            // Colors
            var colorList = UIHelper.GetSelectableBackgroundColors();
            colorList.AddRange(UIHelper.GetSelectableSystemBackgroundColors());
            backgroundColorDropdownList.DataSource = colorList;
            BackgroundImageDropdown.DataSource = UIHelper.GetSelectableBackgroundColors();

            if (backgroundColorDropdownList.Items.Count > 0)
            {
                Color savedColor = settings.MainWindowBackgroundColor;
                var item = colorList.FirstOrDefault(x => x.ToArgb() == savedColor.ToArgb());
                backgroundColorDropdownList.SelectedItem = item;
            }

            // Log.Information("Load Settings Cache size: {_selectedCacheSize}", _selectedCacheSize);

            UpdateCacheStats();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var settings = _applicationSettingsService.Settings;
            if (_originalSettings != settings)
            {
                _applicationSettingsService.SetSettingsStateModified();
            }

            if (rbImgTransformNone.Checked)
                settings.NextImageAnimation = ApplicationSettingsModel.ChangeImageAnimation.None;

            settings.AutoRandomizeCollection = chkAutoRandomize.Checked;
            settings.ShowSwitchImageButtons = chkShowSwitchImgButtons.Checked;
            settings.EnableAutoLoadFunctionFromMenu = chkEnableAutoload.Checked;
            settings.ShowNextPrevControlsOnEnterWindow = rbOverWindow.Checked;
            settings.ConfirmApplicationShutdown = chkConfirmExit.Checked;
            settings.AutoHideCursor = chkAutohideCursor.Checked;

            if (rbImgTransformFadeIn.Checked)
            {
                settings.NextImageAnimation = ApplicationSettingsModel.ChangeImageAnimation.FadeIn;
            }

            settings.ToggleSlideshowWithThirdMouseButton = chkToggleSlidshowWithThirdMouseButton.Checked;
            settings.ImageTransitionTime = trackBarFadeTime.Value;
            settings.ScreenMinXOffset = Convert.ToInt32(numericScreenMinOffset.Value);
            settings.ScreenWidthOffset = Convert.ToInt32(numericScreenWidthOffset.Value);

            Color selectedColor = (Color) backgroundColorDropdownList.SelectedItem;
            settings.MainWindowBackgroundColor = selectedColor;
            settings.AutoHideCursorDelay = Convert.ToInt32(numericAutohideCursorDelay.Value);

            _applicationSettingsService.Settings.ImageCacheSize = _selectedCacheSize;
            _applicationSettingsService.Settings.AutomaticUpdateCheck = ChkAutomaticallyCheckForUpdates.Checked;
            _imageCacheService.CacheSize = _selectedCacheSize;

            _applicationSettingsService.SaveSettings();
            _applicationSettingsService.SetSettingsStateModified();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkPasswordProtectBookmarks_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void chkPasswordProtectBookmarks_MouseUp(object sender, MouseEventArgs e)
        {
            if (chkPasswordProtectBookmarks.Checked)
                using (var formSetPassword = new FormSetPassword())
                {
                    if (formSetPassword.ShowDialog(this) == DialogResult.OK)
                    {
                        if (formSetPassword.VerifiedPassword == null)
                        {
                            chkPasswordProtectBookmarks.Checked = false;
                            return;
                        }
                    }
                    else
                        chkPasswordProtectBookmarks.Checked = false;
                }
            else
            {
                using (
                    var formGetPassword = new FormGetPassword
                    {
                        PasswordDerivedString = _applicationSettingsService.Settings.PasswordDerivedString
                    })
                {
                    if (formGetPassword.ShowDialog() == DialogResult.OK && formGetPassword.PasswordVerified)
                    {
                    }
                    else
                        chkPasswordProtectBookmarks.Checked = true;
                }
            }
        }

        private void UpdateCacheStats()
        {
            long cacheSize = _selectedCacheSize;
            long cacheUsage = _imageCacheService.CacheSpaceAllocated;
            const long maxSize = ImageCacheService.MaxCacheSize;
            const long minSize = ImageCacheService.MinCacheSize;
            pbarPercentUsed.Maximum = 100;
            pbarPercentUsed.Minimum = 0;

            lblCacheItems.Text = _imageCacheService.CachedImages.ToString();
            lblUsedSpace.Text = GeneralConverters.FormatFileSizeToString(cacheUsage, 2);
            lblFreeSpace.Text = GeneralConverters.FormatFileSizeToString(cacheSize - cacheUsage);
            pbarPercentUsed.Value = Convert.ToInt32((double) cacheUsage / cacheSize * 100d);

            trackBarCacheSize.Minimum = Convert.ToInt32(minSize / TrackbarDivider);
            trackBarCacheSize.Maximum = Convert.ToInt32(maxSize / TrackbarDivider);
            trackBarCacheSize.Value = Convert.ToInt32(cacheSize / TrackbarDivider);
            UpdateCacheSizeLabel();
        }

        private void UpdateCacheSizeLabel()
        {
            lblCacheSize.Text = GeneralConverters.FormatFileSizeToString(trackBarCacheSize.Value * TrackbarDivider, 0);
        }

        private void trackBarFadeTime_Scroll(object sender, EventArgs e)
        {
            lblFadeTime.Text = trackBarFadeTime.Value + " ms";
        }

        private void trackBarCacheSize_Scroll(object sender, EventArgs e)
        {
            UpdateCacheSizeLabel();
            _selectedCacheSize = trackBarCacheSize.Value * TrackbarDivider;
        }

        private void toolTipUpdateDescription_Popup(object sender, PopupEventArgs e)
        {
        }
    }
}