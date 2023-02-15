using ImageMagick;
using ImageMagick.Configuration;

namespace ImageViewer.Providers;

[UsedImplicitly]
public class ImageProvider : ProviderBase
{
    private readonly ImageTypeConverter _imgConverter;

    public ImageProvider(ImageTypeConverter imgConverter)
    {
        _imgConverter = imgConverter;
        InitMagickNet();
    }

    private void InitMagickNet()
    {
        var configFiles = ConfigurationFiles.Default;
        configFiles.Policy.Data = @"
            <policymap>
              <policy domain=""delegate"" rights=""none"" pattern=""*"" />
              <policy domain=""coder"" rights=""none"" pattern=""*"" />
              <policy domain=""coder"" rights=""read|write"" pattern=""{GIF,JPEG,PNG,WEBP,BMP}"" />
            </policymap>";

        string temporaryDirectory = MagickNET.Initialize(configFiles);
    }

    public Image LoadFromByteArray(byte[] readBytes)
    {
        var ms = new MemoryStream(readBytes);
        var img = Image.FromStream(ms, true, true);
        return img;
    }

    private byte[] ImageToBytes(Image image)
    {
        var magicImage = _imgConverter.ConvertFromSystemImage(image);

        return magicImage.ToByteArray(MagickFormat.Jpeg);
    }

    public Image CreateThumbnail(FileInfo imgFileInfo, Size size)
    {
        var image = LoadImageFile(imgFileInfo);
        image.Resize(size.Width, size.Height);
        return _imgConverter.ConvertToSystemImage(image);
    }


    public MagickImage LoadImageFile(string fileName)
    {
        var fi = new FileInfo(fileName);
        var image = new MagickImage(fi);

        return image;
    }

    public MagickImage LoadImageFile(FileInfo imgFileInfo)
    {
        var image = new MagickImage(imgFileInfo);

        return image;
    }

    public Image LoadSystemImage(string filePath)
    {
        return Image.FromFile(filePath);
    }

    public Image RestoreImageFromCache(byte[] arg)
    {
        try
        {
            return LoadFromByteArray(arg);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "RestoreImageFromCache exception, byteArr Size {size}", arg?.Length);
        }

        return null;
    }

    public MagickImage CreateThumbnailFromImage(Image image, Size size)
    {
        var img = _imgConverter.ConvertFromSystemImage(image);
        img.Resize(size.Width, size.Height);

        return img;
    }

    public byte[] CreateThumbnailToByteArray(FileInfo fi, Size thumbnailSize)
    {
        var image = LoadImageFile(fi.FullName);
        image.Resize(thumbnailSize.Width, thumbnailSize.Height);
        return image.ToByteArray(MagickFormat.Jpeg);
    }
}