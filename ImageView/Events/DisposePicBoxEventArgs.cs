using ImageViewer.Properties;

namespace ImageViewer.Events;

/// <summary>
/// </summary>
/// <seealso cref="System.EventArgs" />
public class DisposePicBoxEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DisposePicBoxEventArgs" /> class.
    /// </summary>
    /// <param name="pictureBox">The picture box.</param>
    public DisposePicBoxEventArgs(PictureBox pictureBox)
    {
        if (pictureBox == null || pictureBox.Disposing || pictureBox.IsDisposed) throw new ArgumentException(Resources.PictureBoxNullOrDisposed);

        PictureBoxAwaitingDisposal = pictureBox;
    }

    /// <summary>
    ///     Gets the PictureBox awaiting disposal.
    /// </summary>
    /// <value>
    ///     The PictureBox awaiting disposal.
    /// </value>
    public PictureBox PictureBoxAwaitingDisposal { get; }

    /// <summary>
    ///     Disposes the PictureBox.
    /// </summary>
    /// <param name="disposeDelegate">The dispose delegate.</param>
    public void DisposePictureBox(DisposePictureBoxDelegate disposeDelegate)
    {
        disposeDelegate?.Invoke(PictureBoxAwaitingDisposal);
    }
}