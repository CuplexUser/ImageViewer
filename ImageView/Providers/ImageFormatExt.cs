using ImageViewer.Providers;

public class ImageFormatExt
{
    private static ImageFormatExt _instance;

    private ImageFormatExt()
    {
        var imgFormats = ImageFormatInfo.GetImageFormatTypes();

        Jpeg = imgFormats.FirstOrDefault(x => x.Name == "Jpeg");
        Pgn = imgFormats.FirstOrDefault(x => x.Name == "Pgn");
        Bmp = imgFormats.FirstOrDefault(x => x.Name == "Bmp");
        Webp = imgFormats.FirstOrDefault(x => x.Name == "Webp");
        Gif = imgFormats.FirstOrDefault(x => x.Name == "Gif");
    }

    public ImageFormatInfo Jpeg { get; }
    public ImageFormatInfo Pgn { get; }
    public ImageFormatInfo Bmp { get; }
    public ImageFormatInfo Webp { get; }
    public ImageFormatInfo Gif { get; }

    public static ImageFormatExt Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ImageFormatExt();
            }

            return _instance;
        }
    }
}