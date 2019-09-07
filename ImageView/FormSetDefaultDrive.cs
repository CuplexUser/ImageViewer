using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using Serilog;

namespace ImageViewer
{
    public partial class FormSetDefaultDrive : Form
    {
        private List<DriveInfo> availableDriveInfos;

        public FormSetDefaultDrive()
        {
            InitializeComponent();
        }

        public string SelectedDrive { get; set; }

        private void FormSetDefaultDrive_Load(object sender, EventArgs e)
        {
            availableDriveInfos = new List<DriveInfo>();
            try
            {
                var driveInfos = DriveInfo.GetDrives();
                foreach (DriveInfo driveInfo in driveInfos.Where(driveInfo => driveInfo.IsReady))
                {
                    availableDriveInfos.Add(driveInfo);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in FormSetDefaultDrive_Load()");
            }

            foreach (DriveInfo availableDriveInfo in availableDriveInfos)
            {
                cbDriveList.Items.Add(GetDriveInfoListItemText(availableDriveInfo));
            }

            if (cbDriveList.Items.Count > 0)
                cbDriveList.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            SelectedDrive = availableDriveInfos[cbDriveList.SelectedIndex].Name;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private string GetDriveInfoListItemText(DriveInfo driveInfo)
        {
            string driveInfoText = driveInfo.VolumeLabel ?? "";
            if (!string.IsNullOrEmpty(driveInfo.Name))
                driveInfoText += string.Format(" ({0}) ", driveInfo.Name.Replace("\\", ""));
            else
                driveInfoText += " [" + driveInfo.VolumeLabel + "]";

            driveInfoText += string.Format("{0} free of {1} [{2}, {3}]",
                GeneralConverters.FormatFileSizeToString(driveInfo.TotalFreeSpace),
                GeneralConverters.FormatFileSizeToString(driveInfo.TotalSize),
                driveInfo.DriveType, driveInfo.DriveFormat);

            return driveInfoText;
        }
    }
}