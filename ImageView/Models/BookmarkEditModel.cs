using GeneralToolkitLib.Converters;

namespace ImageViewer.Models
{
    public class BookmarkEditModel
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string CompletePath { get; set; }
        public long FileSize { get; set; }

        public string Directory
        {
            get
            {
                if (!string.IsNullOrEmpty(CompletePath))
                    return GeneralConverters.GetDirectoryNameFromPath(CompletePath, false);
                else
                    return string.Empty;
            }
        }
    }
}