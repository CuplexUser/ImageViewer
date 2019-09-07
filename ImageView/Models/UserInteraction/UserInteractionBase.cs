using System.Windows.Forms;

namespace ImageViewer.Models.UserInteraction
{
    public abstract class UserInteractionBase
    {
        public virtual MessageBoxButtons Buttons { get; set; }
        public virtual string Label { get; set; }
        public virtual string Message { get; set; }
        public virtual MessageBoxIcon Icon { get; set; }
    }
}
