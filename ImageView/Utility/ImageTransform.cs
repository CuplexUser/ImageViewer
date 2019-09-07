using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Serilog;

namespace ImageViewer.Utility
{
    public static class ImageTransform
    {
        public static Bitmap BitwiseBlend(Bitmap sourceBitmap, Bitmap blendBitmap, BitwiseBlendType blendTypeBlue,
            BitwiseBlendType blendTypeGreen, BitwiseBlendType blendTypeRed)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            var pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);


            BitmapData blendData = blendBitmap.LockBits(new Rectangle(0, 0, blendBitmap.Width, blendBitmap.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            var blendBuffer = new byte[blendData.Stride * blendData.Height];
            Marshal.Copy(blendData.Scan0, blendBuffer, 0, blendBuffer.Length);
            blendBitmap.UnlockBits(blendData);

            int blue = 0, green = 0, red = 0;


            for (int k = 0;
                (k + 4 < pixelBuffer.Length) &&
                (k + 4 < blendBuffer.Length);
                k += 4)
            {
                if (blendTypeBlue == BitwiseBlendType.And)
                    blue = pixelBuffer[k] & blendBuffer[k];
                else if (blendTypeBlue == BitwiseBlendType.Or)
                    blue = pixelBuffer[k] | blendBuffer[k];
                else if (blendTypeBlue == BitwiseBlendType.Xor)
                    blue = pixelBuffer[k] ^ blendBuffer[k];


                if (blendTypeGreen == BitwiseBlendType.And)
                    green = pixelBuffer[k + 1] & blendBuffer[k + 1];
                else if (blendTypeGreen == BitwiseBlendType.Or)
                    green = pixelBuffer[k + 1] | blendBuffer[k + 1];
                else if (blendTypeGreen == BitwiseBlendType.Xor)
                    green = pixelBuffer[k + 1] ^ blendBuffer[k + 1];


                if (blendTypeRed == BitwiseBlendType.And)
                    red = pixelBuffer[k + 2] & blendBuffer[k + 2];
                else if (blendTypeRed == BitwiseBlendType.Or)
                    red = pixelBuffer[k + 2] | blendBuffer[k + 2];
                else if (blendTypeRed == BitwiseBlendType.Xor)
                    red = pixelBuffer[k + 2] ^ blendBuffer[k + 2];


                if (blue < 0)
                    blue = 0;
                else if (blue > 255)
                    blue = 255;


                if (green < 0)
                    green = 0;
                else if (green > 255)
                    green = 255;


                if (red < 0)
                    red = 0;
                else if (red > 255)
                    red = 255;


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }


            var resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);


            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                resultBitmap.Width, resultBitmap.Height),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);


            return resultBitmap;
        }

        public static void MatrixBlend(Bitmap sourceBitmap, Bitmap blendBitmap, byte alpha)
        {
            // for the matrix the range is 0.0 - 1.0
            float alphaNorm = alpha / 255.0F;
            using (Bitmap image1 = sourceBitmap)
            {
                using (Bitmap image2 = blendBitmap)
                {
                    // just change the alpha
                    var matrix = new ColorMatrix(new[]
                    {
                        new[] {1F, 0, 0, 0, 0},
                        new[] {0, 1F, 0, 0, 0},
                        new[] {0, 0, 1F, 0, 0},
                        new[] {0, 0, 0, alphaNorm, 0},
                        new[] {0, 0, 0, 0, 1F}
                    });

                    var imageAttributes = new ImageAttributes();
                    imageAttributes.SetColorMatrix(matrix);

                    using (Graphics g = Graphics.FromImage(image1))
                    {
                        g.CompositingMode = CompositingMode.SourceOver;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(image1,
                            new Rectangle(0, 0, image1.Width, image1.Height),
                            0,
                            0,
                            image2.Width,
                            image2.Height,
                            GraphicsUnit.Pixel,
                            imageAttributes);
                    }
                }
            }
        }

        public static Image OffsetImagesHorizontal(Image currentImage, Image nextImage, Size picBoxSize, float factor,
            bool leftToRight)
        {
            var result = new Bitmap(Math.Max(currentImage.Width, nextImage.Width),
                Math.Max(currentImage.Height, nextImage.Height));
            //var result = new Bitmap(picBoxSize.Width, picBoxSize.Height);
            int x1 = result.Width / 2 - currentImage.Width / 2;
            int x2 = x1 + currentImage.Width;
            int offset = (int)(result.Width * factor);

            if (!leftToRight)
                offset = offset * -1;

            int y1 = 0;
            int y2 = 0;
            int x1Offset = Math.Min(0, x1 + offset);

            float ratio1 = currentImage.Width / (float)currentImage.Height;
            float ratio2 = nextImage.Width / (float)nextImage.Height;

            Graphics gfx = Graphics.FromImage(result);
            gfx.DrawImage(currentImage,
                new Rectangle(Math.Max(0, x1 + offset), y1, currentImage.Width + x1Offset, result.Height));
            gfx.DrawImage(nextImage, new Rectangle(Math.Max(0, x2 + offset), y2, nextImage.Width, result.Height));
            gfx.Dispose();

            return result;
        }

        public static Image OffsetImagesVertical(Image currentImage, Image nextImage, Size imageSize, float factor,
            bool topToBottom)
        {
            var result = new Bitmap(nextImage, imageSize.Width, imageSize.Height);

            int verticalOffset = (int)(imageSize.Height * factor);
            int verticalOffset2 = (int)(nextImage.Height * (1 - factor));

            using (Graphics gfx = Graphics.FromImage(result))
            {
                gfx.DrawImage(currentImage, new Rectangle(0, 0, imageSize.Width, imageSize.Height - verticalOffset));
                gfx.DrawImage(nextImage,
                    new Rectangle(0, verticalOffset2, imageSize.Width, imageSize.Height + verticalOffset2));
            }
            return result;
        }

        public static Image BlendImages(Image image, Image SecondImage, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                var bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    //create a color matrix object  
                    var matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    var attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height,
                        GraphicsUnit.Pixel, attributes);


                    matrix.Matrix33 = 1 - opacity;
                    gfx.DrawImage(SecondImage, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width,
                        image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in ImageTransform.BlendImages() opacity={opacity}", opacity);
                return null;
            }
        }

        public static Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                var bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {
                    //create a color matrix object  
                    var matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    var attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height,
                        GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in ImageTransform.SetImageOpacity() opacity={opacity}", opacity);
                return null;
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }

    public enum BitwiseBlendType
    {
        None,
        Or,
        And,
        Xor
    }
}