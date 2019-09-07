using System;
using System.Drawing;

namespace ImageViewer.Models
{
    public class CachedImage
    {
        private byte[] _imageData;

        public string Filename { get; private set; }

        public DateTime AddedToCacheTime { get; private set; }


        public int Size
        {
            get
            {
                if (_imageData != null)
                {
                    return _imageData.Length;
                }
                return 0;
            }
        }

        public byte[] GetImageBytes()
        {
            return _imageData;
        }

        public Image GetImage(Func<byte[], Image> imageConverter)
        {
            return imageConverter.Invoke(_imageData);
        }

        public void SetImage(Func<string, byte[]> imageConverter, string fileName)
        {
            _imageData = imageConverter.Invoke(fileName);
            Filename = fileName;
            AddedToCacheTime = DateTime.Now;
        }
    }
}