using ImageMagick;
using System.Drawing.Imaging;

namespace ImageViewer.Providers;

public class ImageTypeConverter : ProviderBase
{
    public Image ConvertToSystemImage(MagickImage source)
    {
        using (var ms = new MemoryStream())
        {
            source.Write(ms, MagickFormat.Jpeg);
            ms.Position = 0;
            var img = Image.FromStream(ms, true, true).Clone() as Image;

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
            var img = new MagickImage(ms, MagickFormat.Jpeg);
            ms.Flush();

            return img;
        }
    }
}