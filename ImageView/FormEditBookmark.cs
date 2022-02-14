using System;
using System.Windows.Forms;
using ImageViewer.Models;

namespace ImageViewer
{
    public partial class FormEditBookmark : Form
    {
        public FormEditBookmark()
        {
            InitializeComponent();
        }

        private void FormEditBookmark_Load(object sender, EventArgs e)
        {

        }

        public void InitEditForm(BookmarkEditModel model, bool editFilename)
        {
            editBookmark1.InitControl(model, editFilename);
        }

        public BookmarkEditModel GetBookmarkEditModel()
        {
            return editBookmark1.GetEditModel();
        }
    }
}
