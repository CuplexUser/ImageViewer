using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using ImageViewer.Models;

namespace ImageViewer.UserControls
{
    public partial class EditBookmark : UserControl
    {
        private BookmarkEditModel _editModel;
        private bool _editFilename;

        public EditBookmark()
        {
            InitializeComponent();
        }

        public void InitControl(BookmarkEditModel model, bool editFilename)
        {
            _editModel = model;
            _editFilename = editFilename;
        }

        private void EditBookmark_Load(object sender, EventArgs e)
        {
            txtFilename.Text = _editModel.CompletePath;
            txtName.Text = _editModel.Name;
            txtFilename.ReadOnly = !_editFilename;

            //UpdateFileStatus();
            txtName.SelectAll();
            txtName.Select();
            txtName.Focus();
        }

        private void UpdateFileStatus()
        {
            string fileName = txtFilename.Text;
            if (File.Exists(fileName))
            {
                FileInfo fi = new FileInfo(fileName);
                _editModel.FileSize = fi.Length;
                string template = $"{GeneralConverters.FormatFileSizeToString(fi.Length, 0)}, {fi.Extension}\n,{fi.Name}";
                lblFileInfo.Text = template;
            }
            else
            {
                lblFileInfo.Text = @"File does not exist";
            }

        }

        private bool ValidateForm()
        {
            bool valid = txtName.Text.Length > 0;
            valid = valid & txtFilename.Text.Length > 0 && File.Exists(txtFilename.Text);
            return valid;
        }

        private void UpdateModelState()
        {
            _editModel.CompletePath = txtFilename.Text;
            _editModel.Name = txtName.Text;
            _editModel.FileName = GeneralConverters.GetFileNameFromPath(_editModel.CompletePath);
        }

        public BookmarkEditModel GetEditModel()
        {
            return _editModel;
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
            if (!ValidateForm()) return;
            UpdateModelState();
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
                btnOk_Click(this, EventArgs.Empty);
            }
        }

        private void txtFilename_TextChanged(object sender, EventArgs e)
        {
            UpdateFileStatus();
        }
    }
}
