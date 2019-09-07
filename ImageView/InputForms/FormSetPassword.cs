using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ImageViewer.InputForms
{
    public partial class FormSetPassword : Form
    {
        private readonly Regex passwordPattern;
        private string errorMessage;

        public FormSetPassword()
        {
            InitializeComponent();
            passwordPattern = new Regex(@"^(?!.*\s)(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$");
            VerifiedPassword = null;
        }

        public string VerifiedPassword { get; set; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidatePasswords())
                MessageBox.Show(errorMessage);
            else
            {
                VerifiedPassword = txtPassword1.Text;
                Close();
            }
        }

        private bool ValidatePasswords()
        {
            errorMessage = null;
            if (txtPassword1.Text != txtPassword2.Text)
                errorMessage = "Passwords did not match!";
            else if (txtPassword1.Text.Length < 8)
                errorMessage = "Password needs to bee atleast 8 characters long";
            else if (!passwordPattern.IsMatch(txtPassword1.Text))
                errorMessage =
                    "Password did not mach the required complexity or did contain illegal characters like whitespaces.";

            return errorMessage == null;
        }

        private void txtPassword1_Enter(object sender, EventArgs e)
        {
            txtPassword1.SelectAll();
        }

        private void txtPassword2_Enter(object sender, EventArgs e)
        {
            txtPassword2.SelectAll();
        }
    }
}