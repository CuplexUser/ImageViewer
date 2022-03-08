using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts.Import
{
    /// <summary>
    /// SourceFolderDataModel
    /// </summary>
    [DataContract(Name = "SourceFolderDataModel")]
    public class SourceFolderDataModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        [DataMember(Name = "ParentFolderId")]
        public string ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        [DataMember(Name = "SortOrder")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        [DataMember(Name = "Tag")]
        public SourceFolderDataModel Tag { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "Name")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [DataMember(Name = "FullName")]
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        [DataMember(Name = "Folders")]
        public List<SourceFolderDataModel> Folders { get; set; }
        /// <summary>
        /// Gets or sets the image list.
        /// </summary>
        /// <value>
        /// The image list.
        /// </value>
        [DataMember(Name = "ImageList")]
        protected List<ImageRefDataModel> ImageList { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}