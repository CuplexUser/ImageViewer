using System.Windows.Forms;

namespace ImageViewer.Events
{
    public class ImageViewFormImageInfo : ImageViewFormInfoBase
    {
        public ImageViewFormImageInfo(Form formRef, string currentImageFileName, int imagesViewed) : base(formRef)
        {
            ImagesViewed = imagesViewed;
            CurrentImageFileName = currentImageFileName;
        }

        public bool FormIsClosing { get; set; }
        public int ImagesViewed { get; }
        public string CurrentImageFileName { get; }
    }
}