namespace ImageViewer.Delegates;

public class UpdatePicBoxEventArgs : EventArgs
{
    public UpdatePicBoxEventArgs(IList<PictureBox> modelListList)
    {
        PictureBoxModelList = modelListList;
    }

    public UpdatePicBoxEventArgs()
    {
    }

    public IList<PictureBox> PictureBoxModelList { get; }
}