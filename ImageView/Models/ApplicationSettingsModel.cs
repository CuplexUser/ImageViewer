using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using ImageViewer.Configuration;
using ImageViewer.Library.Extensions;

namespace ImageViewer.Models
{
    /// <summary>
    /// ApplicationSettingsModel
    /// </summary>
    public class ApplicationSettingsModel
    {
        /// <summary>
        /// ChangeImageAnimation
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
        /// WindowDockProximity
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
        /// Gets or sets a value indicating whether [show image view forms in task bar].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show image view forms in task bar]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowImageViewFormsInTaskBar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [always ontop].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [always ontop]; otherwise, <c>false</c>.
        /// </value>
        public bool AlwaysOntop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic randomize collection].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic randomize collection]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoRandomizeCollection { get; set; }

        /// <summary>
        /// Gets or sets the last used search paths.
        /// </summary>
        /// <value>
        /// The last used search paths.
        /// </value>
        public List<string> LastUsedSearchPaths { get; set; }

        /// <summary>
        /// Gets or sets the next image animation.
        /// </summary>
        /// <value>
        /// The next image animation.
        /// </value>
        public ChangeImageAnimation NextImageAnimation { get; set; }

        /// <summary>
        /// Gets or sets the slideshow interval.
        /// </summary>
        /// <value>
        /// The slideshow interval.
        /// </value>
        public int SlideshowInterval { get; set; }

        /// <summary>
        /// Gets or sets the image transition time.
        /// </summary>
        /// <value>
        /// The image transition time.
        /// </value>
        public int ImageTransitionTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [enable automatic load function from menu].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable automatic load function from menu]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableAutoLoadFunctionFromMenu { get; set; }

        /// <summary>
        /// Gets or sets the primary image size mode.
        /// </summary>
        /// <value>
        /// The primary image size mode.
        /// </value>
        public int PrimaryImageSizeMode { get; set; }

        /// <summary>
        /// Gets or sets the screen minimum x offset.
        /// </summary>
        /// <value>
        /// The screen minimum x offset.
        /// </value>
        public int ScreenMinXOffset { get; set; }

        /// <summary>
        /// Gets or sets the screen width offset.
        /// </summary>
        /// <value>
        /// The screen width offset.
        /// </value>
        public int ScreenWidthOffset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [password protect bookmarks].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [password protect bookmarks]; otherwise, <c>false</c>.
        /// </value>
        public bool PasswordProtectBookmarks { get; set; }

        /// <summary>
        /// Gets or sets the password derived string.
        /// </summary>
        /// <value>
        /// The password derived string.
        /// </value>
        public string PasswordDerivedString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show switch image buttons].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show switch image buttons]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSwitchImageButtons { get; set; }

        /// <summary>
        /// Gets or sets the last folder location.
        /// </summary>
        /// <value>
        /// The last folder location.
        /// </value>
        public string LastFolderLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show next previous controls on enter window].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show next previous controls on enter window]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowNextPrevControlsOnEnterWindow { get; set; }

        /// <summary>
        /// Gets or sets the size of the image cache.
        /// </summary>
        /// <value>
        /// The size of the image cache.
        /// </value>
        [Range(typeof(long), "33554432", "1073741824", ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public long ImageCacheSize { get; set; }

        /// <summary>
        /// Gets or sets the default key.
        /// </summary>
        /// <value>
        /// The default key.
        /// </value>
        public string DefaultKey { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>
        /// The size of the thumbnail.
        /// </value>
        public int ThumbnailSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum thumbnails.
        /// </summary>
        /// <value>
        /// The maximum thumbnails.
        /// </value>
        public int MaxThumbnails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [confirm application shutdown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [confirm application shutdown]; otherwise, <c>false</c>.
        /// </value>
        public bool ConfirmApplicationShutdown { get; set; }

        /// <summary>
        /// Gets or sets the color of the main window background.
        /// </summary>
        /// <value>
        /// The color of the main window background.
        /// </value>
        public Color MainWindowBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic update check].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic update check]; otherwise, <c>false</c>.
        /// </value>
        public bool AutomaticUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets the last update check.
        /// </summary>
        /// <value>
        /// The last update check.
        /// </value>
        public DateTime LastUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [toggle slideshow with third mouse button].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [toggle slideshow with third mouse button]; otherwise, <c>false</c>.
        /// </value>
        public bool ToggleSlideshowWithThirdMouseButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic hide cursor].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic hide cursor]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoHideCursor { get; set; }


        /// <summary>
        /// Gets or sets the automatic hide cursor delay.
        /// </summary>
        /// <value>
        /// The automatic hide cursor delay.
        /// </value>
        [FixedBounds(minValue: GenericConstants.MinCursorDelayValue, maxValue: GenericConstants.MaxCursorDelayValue, GenericConstants.DefaultCursorDelayValue, "Validation failed because value is out of range.")]
        public int AutoHideCursorDelay { get; set; }

        /// <summary>
        /// Gets or sets the form state models.
        /// </summary>
        /// <value>
        /// The form state models.
        /// </value>
        public IDictionary<string, FormStateModel> FormStateModels { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show overlay window].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show overlay window]; otherwise, <c>false</c>.
        /// </value>
        public bool BookmarksShowOverlayWindow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show maximized image area].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show maximized image area]; otherwise, <c>false</c>.
        /// </value>
        public bool BookmarksShowMaximizedImageArea { get; set; }

        /// <summary>
        /// Gets or sets the application settings UUID.
        /// </summary>
        /// <value>
        /// The application settings UUID.
        /// </value>
        public Guid AppSettingsGuid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loaded from disk.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is loaded from disk; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoadedFromDisk { get; set; }
    }
}