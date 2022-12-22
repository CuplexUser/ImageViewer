﻿using System.Drawing.Imaging;
using ImageMagick;

namespace ImageViewer.Providers;

public class ImageTypeConverter : ProviderBase
{
    public Image ConvertToSystemImage(MagickImage source)
    {
        using (var ms = new MemoryStream())
        {
            source.Write(ms, MagickFormat.Jpeg);
            ms.Position = 0;
            Image img = Image.FromStream(ms, true, true);

            return img;
        }
    }

    public MagickImage ConvertFromSystemImage(Image source)
    {
        using (var ms = new MemoryStream())
        {
            ms.Position = 0;
            source.Save(ms, ImageFormat.Jpeg);

            ms.Position = 0;
            MagickImage img = new MagickImage(ms, MagickFormat.Jpeg);
            ms.Flush();

            return img;
        }
    }
}