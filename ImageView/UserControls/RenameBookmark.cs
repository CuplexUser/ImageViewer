using System;
using System.Windows.Forms;

namespace ImageViewer.UserControls
{
    public partial class RenameBookmark : UserControl
    {
        private string _bookmarkName;
        private string _filename;

        public RenameBookmark()
        {
            InitializeComponent();
        }

        public void InitControl(string bookmarkName, string filename)
        {
            _bookmarkName = bookmarkName;
            _filename = filename;
        }

        private void RenameBookmark_Load(object sender, EventArgs e)
        {
            lblFilename.Text = _filename;
            txtName.Text = _bookmarkName;
            txtName.SelectAll();
            txtName.Select();
            txtName.Focus();
        }

        private bool ValidateName()
        {
            return txtName.Text.Length > 0;
        }

        public string GetNewBookmarkName()
        {
            return txtName.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form parentForm = ParentForm;
            if (parentForm == null) return;
            parentForm.DialogResult = DialogResult.Cancel;
            parentForm?.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateName()) return;
            Form parentForm = ParentForm;
            if (parentForm == null) return;
            parentForm.DialogResult = DialogResult.OK;
            parentForm.Close();
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnOk_Click(this, new EventArgs());
            }
        }
    }
}
