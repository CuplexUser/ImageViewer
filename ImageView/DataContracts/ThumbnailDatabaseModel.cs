using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ImageViewer.Models;

namespace ImageViewer.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "ThumbnailDatabaseModel")]
    public class ThumbnailDatabaseModel
    {
        /// <summary>
        /// Gets or sets the thumbnail entries.
        /// </summary>
        /// <value>
        /// The thumbnail entries.
        /// </value>
        [DataMember(Name = "ThumbnailEntries", Order = 1)]
        public List<ThumbnailEntryModel> ThumbnailEntries { get; set; }

        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        [DataMember(Name = "DatabaseId", Order = 2)]
        public string DatabaseId { get; set; }


        /// <summary>
        /// Gets or sets the data storage path.
        /// </summary>
        /// <value>
        /// The data storage path.
        /// </value>
        [DataMember(Name = "DataStoragePath", Order = 3)]
        public string DataStoragePath { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        [DataMember(Name = "LastUpdated", Order = 4)]
        public DateTime LastUpdated { get; set; }
    }
}
