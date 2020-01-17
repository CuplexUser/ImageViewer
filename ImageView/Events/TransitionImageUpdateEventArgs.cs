using System;
using System.Drawing;
using JetBrains.Annotations;

namespace ImageViewer.Events
{
    [UsedImplicitly]
    public class TransitionImageUpdateEventArgs : EventArgs
    {
        public Image TransitionImage { get; set; }
    }
}