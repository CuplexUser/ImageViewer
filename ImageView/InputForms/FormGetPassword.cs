using System;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;

namespace ImageViewer.InputForms
{
    public partial class FormGetPassword : Form
    {
        public FormGetPassword()
        {
            InitializeComponent();
            PasswordVerified = false;
        }

        public string PasswordDerivedString { get; set; }
        public string PasswordString { get; private set; }
        public bool PasswordVerified { get; private set; }

        private void FormGetPassword_Shown(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            HandleOkClick();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormGetPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();

            if (e.KeyCode == Keys.Enter)
                HandleOkClick();
        }

        private void HandleOkClick()
        {
            if (PasswordDerivedString != GeneralConverters.GeneratePasswordDerivedString(txtPassword.Text))
            {
                MessageBox.Show(this, "Invalid password");
                return;
            }
            PasswordString = txtPassword.Text;
            PasswordVerified = true;
            Close();
            DialogResult = DialogResult.OK;
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                e.Handled = true;
                HandleOkClick();
            }
        }
    }
}