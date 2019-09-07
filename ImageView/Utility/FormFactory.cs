using System.Drawing;
using System.Windows.Forms;

namespace ImageViewer.Utility
{
    public static class FormFactory
    {
        public static Form CreateSettingsForm(UserControl userControl)
        {
            Form frmSettings = new Form();
            frmSettings.Controls.Add(userControl);
            frmSettings.FormBorderStyle = FormBorderStyle.Fixed3D;
            frmSettings.StartPosition = FormStartPosition.CenterParent;
            frmSettings.ShowInTaskbar = false;
            frmSettings.ShowIcon = false;
            frmSettings.MaximizeBox = false;
            frmSettings.Width = frmSettings.Controls[0].Size.Width + 25;
            frmSettings.Height = frmSettings.Controls[0].Size.Height + 55;

            return frmSettings;
        }

        public static Form CreateModalForm(UserControl userControl)
        {
            Form frmModal = new Form();
            frmModal.Controls.Add(userControl);
            frmModal.FormBorderStyle = FormBorderStyle.Fixed3D;
            frmModal.StartPosition = FormStartPosition.CenterParent;
            frmModal.ShowInTaskbar = false;
            frmModal.ShowIcon = false;
            frmModal.MaximizeBox = false;
            frmModal.Width = userControl.Controls[0].Size.Width + 25;
            frmModal.Height = userControl.Controls[0].Size.Height + 55;

            return frmModal;
        }

        public static Form CreateModalSimpleDialog(UserControl userControl)
        {
            Form frmModal = new Form();
            frmModal.Controls.Add(userControl);
            frmModal.FormBorderStyle = FormBorderStyle.None;
            frmModal.StartPosition = FormStartPosition.CenterParent;
            frmModal.ShowInTaskbar = false;
            frmModal.ShowIcon = false;

            userControl.BorderStyle = BorderStyle.FixedSingle;

            // Try to find a main raqpping panel
            Panel mainPanel = null;
            for (int i = 0; i < userControl.Controls.Count; i++)
            {
                if (userControl.Controls[i] is Panel panel)
                {
                    mainPanel = panel;
                    break;
                }
            }

            if (mainPanel != null)
            {
                mainPanel.Margin = new Padding(5);
            }

            frmModal.Width = userControl.Controls[0].Size.Width + 10;
            frmModal.Height = userControl.Controls[0].Size.Height + 10;


            return frmModal;
        }

        public static Form CreateFloatingForm(UserControl userControl, Size formSize)
        {
            Form form = new Form();
            form.Controls.Add(userControl);
            form.FormBorderStyle = FormBorderStyle.None;
            form.StartPosition = FormStartPosition.CenterParent;
            form.TopMost = true;
            form.ShowInTaskbar = false;
            form.ShowIcon = false;
            form.Size = formSize;

            return form;
        }
    }
}
