using System;
using System.Windows.Forms;
using ImageViewer.Services;

namespace ImageViewer.UserControls
{
    public partial class ConfirmExitUserControl : UserControl
    {
        private readonly ApplicationSettingsService _applicationSettingsService;

        public ConfirmExitUserControl(ApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
            InitializeComponent();
        }

        private void ConfirmExitUserControl_Load(object sender, EventArgs e)
        {
            
        }

        private void chkDisableExitDialog_CheckedChanged(object sender, EventArgs e)
        {
            _applicationSettingsService.Settings.ConfirmApplicationShutdown = !chkDisableExitDialog.Checked;
            _applicationSettingsService.SetSettingsStateModified();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                _applicationSettingsService.SaveSettings();
                ParentForm.DialogResult = DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
