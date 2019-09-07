using System;
using System.Windows.Forms;

namespace ImageViewer.UserControls
{
    public partial class RenameBookmarkFolder : UserControl
    {
        private string _folderName;
        private int _bookmarks;

        public RenameBookmarkFolder()
        {
            InitializeComponent();
        }

        public void InitControl(string folderName, int bookmarks)
        {
            _folderName = folderName;
            _bookmarks = bookmarks;
        }

        private void RenameBookmarkFolder_Load(object sender, EventArgs e)
        {
            lblBookmarks.Text = _bookmarks.ToString();
            txtName.Text = _folderName;
            txtName.SelectAll();
            txtName.Select();
            txtName.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateName()) return;
            Form parentForm = ParentForm;
            if (parentForm == null) return;
            parentForm.DialogResult = DialogResult.OK;
            parentForm.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form parentForm = ParentForm;
            if (parentForm == null) return;
            parentForm.DialogResult = DialogResult.Cancel;
            parentForm?.Close();
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnOk_Click(this, new EventArgs());
            }
        }

        private bool ValidateName()
        {
            return txtName.Text.Length > 0;
        }

        public string GetNewFolderName()
        {
            return txtName.Text;
        }
    }
}
