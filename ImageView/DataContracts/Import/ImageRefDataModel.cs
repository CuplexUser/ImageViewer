using System;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts.Import
{
    /// <summary>
    /// ImageRefDataModel
    /// </summary>
    [DataContract(Name = "ImageRefDataModel")]
    public class ImageRefDataModel
    {
        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        [DataMember(Name = "ParentFolderId")]
        public string ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataMember(Name = "FileName")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        [DataMember(Name = "Directory")]
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the complete path.
        /// </summary>
        /// <value>
        /// The complete path.
        /// </value>
        [DataMember(Name = "CompletePath")]
        public string CompletePath { get; set; }

        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        /// <value>
        /// The type of the image.
        /// </value>
        [DataMember(Name = "ImageType")]
        public string ImageType { get; set; }

        /// <summary>
        /// Gets or sets the last modifies.
        /// </summary>
        /// <value>
        /// The last modifies.
        /// </value>
        [DataMember(Name = "LastModifies")]
        public DateTime LastModifies { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember(Name = "FileSize")]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        [DataMember(Name = "SortOrder")]
        public int SortOrder { get; set; }
    }
}