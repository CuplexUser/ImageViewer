namespace ImageViewer.Models;

/// <summary>
///     CachedImage
/// </summary>
public class CachedImage
{
    /// <summary>
    ///     The image data
    /// </summary>
    private byte[] _imageData;

    /// <summary>
    ///     Gets the filename.
    /// </summary>
    /// <value>
    ///     The filename.
    /// </value>
    public string Filename { get; private set; }

    /// <summary>
    ///     Gets the added to cache time.
    /// </summary>
    /// <value>
    ///     The added to cache time.
    /// </value>
    public DateTime AddedToCacheTime { get; private set; }


    /// <summary>
    ///     Gets the size.
    /// </summary>
    /// <value>
    ///     The size.
    /// </value>
    public int Size => _imageData?.Length ?? 0;

    /// <summary>
    ///     Gets the image bytes.
    /// </summary>
    /// <returns></returns>
    public byte[] GetImageBytes()
    {
        return _imageData;
    }

    /// <summary>
    ///     Gets the image.
    /// </summary>
    /// <param name="imageConverter">The image converter.</param>
    /// <returns></returns>
    public Image GetImage(Func<byte[], Image> imageConverter)
    {
        return imageConverter.Invoke(_imageData);
    }

    /// <summary>
    ///     Sets the image.
    /// </summary>
    /// <param name="imageConverter">The image converter.</param>
    /// <param name="fileName">Name of the file.</param>
    public void SetImage(Func<string, byte[]> imageConverter, string fileName)
    {
        _imageData = imageConverter.Invoke(fileName);
        Filename = fileName;
        AddedToCacheTime = DateTime.Now;
    }
}