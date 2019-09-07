using System;
using System.Drawing;

namespace ImageViewer.Models
{
    public class RawImage
    {
        private readonly byte[] _imageBytes;

        public RawImage(byte[] imageBytes)
        {
            _imageBytes = imageBytes;
        }

        public byte[] ImageData
        {
            get { return _imageBytes; }
        }

        public Image LoadImage(Func< byte[],Image> loadImageToByteArrFunc)
        {
            return loadImageToByteArrFunc.Invoke(ImageData);
        }
    }
}
