using System;
using System.Drawing;
using System.Windows.Forms;
using Serilog;

namespace ImageViewer.UserControls
{
    public partial class BookmarkPreviewOverlayUserControl : UserControl
    {
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }

        public BookmarkPreviewOverlayUserControl()
        {
            InitializeComponent();
        }

        public void LoadImage(string filename)
        {
            try
            {
                OverlayPictureBox.Image = Image.FromFile(filename);
            }
            catch (Exception exception)
            {
                Log.Error(exception,"Failed to load image: "+ filename);
                Console.WriteLine(exception);
            }
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
