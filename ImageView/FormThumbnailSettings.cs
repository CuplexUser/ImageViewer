using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using ImageViewer.Services;

namespace ImageViewer
{
    public partial class FormThumbnailSettings : Form
    {
        private readonly ThumbnailService _thumbnailService;
        public FormThumbnailSettings(ThumbnailService thumbnailService)
        {
            _thumbnailService = thumbnailService;
            thumbnailService.StartedThumbnailScan += ThumbnailService_StartedThumbnailScan;
            thumbnailService.CompletedThumbnailScan += ThumbnailService_CompletedThumbnailScan;
            InitializeComponent();
        }

        private void ThumbnailService_CompletedThumbnailScan(object sender, EventArgs e)
        {
            btnUpdateCurrentUsage.Enabled = true;
        }

        private void ThumbnailService_StartedThumbnailScan(object sender, EventArgs e)
        {
            btnUpdateCurrentUsage.Enabled = false;
        }

        private void FormThumbnailSettings_Load(object sender, EventArgs e)
        {
            _thumbnailService.LoadThumbnailDatabase();
            UpdateInformationLabels();
            lblInfo.Text = "Progress Information.";
        }

        private void UpdateInformationLabels()
        {
            lblCurrentDbSize.Text = GeneralConverters.FormatFileSizeToString(_thumbnailService.GetThumbnailDbSize());
            int thumbnailItems = _thumbnailService.GetNumberOfCachedThumbnails();
            lblCachedItems.Text = thumbnailItems > 0 ? _thumbnailService.GetNumberOfCachedThumbnails().ToString() : "n/a";
        }

        private async void BtnRunDefragJob_Click(object sender, EventArgs e)
        {
            btnRunDefragmentJob.Enabled = false;
            await _thumbnailService.OptimizeDatabaseAsync();
            btnRunDefragmentJob.Enabled = true;
            UpdateInformationLabels();
        }

        private void btnClearDatabase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to completely remove the thumbnail cache?", "Confirm delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (_thumbnailService.ClearDatabase())
                {
                    MessageBox.Show("Successively cleared the Thumbnail database.", "Database cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to clear the Thumbnail database because it is in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                UpdateInformationLabels();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnRemoveFilesNotFound_Click(object sender, EventArgs e)
        {
            btnRemoveFilesNotFound.Enabled = false;
            bool result = await Task<bool>.Factory.StartNew(() => _thumbnailService.RemoveAllNonAccessibleFilesAndSaveDb());
            btnRemoveFilesNotFound.Enabled = true;

            lblInfo.Text = result ? "Successfully removed missing files." : "No missing files where found.";
            UpdateInformationLabels();
        }

        private async void btnReduceCacheSize_Click(object sender, EventArgs e)
        {
            int maxSize = Convert.ToInt32(numericSize.Value);
            long truncatedSize = maxSize * 1048576;

            // Verify that the actual thumbnail database file is larger then the target size
            if (truncatedSize > _thumbnailService.GetThumbnailDbSize())
            {
                MessageBox.Show("The thumbnail database is already smaller then the selected size!", "Unable to truncate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnReduceCachSize.Enabled = false;
            bool result = await Task.Run(() => _thumbnailService.TruncateCacheSize(truncatedSize));

            MessageBox.Show(result ? "The thumbnail database was successfully truncated" : "Failed to truncate the database because the db is locked. Please try again in a minute", "Completed", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            UpdateInformationLabels();
            btnReduceCachSize.Enabled = true;
        }

        private void btnUpdateCurrentUsage_Click(object sender, EventArgs e)
        {
            if (!_thumbnailService.IsRunningScan)
            {
                UpdateInformationLabels();
            }
            else
            {
                MessageBox.Show("Can not update info values while a scan is running", "Scan is running", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
