using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using JetBrains.Annotations;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using ImageViewer.Models;
using Serilog;

namespace ImageViewer.Managers
{
    [UsedImplicitly]
    public class ImageManager : ManagerBase, IDisposable
    {
        private readonly ImageFactory _imageFactory;

        public ImageManager()
        {
            _imageFactory = new ImageFactory();
        }

        public Image CreateThumbnail(string fullPath, Size size)
        {
            FileStream fs = null;

            try
            {
                fs = File.OpenRead(fullPath);

                _imageFactory.Load(fs);
                _imageFactory.Resize(size);
                return _imageFactory.Image;
            }
            catch (Exception ex)
            {
                Log.Error(ex,$"Create Thumbnail exception for file {fullPath}");
            }
            finally
            {
                fs?.Close();
            }

            return null;
        }

        public Image LoadFromByteArray(byte[] readBytes)
        {
            _imageFactory.Load(readBytes);
            return _imageFactory.Image;
        }

        public void Dispose()
        {
            _imageFactory?.Dispose();
        }

        public RawImage CreateRawImageFromImage(Image image)
        {
            if (image == null)
                return null;

            RawImage rawImage;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                ms.Flush();
                rawImage = new RawImage(ms.ToArray());
            }

            return rawImage;
        }

        public byte[] GetImageByteArray(string fileName, ISupportedImageFormat imageFormat)
        {
            try
            {
                byte[] imgBytes;
                _imageFactory.Load(fileName);
                if (!Equals(_imageFactory.CurrentImageFormat, imageFormat))
                {
                    _imageFactory.Format(imageFormat);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    _imageFactory.Save(ms);
                    ms.Flush();
                    imgBytes = ms.ToArray();
                }

                return imgBytes;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"GetImageByteArray Failed using filename: {fileName} and image format: {imageFormat}");
                return null;
            }
        }

        public static Image GetImageFromByteArray(byte[] imgBytes)
        {
            try
            {
                ImageFactory imgImageFactory = new ImageFactory();
                imgImageFactory.Load(imgBytes);
                return imgImageFactory.Image;
            }
            catch (Exception e)
            {
                Log.Error(e, "GetImageFromByteArray failed");
                return null;
            }
        }
    }
}