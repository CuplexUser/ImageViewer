using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;

namespace ImageView.DataModels
{
    [Serializable]
    [DataContract]
    public class ImageViewApplicationSettings
    {
        public enum ChangeImageAnimation
        {
            None = 0,
            SlideLeft = 1,
            SlideRight = 2,
            SlideDown = 3,
            SlideUp = 4,
            FadeIn = 5
        }

        public enum WindowDockProximity
        {
            Near = 1,
            Normal = 2,
            Far = 3
        }


        // Instansiate with default settings
        public ImageViewApplicationSettings()
        {
            AlwaysOntop = true;
            AutoRandomizeCollection = true;
            LastUsedSearchPaths = new List<string>();
            ShowImageViewFormsInTaskBar = true;
            NextImageAnimation = ChangeImageAnimation.None;
            ImageTransitionTime = 1000;
            SlideshowInterval = 5000;
            PrimaryImageSizeMode = (int) PictureBoxSizeMode.Zoom;
            PasswordProtectBookmarks = false;
            PasswordDerivedString = "";
            ShowNextPrevControlsOnEnterWindow = true;
        }

        [DataMember(Name = "ShowImageViewFormsInTaskBar", Order = 1)]
        public bool ShowImageViewFormsInTaskBar { get; set; }

        [DataMember(Name = "AlwaysOntop", Order = 2)]
        public bool AlwaysOntop { get; set; }

        [DataMember(Name = "AutoRandomizeCollection", Order = 3)]
        public bool AutoRandomizeCollection { get; set; }

        [DataMember(Name = "LastUsedSearchPaths", Order = 4)]
        public List<string> LastUsedSearchPaths { get; set; }

        [DataMember(Name = "NextImageAnimation", Order = 5)]
        public ChangeImageAnimation NextImageAnimation { get; set; }

        [DataMember(Name = "SlideshowInterval", Order = 6)]
        public int SlideshowInterval { get; set; }

        [DataMember(Name = "ImageTransitionTime", Order = 7)]
        public int ImageTransitionTime { get; set; }

        [DataMember(Name = "EnableAutoLoadFunctionFromMenu", Order = 8)]
        public bool EnableAutoLoadFunctionFromMenu { get; set; }

        [DataMember(Name = "EnableWindowDocking", Order = 9)]
        public bool EnableWindowDocking { get; set; }

        [DataMember(Name = "WindowDockingProximity", Order = 10)]
        public WindowDockProximity WindowDockingProximity { get; set; }

        [DataMember(Name = "PrimaryImageSizeMode", Order = 11)]
        public int PrimaryImageSizeMode { get; set; }

        [DataMember(Name = "ScreenMinXOffset", Order = 12)]
        public int ScreenMinXOffset { get; set; }

        [DataMember(Name = "ScreenWidthOffset", Order = 13)]
        public int ScreenWidthOffset { get; set; }

        [DataMember(Name = "PasswordProtectBookmarks", Order = 14)]
        public bool PasswordProtectBookmarks { get; protected set; }

        [DataMember(Name = "PasswordDerivedString", Order = 15)]
        public string PasswordDerivedString { get; protected set; }

        [DataMember(Name = "MainFormPosition", Order = 16)]
        public Point MainFormPosition { get; protected set; }

        [DataMember(Name = "MainFormSize", Order = 17)]
        public Size MainFormSize { get; protected set; }

        [DataMember(Name = "UseSavedMainFormPosition", Order = 18)]
        public bool UseSavedMainFormPosition { get; protected set; }

        [DataMember(Name = "ShowSwitchImageButtons", Order = 19)]
        public bool ShowSwitchImageButtons { get;  set; }

        [DataMember(Name = "LastFolderLocation", Order = 20)]
        public string LastFolderLocation { get; set; }

        [DataMember(Name = "ShowNextPrevControlsOnEnterWindow", Order = 21)]
        public bool ShowNextPrevControlsOnEnterWindow { get; set; }

        [DataMember(Name = "ImageCacheSize", Order = 22)]
        public int ImageCacheSize { get; set; }

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

        public void DisablePasswordProtectBookmarks()
        {
            PasswordProtectBookmarks = false;
            PasswordDerivedString = null;
        }

        public void EnablePasswordProtectBookmarks(string verifiedPassword)
        {
            if (verifiedPassword != null)
            {
                PasswordProtectBookmarks = true;
                PasswordDerivedString = GeneralConverters.GeneratePasswordDerivedString(verifiedPassword);
            }
        }

        public void SetMainFormPosition(Rectangle mainFormBounds)
        {
            Rectangle mainScreenBounds = Screen.PrimaryScreen.Bounds;
            Rectangle intersection = Rectangle.Intersect(mainScreenBounds, mainFormBounds);

            if (intersection != Rectangle.Empty)
            {
                MainFormPosition = new Point(mainFormBounds.X, mainFormBounds.Y);
                MainFormSize = new Size(mainFormBounds.Width, mainFormBounds.Height);
                UseSavedMainFormPosition = true;
            }
        }
    }
}