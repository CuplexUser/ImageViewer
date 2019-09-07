using System;
using System.IO;
using System.Windows.Forms;
using ImageViewer.Models.UserInteraction;
using ImageViewer.Services;

namespace ImageViewer
{
    public partial class FormLoad : Form
    {
        private string _baseSearchPath;
        private readonly ImageLoaderService _imageLoaderService;
        private readonly UserInteractionService _interactionService;

        public FormLoad(ImageLoaderService imageLoaderService, UserInteractionService interactionService)
        {
            _imageLoaderService = imageLoaderService;
            _interactionService = interactionService;
            InitializeComponent();
            _baseSearchPath = null;
        }

        public void SetBasePath(string path)
        {
            _baseSearchPath = path;
        }

        private void FormLoad_Load(object sender, EventArgs e)
        {
            lblImagesLoaded.Text = "0";
            lblStatus.Text = "Ready";
            lblBasePath.Text = _baseSearchPath;
            btnCancel.Enabled = false;
            _imageLoaderService.OnProgressUpdate += Instance_OnProgressUpdate;
            _imageLoaderService.OnImportComplete += Instance_OnImportComplete;
        }

        private void FormLoad_Shown(object sender, EventArgs e)
        {
            btnStart.Focus();
        }

        private void Instance_OnImportComplete(object sender, ProgressEventArgs e)
        {
            if (IsHandleCreated)
                Invoke(new UpdateProgressDelegate(UpdateProgressOnLocalThread), e.ProgressStatus.ToString(), e.ImagesLoaded, e.CompletionRate, true);
        }

        private void Instance_OnProgressUpdate(object sender, ProgressEventArgs e)
        {
            if (IsHandleCreated)
            {
                Invoke(new UpdateProgressDelegate(UpdateProgressOnLocalThread), e.ProgressStatus.ToString(), e.ImagesLoaded, e.CompletionRate, false);
            }
        }

        private void UpdateProgressOnLocalThread(string status, int imagesLoaded, double completionRate, bool completed)
        {
            lblImagesLoaded.Text = imagesLoaded.ToString();
            lblStatus.Text = status;
            if (completed)
            {
                progressBar1.Value = progressBar1.Maximum;
                btnStart.Enabled = true;
                btnCancel.Enabled = false;
                MessageBox.Show(this, $@"Successfuly loaded {imagesLoaded} images", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
                progressBar1.Value = Math.Min(progressBar1.Maximum, Convert.ToInt32(imagesLoaded * completionRate));
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (_baseSearchPath == null)
            {
                MessageBox.Show("Base path must be set");
                return;
            }
            if (!Directory.Exists(_baseSearchPath))
            {
                MessageBox.Show("Base path does not exist");
                return;
            }

            btnCancel.Enabled = true;
            progressBar1.Value = 0;
            bool result = await _imageLoaderService.RunImageImport(_baseSearchPath);

            if (!result)
            {
                _interactionService.InformUser(new UserInteractionInformation { Buttons = MessageBoxButtons.OK, Icon = MessageBoxIcon.Error, Message = "Image import failed", Label = "Error" }); 
            }

            btnStart.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _imageLoaderService.StopImport();
            btnStart.Enabled = true;
            lblStatus.Text = "Canceled";
        }

        private void FormLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && _imageLoaderService.IsRunningImport)
            {
                e.Cancel = true;
                MessageBox.Show("Cancel the import before closing this form");
            }
            else if (e.CloseReason != CloseReason.UserClosing && _imageLoaderService.IsRunningImport)
                _imageLoaderService.StopImport();
        }

        private delegate void UpdateProgressDelegate(string status, int imagesLoaded, double completionRate, bool completed);
    }
}