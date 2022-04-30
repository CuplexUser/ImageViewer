using JetBrains.Annotations;

namespace ImageViewer.Events
{
    [UsedImplicitly]
    public class TransitionImageUpdateEventArgs : EventArgs
    {
        public Image TransitionImage { get; set; }
    }
}