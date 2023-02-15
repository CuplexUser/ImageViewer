using ImageViewer.Models;
using ImageViewer.Services;

namespace ImageViewer.UserControls;

public partial class ThumbnailScanDirectory : UserControl
{
    private readonly ThumbnailService _thumbnailService;
    private CancellationTokenSource _cancelTokenSource;
    private bool _directorySelected;
    private bool _scanningDirectory;

    public ThumbnailScanDirectory(ThumbnailService thumbnailService)
    {
        InitializeComponent();
        _thumbnailService = thumbnailService;
        //_thumbnailService.CompletedThumbnailScan += _thumbnailService_CompletedThumbnailScan;
    }

    private void _thumbnailService_CompletedThumbnailScan(object sender, EventArgs e)
    {
        _scanningDirectory = false;
        _cancelTokenSource = null;
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

    private void btnScan_Click(object sender, EventArgs e)
    {
        progressBar.Value = 0;
        _scanningDirectory = true;
        UpdateButtonState();
        var progress = new Progress<ThumbnailScanProgress>(Handler);
        _cancelTokenSource = new CancellationTokenSource();

        Task.Factory.StartNew(async () =>
        {
           await _thumbnailService.ThumbnailDirectoryScan(txtFolderPath.Text, progress, chbIncludeSubdirs.Checked, _cancelTokenSource.Token);
        }).ConfigureAwait(true);
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
        _cancelTokenSource?.CancelAfter(TimeSpan.FromSeconds(5));
        _thumbnailService.StopThumbnailScan();
    }

    public void OnFormClosed()
    {
        Task.Factory.StartNew(async () => { await _thumbnailService.SaveThumbnailDatabase(); }).Wait();

        if (_scanningDirectory)
        {
            CancelScan();
        }
    }
}