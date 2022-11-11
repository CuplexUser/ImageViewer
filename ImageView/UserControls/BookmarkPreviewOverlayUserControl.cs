using ImageViewer.Services;
using Serilog;

namespace ImageViewer.UserControls
{
    public partial class BookmarkPreviewOverlayUserControl : UserControl
    {
        private readonly ImageCacheService _imageCacheService;
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }

        public BookmarkPreviewOverlayUserControl(ImageCacheService imageCache)
        {
            _imageCacheService = imageCache;
            InitializeComponent();
        }

        public bool LoadImage(string filename)
        {
            try
            {
                OverlayPictureBox.Image = _imageCacheService.GetImageFromCache(filename);
                return true;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to load image: " + filename);
                OverlayPictureBox.Image = new Bitmap(100, 100);
            }

            return false;
        }

        public Size GetImageSize()
        {
            return OverlayPictureBox.Image?.Size ?? Size.Empty;
        }

        private void BookmarkPreviewOverlayUserControl_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                BookmarkPreviewContentPanel.Dock = DockStyle.Fill;
            }
        }
    }
}