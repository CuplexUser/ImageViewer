using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AutoMapper;
using GeneralToolkitLib.Converters;
using ImageViewer.Configuration;
using ImageViewer.DataContracts;
using ImageViewer.Library.Extensions;

namespace ImageViewer.Models
{
    public class ApplicationSettingsModel : IEquatable<ApplicationSettingsModel>
    {
        public static ApplicationSettingsModel CreateDefaultSettings()
        {
            var settings = new ApplicationSettingsModel
            {
                AlwaysOntop = false,
                AutoRandomizeCollection = true,
                LastUsedSearchPaths = new List<string>(),
                ShowImageViewFormsInTaskBar = true,
                NextImageAnimation = ApplicationSettingsDataModel.ChangeImageAnimation.None,
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
                ImageCacheSize = 134217728, // 128 Mb,
                ToggleSlideshowWithThirdMouseButton = true,
                AutoHideCursor = true,
                AutoHideCursorDelay = 2000,
                AppSettingsUUID = Guid.NewGuid(),
                BookmarksShowMaximizedImageArea = false,
                BookmarksShowOverlayWindow = false,
            };
            settings.InitFormStateDictionary();
            return settings;
        }
        public void InitFormStateDictionary()
        {
            FormStateDictionary = new Dictionary<string, FormStateModel<Form>>();
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
        public ApplicationSettingsDataModel.ChangeImageAnimation NextImageAnimation { get; set; }

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
        /// Gets or sets a value indicating whether [enable window docking].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable window docking]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableWindowDocking { get; set; }

        /// <summary>
        /// Gets or sets the window docking proximity.
        /// </summary>
        /// <value>
        /// The window docking proximity.
        /// </value>
        public ApplicationSettingsDataModel.WindowDockProximity WindowDockingProximity { get; set; }

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
        public bool PasswordProtectBookmarks { get; protected set; }

        /// <summary>
        /// Gets or sets the password derived string.
        /// </summary>
        /// <value>
        /// The password derived string.
        /// </value>
        public string PasswordDerivedString { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use saved main form position].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use saved main form position]; otherwise, <c>false</c>.
        /// </value>
        public bool UseSavedMainFormPosition { get; protected set; }

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
        public ColorDataModel MainWindowBackgroundColor { get; set; }

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

        [FixedBounds(minValue: GenericConstants.MinCursorDelayValue, maxValue: GenericConstants.MaxCursorDelayValue, GenericConstants.DefaultCursorDelayValue, "Validation failed because value is out of range.")]
        public int AutoHideCursorDelay { get; set; }
        /// <summary>
        /// Gets or sets the application settings UUID.
        /// </summary>
        /// <value>
        /// The application settings UUID.
        /// </value>
        public Guid AppSettingsUUID { get; set; }

        /// <summary>
        /// Gets or sets the form state list.
        /// </summary>
        /// <value>
        /// The form state list.
        /// </value>
        public Dictionary<string, FormStateModel<Form>> FormStateDictionary { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show maximized image area].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show maximized image area]; otherwise, <c>false</c>.
        /// </value>
        public bool BookmarksShowMaximizedImageArea { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [bookmarks show overlay window].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [bookmarks show overlay window]; otherwise, <c>false</c>.
        /// </value>
        public bool BookmarksShowOverlayWindow { get; set; }

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

        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ApplicationSettingsModel, ApplicationSettingsDataModel>()
                      .ForMember(s => s.FormStateHashSet, o => o.MapFrom(d => d.FormStateDictionary.ToHashSet()))
                      .ForMember(s => s.MainWindowBackgroundColor, o => o.MapFrom(d => d.MainWindowBackgroundColor))
                      .ForMember(s => s.NextImageAnimation, o => o.MapFrom(d => d.NextImageAnimation))
                      .ForMember(s => s.WindowDockingProximity, o => o.MapFrom(d => d.WindowDockingProximity))
                      .ReverseMap()
                      .ForMember(s => s.WindowDockingProximity, o => o.MapFrom(d => d.WindowDockingProximity))
                      .ForMember(s => s.FormStateDictionary, o => o.MapFrom(d => d.FormStateHashSet.ToDictionary(model => model.FormTypeFullName)))
                      .ForMember(s => s.MainWindowBackgroundColor, o => o.MapFrom(d => d.MainWindowBackgroundColor))
                      .ForMember(s => s.WindowDockingProximity, o => o.MapFrom(d => d.WindowDockingProximity))
                      .ForMember(s => s.AppSettingsUUID, o => o.MapFrom(d => d.AppSettingsUUID))
                      .ForMember(s => s.AutoRandomizeCollection, o => o.MapFrom(d => d.AutoRandomizeCollection));

        }

        public bool Equals(ApplicationSettingsModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ShowImageViewFormsInTaskBar == other.ShowImageViewFormsInTaskBar && AlwaysOntop == other.AlwaysOntop && AutoRandomizeCollection == other.AutoRandomizeCollection && 
                   Equals(LastUsedSearchPaths, other.LastUsedSearchPaths) && NextImageAnimation == other.NextImageAnimation && SlideshowInterval == other.SlideshowInterval && 
                   ImageTransitionTime == other.ImageTransitionTime && EnableAutoLoadFunctionFromMenu == other.EnableAutoLoadFunctionFromMenu && EnableWindowDocking == other.EnableWindowDocking && 
                   WindowDockingProximity == other.WindowDockingProximity && PrimaryImageSizeMode == other.PrimaryImageSizeMode && ScreenMinXOffset == other.ScreenMinXOffset && 
                   ScreenWidthOffset == other.ScreenWidthOffset && PasswordProtectBookmarks == other.PasswordProtectBookmarks && PasswordDerivedString == other.PasswordDerivedString && 
                   UseSavedMainFormPosition == other.UseSavedMainFormPosition && ShowSwitchImageButtons == other.ShowSwitchImageButtons && LastFolderLocation == other.LastFolderLocation &&
                   ShowNextPrevControlsOnEnterWindow == other.ShowNextPrevControlsOnEnterWindow && ImageCacheSize == other.ImageCacheSize && DefaultKey == other.DefaultKey &&
                   ThumbnailSize == other.ThumbnailSize && MaxThumbnails == other.MaxThumbnails && ConfirmApplicationShutdown == other.ConfirmApplicationShutdown && 
                   Equals(MainWindowBackgroundColor, other.MainWindowBackgroundColor) && AutomaticUpdateCheck == other.AutomaticUpdateCheck && LastUpdateCheck.Equals(other.LastUpdateCheck) 
                && ToggleSlideshowWithThirdMouseButton == other.ToggleSlideshowWithThirdMouseButton && AutoHideCursor == other.AutoHideCursor && AutoHideCursorDelay == other.AutoHideCursorDelay 
                && AppSettingsUUID.Equals(other.AppSettingsUUID) && Equals(FormStateDictionary, other.FormStateDictionary) && BookmarksShowMaximizedImageArea == other.BookmarksShowMaximizedImageArea
                && BookmarksShowOverlayWindow == other.BookmarksShowOverlayWindow;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ApplicationSettingsModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ShowImageViewFormsInTaskBar.GetHashCode();
                hashCode = (hashCode * 397) ^ AlwaysOntop.GetHashCode();
                hashCode = (hashCode * 397) ^ AutoRandomizeCollection.GetHashCode();
                hashCode = (hashCode * 397) ^ (LastUsedSearchPaths != null ? LastUsedSearchPaths.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)NextImageAnimation;
                hashCode = (hashCode * 397) ^ SlideshowInterval;
                hashCode = (hashCode * 397) ^ ImageTransitionTime;
                hashCode = (hashCode * 397) ^ EnableAutoLoadFunctionFromMenu.GetHashCode();
                hashCode = (hashCode * 397) ^ EnableWindowDocking.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)WindowDockingProximity;
                hashCode = (hashCode * 397) ^ PrimaryImageSizeMode;
                hashCode = (hashCode * 397) ^ ScreenMinXOffset;
                hashCode = (hashCode * 397) ^ ScreenWidthOffset;
                hashCode = (hashCode * 397) ^ PasswordProtectBookmarks.GetHashCode();
                hashCode = (hashCode * 397) ^ (PasswordDerivedString != null ? PasswordDerivedString.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ UseSavedMainFormPosition.GetHashCode();
                hashCode = (hashCode * 397) ^ ShowSwitchImageButtons.GetHashCode();
                hashCode = (hashCode * 397) ^ (LastFolderLocation != null ? LastFolderLocation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ShowNextPrevControlsOnEnterWindow.GetHashCode();
                hashCode = (hashCode * 397) ^ ImageCacheSize.GetHashCode();
                hashCode = (hashCode * 397) ^ (DefaultKey != null ? DefaultKey.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ThumbnailSize;
                hashCode = (hashCode * 397) ^ MaxThumbnails;
                hashCode = (hashCode * 397) ^ ConfirmApplicationShutdown.GetHashCode();
                hashCode = (hashCode * 397) ^ (MainWindowBackgroundColor != null ? MainWindowBackgroundColor.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AutomaticUpdateCheck.GetHashCode();
                hashCode = (hashCode * 397) ^ LastUpdateCheck.GetHashCode();
                hashCode = (hashCode * 397) ^ ToggleSlideshowWithThirdMouseButton.GetHashCode();
                hashCode = (hashCode * 397) ^ AutoHideCursor.GetHashCode();
                hashCode = (hashCode * 397) ^ AutoHideCursorDelay;
                hashCode = (hashCode * 397) ^ AppSettingsUUID.GetHashCode();
                hashCode = (hashCode * 397) ^ (FormStateDictionary != null ? FormStateDictionary.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ BookmarksShowMaximizedImageArea.GetHashCode();
                hashCode = (hashCode * 397) ^ BookmarksShowOverlayWindow.GetHashCode();
                return hashCode;
            }
        }
    }
}