using System;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "ThumbnailEntryModel")]
    public class ThumbnailEntryModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailEntryModel"/> class.
        /// </summary>
        public ThumbnailEntryModel()
        {
            if (UniqueId == Guid.Empty)
            {
                UniqueId = Guid.NewGuid();
            }
        }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataMember(Name = "FullPath", Order = 1)]
        public string FullPath { get; set; }

        [DataMember(Name = "FileName", Order = 2)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        [DataMember(Name = "Directory", Order = 3)]
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the file position.
        /// </summary>
        /// <value>
        /// The file position.
        /// </value>
        [DataMember(Name = "FilePosition", Order = 4)]
        public long FilePosition { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [DataMember(Name = "Length", Order = 5)]
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [DataMember(Name = "Date", Order = 6)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the source image date.
        /// </summary>
        /// <value>
        /// The source image date.
        /// </value>
        [DataMember(Name = "SourceImageDate", Order = 7)]
        public DateTime SourceImageDate { get; set; }

        /// <summary>
        /// Gets or sets the length of the source image.
        /// </summary>
        /// <value>
        /// The length of the source image.
        /// </value>
        [DataMember(Name = "SourceImageLength", Order = 8)]
        public long SourceImageLength { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        /// 
        [DataMember(Name = "UniqueId", Order = 9, EmitDefaultValue = true)]
        public Guid UniqueId { get; protected set; }
    }
}
