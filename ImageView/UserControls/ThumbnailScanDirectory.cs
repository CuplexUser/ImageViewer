﻿using System;
using System.Windows.Forms;
using ImageViewer.Models;
using ImageViewer.Services;

namespace ImageViewer.UserControls
{
    public partial class ThumbnailScanDirectory : UserControl
    {
        private readonly ThumbnailService _thumbnailService;
        private bool _directorySelected;
        private bool _scanningDirectory;

        public ThumbnailScanDirectory(ThumbnailService thumbnailService)
        {
            InitializeComponent();
            _thumbnailService = thumbnailService;
            _thumbnailService.CompletedThumbnailScan += _thumbnailService_CompletedThumbnailScan;
        }

        private void _thumbnailService_CompletedThumbnailScan(object sender, EventArgs e)
        {
            _scanningDirectory = false;
            UpdateButtonState();
        }

        private void ThumbnailScanDirectory_Load(object sender, EventArgs e)
        {
            chbIncludeSubdirs.Checked = true;
            UpdateButtonState();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtFolderPath.Text = folderBrowserDialog1.SelectedPath;
                _directorySelected = true;
            }
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            _scanningDirectory = true;
            UpdateButtonState();
            var progress = new Progress<ThumbnailScanProgress>(Handler);

            await _thumbnailService.ThumbnailDirectoryScan(txtFolderPath.Text, progress, chbIncludeSubdirs.Checked);
        }

        private void Handler(ThumbnailScanProgress thumbnailScanProgress)
        {
            if (thumbnailScanProgress.IsComplete)
            {
                progressBar.Value = progressBar.Maximum;
                _scanningDirectory = false;
                UpdateButtonState();
            }
            else
            {
                progressBar.Value = thumbnailScanProgress.PercentComplete;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_thumbnailService.RunningThumbnailScan)
            {
                CancelScan();
            }
            else
            {
                _scanningDirectory = false;
            }

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            btnScan.Enabled = _directorySelected;

            if (_scanningDirectory)
            {
                btnScan.Enabled = false;
                btnCancel.Enabled = true;
                btnBrowse.Enabled = false;
            }
            else
            {
                btnScan.Enabled = true;
                btnCancel.Enabled = false;
                btnBrowse.Enabled = true;
                //progressBar.Value = progressBar.Maximum;
            }
        }

        private void CancelScan()
        {
            _thumbnailService.StopThumbnailScan();
        }

        public void OnFormClosed()
        {
            if (_scanningDirectory)
                CancelScan();
        }
    }
}