namespace ImageViewer.Models;

/// <summary>
///     PictureBoxModelList
/// </summary>
public class PictureBoxModel
{
    /// <summary>
    ///     Gets or sets the size of the thumbnail.
    /// </summary>
    /// <value>
    ///     The size of the thumbnail.
    /// </value>
    public Size ThumbnailSize { get; set; }

    /// <summary>
    ///     Gets or sets the source image path.
    /// </summary>
    /// <value>
    ///     The source image path.
    /// </value>
    public string SourceImagePath { get; set; }

    /// <summary>
    ///     Gets or sets the color.
    /// </summary>
    /// <value>
    ///     The color.
    /// </value>
    public Color BackColor { get; set; }

    /// <summary>
    ///     Gets the border style.
    /// </summary>
    /// <value>
    ///     The border style.
    /// </value>
    public BorderStyle BorderStyle { get; set; }


    /// <summary>
    ///     Gets the size mode.
    /// </summary>
    /// <value>
    ///     The size mode.
    /// </value>
    public PictureBoxSizeMode SizeMode { get; set; }
}