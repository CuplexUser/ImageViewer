using System;
using System.Windows.Forms;

namespace ImageViewer.UserControls
{
    public partial class SelectPassword : UserControl
    {
        public string SelectedPassword { get; private set; }
        private const int MinLength = 8;

        private void SelectPassword_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "";
        }

        public SelectPassword()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SetPassword();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var parentForm = this.ParentForm;
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.Cancel;
            }
        }

        private void SetPassword()
        {
            if (!VerifyPasswords())
            {
                return;
            }

            SelectedPassword = txtPassword.Text;
            var parentForm = this.ParentForm;
            if (parentForm != null)
            {
                parentForm.DialogResult = DialogResult.OK;
            }
        }

        private bool VerifyPasswords()
        {
            if (txtPassword.Text.Length < MinLength)
            {
                lblStatus.Text = $"Password length must be atleast {MinLength}";
                return false;
            }

            if (txtPassword.Text != txtPasswordConfirm.Text)
            {
                lblStatus.Text = "Passwords dident match";
                return false;
            }

            lblStatus.Text = "";
            return true;
        }

        private void txtPasswordConfirm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetPassword();
            }
        }
    }
}
