namespace ImageViewer.UserControls;

public partial class SelectPassword : UserControl
{
    private const int MinLength = 8;

    public SelectPassword()
    {
        InitializeComponent();
    }

    public string SelectedPassword { get; private set; }

    private void SelectPassword_Load(object sender, EventArgs e)
    {
        lblStatus.Text = "";
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        SetPassword();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        Form parentForm = ParentForm;
        if (parentForm != null) parentForm.DialogResult = DialogResult.Cancel;
    }

    private void SetPassword()
    {
        if (!VerifyPasswords()) return;

        SelectedPassword = txtPassword.Text;
        Form parentForm = ParentForm;
        if (parentForm != null) parentForm.DialogResult = DialogResult.OK;
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
        if (e.KeyCode == Keys.Enter) SetPassword();
    }
}