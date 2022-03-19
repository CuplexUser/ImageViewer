using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ImageViewer.Models.Import;

namespace ImageViewer.DataContracts.Import
{
    /// <summary>
    /// SourceFolderDataModel
    /// </summary>
    [Serializable, DataContract(Name = "SourceFolderDataModel", Namespace = "ImageViewer.DataContracts.Import")]
    public class SourceFolderDataModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "Id", Order = 1, IsRequired = true)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        [DataMember(Name = "ParentFolderId", Order = 2)]
        public string ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        [DataMember(Name = "SortOrder", Order = 3)]
        public int SortOrder { get; set; }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "Name", Order = 4)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [DataMember(Name = "FullName", Order = 5)]
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        [DataMember(Name = "Folders", Order = 6)]
        public List<SourceFolderDataModel> Folders { get; set; }
        /// <summary>
        /// Gets or sets the image list.
        /// </summary>
        /// <value>
        /// The image list.
        /// </value>
        [DataMember(Name = "ImageList", Order = 7)]
        public List<ImageRefModel> ImageList { get; set; }

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