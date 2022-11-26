using AutoMapper;
using ImageViewer.DataContracts;


namespace ImageViewer.Models
{
    /// <summary>
    /// Thumbnail Metadata Database Model
    /// </summary>
    public class ThumbnailMetadataDbModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailMetadataDbModel"/> class.
        /// </summary>
        public ThumbnailMetadataDbModel()
        {

        }

        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="binaryBlobFilePath">The binary BLOB file path.</param>
        /// <returns></returns>
        public static ThumbnailMetadataDbModel CreateModel(string binaryBlobFilePath)
        {
            var model= new ThumbnailMetadataDbModel
            {
                DatabaseId = Guid.NewGuid().ToString("D"),
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                ThumbnailEntries = new List<ThumbnailEntryModel>(),
                BinaryBlobFilename = binaryBlobFilePath
            };

            return model;
        }

        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>
        /// The database identifier.
        /// </value>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Gets or sets the binary BLOB filename.
        /// </summary>
        /// <value>
        /// The binary BLOB filename.
        /// </value>
        public string BinaryBlobFilename { get; private set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets the thumbnail entries.
        /// </summary>
        /// <value>
        /// The thumbnail entries.
        /// </value>
        public List<ThumbnailEntryModel> ThumbnailEntries { get; private set; }

        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ThumbnailMetadataDbModel, ThumbnailMetadataDbDataModel>()
                .ForMember(d => d.DatabaseId, o => o.MapFrom(s => s.DatabaseId))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => s.LastUpdated))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.BinaryBlobFilename, o => o.MapFrom(s => s.BinaryBlobFilename))
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
            return $"{nameof(DatabaseId)}: {DatabaseId}, {nameof(BinaryBlobFilename)}: {BinaryBlobFilename}, {nameof(ThumbnailEntries)}: {ThumbnailEntries?.Count}";
        }
    }
}