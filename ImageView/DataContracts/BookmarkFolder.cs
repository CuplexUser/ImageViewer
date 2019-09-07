using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "BookmarkFolder")]
    public class BookmarkFolder
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "Id", Order = 1)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the bookmark folders.
        /// </summary>
        /// <value>
        /// The bookmark folders.
        /// </value>
        [DataMember(Name = "BookmarkFolders", Order = 2, IsRequired = true)]
        public List<BookmarkFolder> BookmarkFolders { get; set; }

        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        [DataMember(Name = "ParentFolderId", Order = 3)]
        public string ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "Name", Order = 4)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        [DataMember(Name = "SortOrder", Order = 5)]
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the bookmarks.
        /// </summary>
        /// <value>
        /// The bookmarks.
        /// </value>
        [DataMember(Name = "Bookmarks", Order = 6, IsRequired = true)]
        public List<Bookmark> Bookmarks { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
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