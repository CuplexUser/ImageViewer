using System;
using System.IO;
using GeneralToolkitLib.Converters;

namespace ImageView.DataModels
{
    [Serializable]
    public abstract class ImageReferenceBase
    {
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string CompletePath { get; set; }
        public long Size { get; set; }

        public string FormatedFileSize
        {
            get { return GeneralConverters.FormatFileSizeToString(Size); }
        }

        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }

        public string FileExtention
        {
            get
            {
                if (FileName != null)
                    return Path.GetExtension(FileName);
                return null;
            }
        }
    }
}