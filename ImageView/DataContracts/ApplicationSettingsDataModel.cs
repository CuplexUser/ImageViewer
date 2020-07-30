using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using ImageViewer.Configuration;
using ImageViewer.Library.Extensions;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// All Application settings
    /// </summary>
    [Serializable]
    [DataContract(Name = "ApplicationSettingsDataModel")]
    public class ApplicationSettingsDataModel
    {
        /// <summary>
        /// 
        /// </summary>
        public enum ChangeImageAnimation
        {
            /// <summary>
            /// The none
            /// </summary>
            None = 0,
            /// <summary>
            /// The slide left
            /// </summary>
            SlideLeft = 1,
            /// <summary>
            /// The slide right
            /// </summary>
            SlideRight = 2,
            /// <summary>
            /// The slide down
            /// </summary>
            SlideDown = 3,
            /// <summary>
            /// The slide up
            /// </summary>
            SlideUp = 4,
            /// <summary>
            /// The fade in
            /// </summary>
            FadeIn = 5
        }

        /// <summary>
        /// 
        /// </summary>
        public enum WindowDockProximity
        {
            /// <summary>
            /// The near
            /// </summary>
            Near = 1,
            /// <summary>
            /// The normal
            /// </summary>
            Normal = 2,
            /// <summary>
            /// The far
            /// </summary>
            Far = 3
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsDataModel"/> class.
        /// </summary>
        protected ApplicationSettingsDataModel()
        {

        }

        /// <summary>
        /// Gets or sets a value indicating whether [show image view forms in task bar].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show image view forms in task bar]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ShowImageViewFormsInTaskBar", Order = 1)]
        public bool ShowImageViewFormsInTaskBar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [always ontop].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [always ontop]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "AlwaysOntop", Order = 2)]
        public bool AlwaysOntop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic randomize collection].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic randomize collection]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "AutoRandomizeCollection", Order = 3)]
        public bool AutoRandomizeCollection { get; set; }

        /// <summary>
        /// Gets or sets the last used search paths.
        /// </summary>
        /// <value>
        /// The last used search paths.
        /// </value>
        [DataMember(Name = "LastUsedSearchPaths", Order = 4)]
        public List<string> LastUsedSearchPaths { get; set; }

        /// <summary>
        /// Gets or sets the next image animation.
        /// </summary>
        /// <value>
        /// The next image animation.
        /// </value>
        [DataMember(Name = "NextImageAnimation", Order = 5)]
        public ChangeImageAnimation NextImageAnimation { get; set; }

        /// <summary>
        /// Gets or sets the slideshow interval.
        /// </summary>
        /// <value>
        /// The slideshow interval.
        /// </value>
        [DataMember(Name = "SlideshowInterval", Order = 6)]
        public int SlideshowInterval { get; set; }

        /// <summary>
        /// Gets or sets the image transition time.
        /// </summary>
        /// <value>
        /// The image transition time.
        /// </value>
        [DataMember(Name = "ImageTransitionTime", Order = 7)]
        public int ImageTransitionTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable automatic load function from menu].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable automatic load function from menu]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "EnableAutoLoadFunctionFromMenu", Order = 8)]
        public bool EnableAutoLoadFunctionFromMenu { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable window docking].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable window docking]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "EnableWindowDocking", Order = 9)]
        public bool EnableWindowDocking { get; set; }

        /// <summary>
        /// Gets or sets the window docking proximity.
        /// </summary>
        /// <value>
        /// The window docking proximity.
        /// </value>
        [DataMember(Name = "WindowDockingProximity", Order = 10)]
        public WindowDockProximity WindowDockingProximity { get; set; }

        /// <summary>
        /// Gets or sets the primary image size mode.
        /// </summary>
        /// <value>
        /// The primary image size mode.
        /// </value>
        [DataMember(Name = "PrimaryImageSizeMode", Order = 11)]
        public int PrimaryImageSizeMode { get; set; }

        /// <summary>
        /// Gets or sets the screen minimum x offset.
        /// </summary>
        /// <value>
        /// The screen minimum x offset.
        /// </value>
        [DataMember(Name = "ScreenMinXOffset", Order = 12)]
        public int ScreenMinXOffset { get; set; }

        /// <summary>
        /// Gets or sets the screen width offset.
        /// </summary>
        /// <value>
        /// The screen width offset.
        /// </value>
        [DataMember(Name = "ScreenWidthOffset", Order = 13)]
        public int ScreenWidthOffset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [password protect bookmarks].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [password protect bookmarks]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "PasswordProtectBookmarks", Order = 14)]
        public bool PasswordProtectBookmarks { get; protected set; }

        /// <summary>
        /// Gets or sets the password derived string.
        /// </summary>
        /// <value>
        /// The password derived string.
        /// </value>
        [DataMember(Name = "PasswordDerivedString", Order = 15)]
        public string PasswordDerivedString { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use saved main form position].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use saved main form position]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "UseSavedMainFormPosition", Order = 16)]
        public bool UseSavedMainFormPosition { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show switch image buttons].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show switch image buttons]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ShowSwitchImageButtons", Order = 17)]
        public bool ShowSwitchImageButtons { get; set; }

        /// <summary>
        /// Gets or sets the last folder location.
        /// </summary>
        /// <value>
        /// The last folder location.
        /// </value>
        [DataMember(Name = "LastFolderLocation", Order = 18)]
        public string LastFolderLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show next previous controls on enter window].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show next previous controls on enter window]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ShowNextPrevControlsOnEnterWindow", Order = 19)]
        public bool ShowNextPrevControlsOnEnterWindow { get; set; }

        /// <summary>
        /// Gets or sets the size of the image cache.
        /// </summary>
        /// <value>
        /// The size of the image cache.
        /// </value>
        [DataMember(Name = "ImageCacheSize", Order = 20)]
        public long ImageCacheSize { get; set; }

        /// <summary>
        /// Gets or sets the default key.
        /// </summary>
        /// <value>
        /// The default key.
        /// </value>
        [DataMember(Name = "DefaultKey", Order = 21)]
        public string DefaultKey { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>
        /// The size of the thumbnail.
        /// </value>
        [DataMember(Name = "ThumbnailSize", Order = 22)]
        public int ThumbnailSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum thumbnails.
        /// </summary>
        /// <value>
        /// The maximum thumbnails.
        /// </value>
        [DataMember(Name = "MaxThumbnails", Order = 23)]
        public int MaxThumbnails { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [confirm application shutdown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [confirm application shutdown]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ConfirmApplicationShutdown", Order = 24)]
        public bool ConfirmApplicationShutdown { get; set; }

        /// <summary>
        /// Gets or sets the color of the main window background.
        /// </summary>
        /// <value>
        /// The color of the main window background.
        /// </value>
        [DataMember(Name = "MainWindowBackgroundColor", Order = 25)]
        public ColorDataModel MainWindowBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic update check].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic update check]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "AutomaticUpdateCheck", Order = 26)]
        public bool AutomaticUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets the last update check.
        /// </summary>
        /// <value>
        /// The last update check.
        /// </value>
        [DataMember(Name = "LastUpdateCheck", Order = 27)]
        public DateTime LastUpdateCheck { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [toggle slideshow with third mouse button].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [toggle slideshow with third mouse button]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ToggleSlideshowWithThirdMouseButton", Order = 28)]
        public bool ToggleSlideshowWithThirdMouseButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic hide cursor].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic hide cursor]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "AutoHideCursor", Order =29)]
        public bool AutoHideCursor { get; set; }


        [DataMember(Name = "AutoHideCursorDelay", Order = 30)]
        [FixedBounds(minValue: GenericConstants.MinCursorDelayValue, maxValue: GenericConstants.MaxCursorDelayValue, GenericConstants.DefaultCursorDelayValue, "Validation failed because value is out of range.")]
        public int AutoHideCursorDelay { get; set; }

        /// <summary>
        /// Gets or sets the application settings UUID.
        /// </summary>
        /// <value>
        /// The application settings UUID.
        /// </value>
        [DataMember(Name = "AppSettingsUUID", Order = 31)]
        public Guid AppSettingsUUID { get; set; }

        /// <summary>
        /// Gets or sets the form state list.
        /// </summary>
        /// <value>
        /// The form state list.
        /// </value>
        [DataMember(Name = "FormStateDictionary", Order = 32, IsRequired = true, EmitDefaultValue = true)]
        public Dictionary<string, FormStateDataModel> FormStateDictionary { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show maximized image area].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show maximized image area]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "BookmarksShowMaximizedImageArea", Order = 33)]
        public bool BookmarksShowMaximizedImageArea { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show overlay window].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show overlay window]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "BookmarksShowOverlayWindow", Order = 34)]
        public bool BookmarksShowOverlayWindow { get; set; }

        public void InitFormDictionary()
        {
            if (FormStateDictionary == null)
            {
                FormStateDictionary = new Dictionary<string, FormStateDataModel>();
            }
        }
    }
}