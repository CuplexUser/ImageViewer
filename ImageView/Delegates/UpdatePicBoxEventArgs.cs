namespace ImageViewer.Delegates;

public class UpdatePicBoxEventArgs : EventArgs
{
    public UpdatePicBoxEventArgs(PictureBox pictureBox)
    {
        PictureBoxModel = pictureBox;
    }

    public UpdatePicBoxEventArgs()
    {
    }

    public PictureBox PictureBoxModel { get; }
}