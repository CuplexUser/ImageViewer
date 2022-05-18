namespace ImageViewer.Delegates
{
    public delegate void UpdatePicBoxListEventHandler(object sender, UpdatePicBoxEventArgs eventArgs);

    public class UpdatePicBoxEventArgs : EventArgs
    {
        public IList<PictureBox> PictureBoxModelList { get; }

        public UpdatePicBoxEventArgs(IList<PictureBox> modelListList)
        {
            PictureBoxModelList = modelListList;
        }

        public UpdatePicBoxEventArgs()
        {
        }
    }
}