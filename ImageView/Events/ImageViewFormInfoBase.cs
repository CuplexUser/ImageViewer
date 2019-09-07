using System.Windows.Forms;

namespace ImageViewer.Events
{
    public abstract class ImageViewFormInfoBase
    {
        protected ImageViewFormInfoBase(Form formReference)
        {
            FormReference = formReference;
        }

        public Form FormReference { get; }
    }
}