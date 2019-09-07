using System;
using System.Runtime.Serialization;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "BookmarkContainer", Namespace = "ImageView.DataContracts")]
    public class BookmarkContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkContainer"/> class.
        /// </summary>
        public BookmarkContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkContainer"/> class.
        /// </summary>
        /// <param name="rootFolder">The root folder.</param>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="lastUpdate">The last update.</param>
        public BookmarkContainer(BookmarkFolder rootFolder, string containerId, DateTime lastUpdate)
        {
            RootFolder = rootFolder;
            ContainerId = containerId;
            LastUpdate = lastUpdate;
        }

        /// <summary>
        /// Gets or sets the root folder.
        /// </summary>
        /// <value>
        /// The root folder.
        /// </value>
        [DataMember(Name = "RootFolder", Order = 1)]
        public BookmarkFolder RootFolder { get; set; }

        /// <summary>
        /// Gets or sets the container identifier.
        /// </summary>
        /// <value>
        /// The container identifier.
        /// </value>
        [DataMember(Name = "ContainerId", Order = 2)]
        public string ContainerId { get; set; }

        /// <summary>
        /// Gets or sets the last update.
        /// </summary>
        /// <value>
        /// The last update.
        /// </value>
        [DataMember(Name = "LastUpdate", Order = 3)]
        public DateTime LastUpdate { get; set; }
    }
}