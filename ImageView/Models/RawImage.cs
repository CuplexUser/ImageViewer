namespace ImageViewer.Models;

/// <summary>
///     Data storage class for image data
/// </summary>
public class RawImage
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RawImage" /> class.
    /// </summary>
    /// <param name="imageBytes">The image bytes.</param>
    public RawImage(byte[] imageBytes)
    {
        ImageData = imageBytes;
    }

    /// <summary>
    ///     Gets the image data.
    /// </summary>
    /// <value>
    ///     The image data.
    /// </value>
    public byte[] ImageData { get; }

    /// <summary>
    ///     Loads the image.
    /// </summary>
    /// <param name="loadImageToByteArrFunc">The load image to byte arr function.</param>
    /// <returns></returns>
    public Image LoadImage(Func<byte[], Image> loadImageToByteArrFunc)
    {
        return loadImageToByteArrFunc.Invoke(ImageData);
    }
}