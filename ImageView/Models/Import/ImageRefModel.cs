using System;

namespace ImageViewer.Models.Import
{
    /// <summary>
    /// ImageRefModel
    /// </summary>
    public class ImageRefModel
    {
        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        public string ParentFolderId { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public string Directory { get; set; }
        /// <summary>
        /// Gets or sets the complete path.
        /// </summary>
        /// <value>
        /// The complete path.
        /// </value>
        public string CompletePath { get; set; }
        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        /// <value>
        /// The type of the image.
        /// </value>
        public string ImageType { get; set; }
        /// <summary>
        /// Gets or sets the last modifies.
        /// </summary>
        /// <value>
        /// The last modifies.
        /// </value>
        public DateTime LastModifies { get; set; }
        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public long FileSize { get; set; }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public int SortOrder { get; set; } }
}