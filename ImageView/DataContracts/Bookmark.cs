using System;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "Bookmark")]
    public class Bookmark
    {
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        [DataMember(Name = "SortOrder", Order = 1)]
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the name of the boookmark.
        /// </summary>
        /// <value>
        /// The name of the boookmark.
        /// </value>
        [DataMember(Name = "BoookmarkName", Order = 2)]
        public string BoookmarkName { get; set; }

        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        [DataMember(Name = "ParentFolderId", Order = 3)]
        public string ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        [DataMember(Name = "Directory", Order = 4)]
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataMember(Name = "FileName", Order = 5)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the complete path.
        /// </summary>
        /// <value>
        /// The complete path.
        /// </value>
        [DataMember(Name = "CompletePath", Order = 6)]
        public string CompletePath { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        [DataMember(Name = "Size", Order = 7)]
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        [DataMember(Name = "CreationTime", Order = 8)]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the last write time.
        /// </summary>
        /// <value>
        /// The last write time.
        /// </value>
        [DataMember(Name = "LastWriteTime", Order = 9)]
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets or sets the last access time.
        /// </summary>
        /// <value>
        /// The last access time.
        /// </value>
        [DataMember(Name = "LastAccessTime", Order = 10)]
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// Gets the size formated.
        /// </summary>
        /// <value>
        /// The size formated.
        /// </value>
        public string SizeFormated => GeneralToolkitLib.Converters.GeneralConverters.FormatFileSizeToString(Size);
    }
}