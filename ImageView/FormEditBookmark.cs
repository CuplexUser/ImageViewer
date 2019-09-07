using System;
using System.Windows.Forms;

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

        public void InitForRename(string bookmarkName, string filename)
        {
            renameBookmark1.InitControl(bookmarkName,filename);
        }

        public string GetNewName()
        {
            return renameBookmark1.GetNewBookmarkName();
        }
    }
}
