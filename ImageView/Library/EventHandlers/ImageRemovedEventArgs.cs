using ImageViewer.Models;

namespace ImageViewer.Library.EventHandlers;

public class ImageRemovedEventArgs : EventArgs
{
    public ImageRemovedEventArgs(int imgRefIndexToRemove)
    {
        ImgRefIndexToRemove = imgRefIndexToRemove;
    }

    public ImageRemovedEventArgs(ImageReference imgRef, int imgRefIndexToRemove)
    {
        ImageReference = imgRef;
        ImgRefIndexToRemove = imgRefIndexToRemove;
    }

    public int ImgRefIndexToRemove { get; }

    public ImageReference ImageReference { get; set; }
}