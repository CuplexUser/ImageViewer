namespace ImageViewer.Models.Import
{
    public class ImageCollectionFile
    {
        public string FileName { get; set; }

        public string FullPath { get; set; }

        public bool IsSaved { get; set; }

        public bool IsChanged { get; set; }

        public static ImageCollectionFile CreateNew(string fileName = "newImageCollection.ivc")
        {
            var file = new ImageCollectionFile
            {
                FileName = fileName,
                IsChanged = false,
                IsSaved = false,
            };

            return file;
        }
    }
}