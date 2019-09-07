using System;
using System.IO;

namespace ImageView.DataModels
{
    [Serializable]
    public class Bookmark : ImageReferenceBase
    {
        public bool FileAvailable
        {
            get { return File.Exists(CompletePath); }
        }

        public int SortOrder { get; set; }
        public string BoookmarkName { get; set; }
    }
}