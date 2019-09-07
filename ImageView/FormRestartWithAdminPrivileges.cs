using System;
using System.Drawing;
using System.Windows.Forms;
using GeneralToolkitLib.WindowsApi;
using GeneralToolkitLib.WindowsApi.UserAccountControl;

namespace ImageViewer
{
    public partial class FormRestartWithAdminPrivileges : Form
    {
        private readonly CSUACSelfElevation _uacElevation;
        private Icon _formIcon;

        public FormRestartWithAdminPrivileges()
        {
            InitializeComponent();
            _uacElevation = new CSUACSelfElevation(true);
        }

        private void FormRestartWithAdminPrivileges_Load(object sender, EventArgs e)
        {
            _formIcon = Icon.FromHandle(LoadSystemIcon.GetShieldIcon().GetHicon());
            Icon = _formIcon;
            _uacElevation.UacButton = btnRestartWithAdminAccess;
            _uacElevation.Initialize();
            _uacElevation.DisplayMessagBoxes = true;
            SyncControlStateWithCoreObjectState();
            DialogResult = DialogResult.None;
        }

        private void btnRestartWithAdminAccess_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            _uacElevation.ElevateProcessToRunAsAdmin();
        }

        private void btnCansel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SyncControlStateWithCoreObjectState()
        {
            lblApplicationRunningAsAdmin.Text = _uacElevation.IsApplicationRuningnWithAdminAccess.ToString();
            lblCurrenUserInAdminGrp.Text = _uacElevation.CurrentUserInAdminGroup.ToString();
            lblProcessElevated.Text = _uacElevation.IsElevated.ToString();
            lblIntegrityLevel.Text = _uacElevation.IntegrityLevel.Trim();

            btnRestartWithAdminAccess.Enabled = !_uacElevation.IsElevated;
        }
    }
}