using System;
using ImageViewer.DataContracts;
using ImageViewer.Models;

namespace ImageViewer.Library.EventHandlers
{
    public delegate void ImageRemovedEventHandler(object sender, ImageRemovedEventArgs e);

    public class ImageRemovedEventArgs : EventArgs
    {
        public int ImgRefIndexToRemove { get; }

        public ImageReference ImageReference { get; set; }

        public ImageRemovedEventArgs(int imgRefIndexToRemove)
        {
            ImgRefIndexToRemove = imgRefIndexToRemove;
        }

        public ImageRemovedEventArgs(ImageReference imgRef, int imgRefIndexToRemove)
        {
            ImageReference = imgRef;
            ImgRefIndexToRemove = imgRefIndexToRemove;
        }
    }
}