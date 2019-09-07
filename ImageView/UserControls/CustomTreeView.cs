using System.Windows.Forms;
using ImageView.DataModels;

namespace ImageView.UserControls
{
    public class CustomTreeView : Panel
    {
        public object DataSource { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DataSource is BookmarkTree)
            {
                var bookmarkTree = DataSource as BookmarkTree;

                //bookmarkTree.BookmarksFolders.
            }

            base.OnPaint(e);
        }
    }
}