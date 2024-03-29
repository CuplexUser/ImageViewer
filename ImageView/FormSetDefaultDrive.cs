﻿using ImageViewer.Utility;

namespace ImageViewer;

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
            foreach (var driveInfo in driveInfos.Where(driveInfo => driveInfo.IsReady)) availableDriveInfos.Add(driveInfo);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in FormSetDefaultDrive_Load()");
        }

        foreach (var availableDriveInfo in availableDriveInfos) cbDriveList.Items.Add(GetDriveInfoListItemText(availableDriveInfo));

        if (cbDriveList.Items.Count > 0)
        {
            cbDriveList.SelectedIndex = 0;
        }
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
        {
            driveInfoText += string.Format(" ({0}) ", driveInfo.Name.Replace("\\", ""));
        }
        else
        {
            driveInfoText += " [" + driveInfo.VolumeLabel + "]";
        }

        driveInfoText += string.Format("{0} free of {1} [{2}, {3}]",
            SystemIOHelper.FormatFileSizeToString(driveInfo.TotalFreeSpace),
            SystemIOHelper.FormatFileSizeToString(driveInfo.TotalSize),
            driveInfo.DriveType, driveInfo.DriveFormat);

        return driveInfoText;
    }
}