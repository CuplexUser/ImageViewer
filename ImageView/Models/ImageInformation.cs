using System;
using GeneralToolkitLib.Converters;

namespace ImageViewer.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageInformation
    {
        /// <summary>
        /// Gets or sets the image dimensions.
        /// </summary>
        /// <value>
        /// The image dimensions.
        /// </value>
        public string ImageDimensions { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public long Size { get; set; }


        /// <summary>
        /// Gets the size formatted.
        /// </summary>
        /// <value>
        /// The size formatted.
        /// </value>
        public string SizeFormatted => GeneralConverters.FormatFileSizeToString(Size);

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file directory.
        /// </summary>
        /// <value>
        /// The file directory.
        /// </value>
        public string FileDirectory { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets the last modified date.
        /// </summary>
        /// <value>
        /// The last modified date.
        /// </value>
        public DateTime LastModifiedDate { get; set; }
        
    }
}
