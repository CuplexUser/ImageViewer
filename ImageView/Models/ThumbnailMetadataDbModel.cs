using ImageViewer.DataContracts;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace ImageViewer.Models;

/// <summary>
///     Thumbnail Metadata Database Model
/// </summary>
public class ThumbnailMetadataDbModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ThumbnailMetadataDbModel" /> class.
    /// </summary>
    public ThumbnailMetadataDbModel()
    {
    }

    /// <summary>
    ///     Gets or sets the database identifier.
    /// </summary>
    /// <value>
    ///     The database identifier.
    /// </value>
    public string DatabaseId { get; set; }

    /// <summary>
    ///     Gets or sets the binary BLOB filename.
    /// </summary>
    /// <value>
    ///     The binary BLOB filename.
    /// </value>
    public string BinaryBlobFilename { get; private init; }

    /// <summary>
    ///     Gets or sets the last updated.
    /// </summary>
    /// <value>
    ///     The last updated.
    /// </value>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    ///     Gets or sets the created date.
    /// </summary>
    /// <value>
    ///     The created date.
    /// </value>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    ///     Gets the thumbnail entries.
    /// </summary>
    /// <value>
    ///     The thumbnail entries.
    /// </value>
    public ConcurrentDictionary<string, ThumbnailEntryModel> ThumbnailEntries { get; set; }

    /// <summary>
    ///     Creates the model.
    /// </summary>
    /// <param name="binaryBlobFilePath">The binary BLOB file path.</param>
    /// <returns></returns>
    public static ThumbnailMetadataDbModel CreateModel(string binaryBlobFilePath)
    {
        var model = new ThumbnailMetadataDbModel
        {
            DatabaseId = Guid.NewGuid().ToString("D"),
            CreatedDate = DateTime.Now,
            LastUpdated = DateTime.Now,
            ThumbnailEntries = new ConcurrentDictionary<string, ThumbnailEntryModel>(),
            BinaryBlobFilename = binaryBlobFilePath
        };

        return model;
    }

    public static List<ThumbnailEntryModel> CreateNewList(ConcurrentDictionary<string, ThumbnailEntryModel> sourceDict)
    {
        var list = new List<ThumbnailEntryModel>();

        foreach (var entry in sourceDict.Values)
        {
            list.Add(entry);
        }

        return list;

    }

    public static ConcurrentDictionary<string, ThumbnailEntryModel> CreateNewDictionaty(List<ThumbnailEntryDataModel> sourceList)
    {
        var dict = new ConcurrentDictionary<string, ThumbnailEntryModel>();

        foreach (var entry in sourceList)
        {
         


        }

        return dict;
    }


    /// <summary>
    ///     Creates the mapping.
    /// </summary>
    /// <param name="expression">The expression.</param>
    public static void CreateMapping(IProfileExpression profileExpression)
    {
        profileExpression.CreateMap<ThumbnailMetadataDbModel, ThumbnailMetadataDbDataModel>()
            .ForMember(dest => dest.DatabaseId, opt => opt.MapFrom(src => src.DatabaseId))
            .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.LastUpdated))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
            .ForMember(dest => dest.BinaryBlobFilename, opt => opt.MapFrom(src => src.BinaryBlobFilename))
            .ForMember(dest => dest.ThumbnailEntries, opt => opt.MapFrom(src => src.ThumbnailEntries.Values.ToImmutableList()))
            .ReverseMap();


        profileExpression.CreateMap<ThumbnailEntryModel, ThumbnailEntryDataModel>()
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
            .ForMember(dest => dest.FilePosition, opt => opt.MapFrom(src => src.FilePosition))
            .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
            .ForMember(dest => dest.ThumbnailSize, opt => opt.MapFrom(src => new SizeDataModel(src.ThumbnailSize.Width, src.ThumbnailSize.Height)))
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
            .ForMember(dest => dest.EntryId, opt => opt.MapFrom(src => src.EntryId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.OriginalImageModel, opt => opt.MapFrom(src => src.OriginalImageModel))
            .ReverseMap();
    }



    /// <summary>
    ///     Converts to string.
    /// </summary>
    /// <returns>
    ///     A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return $"{nameof(DatabaseId)}: {DatabaseId}, {nameof(BinaryBlobFilename)}: {BinaryBlobFilename}, {nameof(ThumbnailEntries)}: {ThumbnailEntries?.Count}";
    }
}