using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using ImageViewer.Utility;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// All Application settings
    /// </summary>
    [Serializable]
    [DataContract(Name = "ImageViewApplicationSettings")]
    public class ImageViewApplicationSettings
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
        /// Initializes a new instance of the <see cref="ImageViewApplicationSettings"/> class.
        /// </summary>
        protected ImageViewApplicationSettings()
        {

        }

        /// <summary>
        /// Creates the default settings.
        /// </summary>
        /// <returns></returns>
        public static ImageViewApplicationSettings CreateDefaultSettings()
        {
            var settings = new ImageViewApplicationSettings
            {
                AlwaysOntop = false,
                AutoRandomizeCollection = true,
                LastUsedSearchPaths = new List<string>(),
                ShowImageViewFormsInTaskBar = true,
                NextImageAnimation = ChangeImageAnimation.None,
                ImageTransitionTime = 1000,
                SlideshowInterval = 5000,
                PrimaryImageSizeMode = (int)PictureBoxSizeMode.Zoom,
                PasswordProtectBookmarks = false,
                PasswordDerivedString = "",
                ShowNextPrevControlsOnEnterWindow = false,
                ThumbnailSize = 256,
                MaxThumbnails = 256,
                ConfirmApplicationShutdown = true,
                AutomaticUpdateCheck = true,
                LastUpdateCheck = new DateTime(2010, 1, 1),
                ExtendedAppSettings = ApplicationSettingsHelper.Create(),
                ImageCacheSize = 134217728, // 128 Mb,
                ToggleSlideshowWithThirdMouseButton = true,
            };

            return settings;
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
        /// Gets or sets the main form position.
        /// </summary>
        /// <value>
        /// The main form position.
        /// </value>
        [DataMember(Name = "MainFormPosition", Order = 16)]
        public PointDataModel MainFormPosition { get; protected set; }

        /// <summary>
        /// Gets or sets the size of the main form.
        /// </summary>
        /// <value>
        /// The size of the main form.
        /// </value>
        [DataMember(Name = "MainFormSize", Order = 17)]
        public SizeDataModel MainFormSize { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use saved main form position].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use saved main form position]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "UseSavedMainFormPosition", Order = 18)]
        public bool UseSavedMainFormPosition { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show switch image buttons].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show switch image buttons]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ShowSwitchImageButtons", Order = 19)]
        public bool ShowSwitchImageButtons { get; set; }

        /// <summary>
        /// Gets or sets the last folder location.
        /// </summary>
        /// <value>
        /// The last folder location.
        /// </value>
        [DataMember(Name = "LastFolderLocation", Order = 20)]
        public string LastFolderLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show next previous controls on enter window].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show next previous controls on enter window]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ShowNextPrevControlsOnEnterWindow", Order = 21)]
        public bool ShowNextPrevControlsOnEnterWindow { get; set; }

        /// <summary>
        /// Gets or sets the size of the image cache.
        /// </summary>
        /// <value>
        /// The size of the image cache.
        /// </value>
        [DataMember(Name = "ImageCacheSize", Order = 22)]
        public long ImageCacheSize { get; set; }

        /// <summary>
        /// Gets or sets the default key.
        /// </summary>
        /// <value>
        /// The default key.
        /// </value>
        [DataMember(Name = "DefaultKey", Order = 23)]
        public string DefaultKey { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail.
        /// </summary>
        /// <value>
        /// The size of the thumbnail.
        /// </value>
        [DataMember(Name = "ThumbnailSize", Order = 24)]
        public int ThumbnailSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum thumbnails.
        /// </summary>
        /// <value>
        /// The maximum thumbnails.
        /// </value>
        [DataMember(Name = "MaxThumbnails", Order = 25)]
        public int MaxThumbnails { get; set; }

        /// <summary>
        /// Gets or sets the size of the thumbnail form.
        /// </summary>
        /// <value>
        /// The size of the thumbnail form.
        /// </value>
        [DataMember(Name = "ThumbnailFormSize", Order = 26)]
        public SizeDataModel ThumbnailFormSize { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail form location.
        /// </summary>
        /// <value>
        /// The thumbnail form location.
        /// </value>
        [DataMember(Name = "ThumbnailFormLocation", Order = 27)]
        public PointDataModel ThumbnailFormLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [confirm application shutdown].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [confirm application shutdown]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ConfirmApplicationShutdown", Order = 28)]
        public bool ConfirmApplicationShutdown { get; set; }

        /// <summary>
        /// Gets or sets the color of the main window background.
        /// </summary>
        /// <value>
        /// The color of the main window background.
        /// </value>
        [DataMember(Name = "MainWindowBackgroundColor", Order = 29)]
        public ColorDataModel MainWindowBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [automatic update check].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic update check]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "AutomaticUpdateCheck", Order = 30)]
        public bool AutomaticUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets the last update check.
        /// </summary>
        /// <value>
        /// The last update check.
        /// </value>
        [DataMember(Name = "LastUpdateCheck", Order = 31)]
        public DateTime LastUpdateCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [toggle slideshow with third mouse button].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [toggle slideshow with third mouse button]; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Name = "ToggleSlideshowWithThirdMouseButton", Order = 32)]
        public bool ToggleSlideshowWithThirdMouseButton { get; set; }

        [DataMember(Name = "ExtendedAppSettings", Order = 33)]
        public AppSettingsExtendedDataModel ExtendedAppSettings { get; set; }

        /// <summary>
        /// Removes the duplicate entries with ignore case.
        /// </summary>
        public void RemoveDuplicateEntriesWithIgnoreCase()
        {
            var deleteStack = new Stack<string>();
            foreach (string searchPath in LastUsedSearchPaths)
            {
                if (LastUsedSearchPaths.Any(s => s.ToLower() == searchPath))
                    deleteStack.Push(searchPath);
            }

            while (deleteStack.Count > 0)
                LastUsedSearchPaths.Remove(deleteStack.Pop());
        }

        /// <summary>
        /// Disables the password protect bookmarks.
        /// </summary>
        public void DisablePasswordProtectBookmarks()
        {
            PasswordProtectBookmarks = false;
            PasswordDerivedString = null;
        }

        /// <summary>
        /// Enables the password protect bookmarks.
        /// </summary>
        /// <param name="verifiedPassword">The verified password.</param>
        public void EnablePasswordProtectBookmarks(string verifiedPassword)
        {
            if (verifiedPassword != null)
            {
                PasswordProtectBookmarks = true;
                PasswordDerivedString = GeneralConverters.GeneratePasswordDerivedString(verifiedPassword);
                DefaultKey = null;
            }
        }
    }
}