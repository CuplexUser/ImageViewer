using System;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using ImageViewer.DataContracts;

namespace ImageViewer.Models
{
    /// <summary>
    /// ThumbnailDatabase 
    /// </summary>
    public class ThumbnailDatabase
    {
        /// <summary>
        /// Gets or sets the thumbnail entries.
        /// </summary>
        /// <value>
        /// The thumbnail entries.
        /// </value>
        public EditableList<ThumbnailEntry> ThumbnailEntries { get; set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="ThumbnailDatabase"/> class from being created.
        /// </summary>
        private ThumbnailDatabase()
        {
        }

        /// <summary>
        /// Creates the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static ThumbnailDatabase Create(string filename)
        {
            var db = new ThumbnailDatabase {ThumbnailEntries = new EditableList<ThumbnailEntry>(), LastUpdated = DateTime.Now, DatabaseId = Guid.NewGuid().ToString(), DataStoragePath = filename};

            return db;
        }

        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Gets or sets the data storage path.
        /// </summary>
        /// <value>
        /// The data storage path.
        /// </value>
        public string DataStoragePath { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ThumbnailDatabase, ThumbnailDatabaseModel>()
                .ForMember(d => d.DatabaseId, o => o.MapFrom(s => s.DatabaseId))
                .ForMember(d => d.DataStoragePath, o => o.MapFrom(s => s.DataStoragePath))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => s.LastUpdated))
                .ForMember(d => d.ThumbnailEntries, o => o.MapFrom(s => s.ThumbnailEntries))
                .ReverseMap();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{nameof(ThumbnailEntries)}: {ThumbnailEntries}, {nameof(DatabaseId)}: {DatabaseId}, {nameof(DataStoragePath)}: {DataStoragePath}, {nameof(LastUpdated)}: {LastUpdated}";
        }
    }
}