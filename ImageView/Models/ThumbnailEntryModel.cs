using ImageViewer.DataContracts;
using ImageViewer.Models.Import;

namespace ImageViewer.Models;

/// <summary>
/// ThumbnailEntryModel
/// </summary>
public class ThumbnailEntryModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThumbnailEntryModel"/> class.
    /// </summary>
    public ThumbnailEntryModel()
    {
        if (EntryId == Guid.Empty)
        {
            InitNew();
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThumbnailEntryModel"/> class.
    /// </summary>
    /// <param name="entryId">The entry identifier.</param>
    public ThumbnailEntryModel(Guid entryId)
    {
        EntryId = entryId;
    }

    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    /// <value>
    /// The entry identifier.
    /// </value>
    public Guid EntryId { get; protected set; }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    /// <value>
    /// The full name.
    /// </value>
    public string FullName { get; set; }

    /// <summary>
    /// Gets or sets the file position.
    /// </summary>
    /// <value>
    /// The file position.
    /// </value>
    public long FilePosition { get; set; }

    /// <summary>
    /// Gets or sets the size of the file.
    /// </summary>
    /// <value>
    /// The size of the file.
    /// </value>
    public int FileSize { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    /// <value>
    /// The creation date.
    /// </value>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// Gets or sets the original image model.
    /// </summary>
    /// <value>
    /// The original image model.
    /// </value>
    public ImageRefModel OriginalImageModel { get; set; }

    /// <summary>
    /// Gets or sets the size of the thumbnail.
    /// </summary>
    /// <value>
    /// The size of the thumbnail.
    /// </value>
    public Size ThumbnailSize { get; set; }

    /// <summary>
    /// Initializes the new.
    /// </summary>
    private void InitNew()
    {
        EntryId = Guid.NewGuid();
    }


    /// <summary>
    /// Creates the mapping.
    /// </summary>
    /// <param name="expression">The expression.</param>
    public static void CreateMapping(IProfileExpression expression)
    {
        expression.CreateMap<ThumbnailEntryModel, ThumbnailEntryDataModel>()
            .ForMember(d => d.EntryId, o => o.MapFrom(s => s.EntryId))
            .ForMember(d => d.FilePosition, o => o.MapFrom(s => s.FilePosition))
            .ForMember(d => d.FileSize, o => o.MapFrom(s => s.FileSize))
            .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
            .ForMember(d => d.OriginalImageModel, o => o.MapFrom(s => s.OriginalImageModel))
            .ForMember(d => d.ThumbnailSize, o => o.MapFrom(s => SizeDataModel.CreateFromSize(s.ThumbnailSize)))
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
            .ReverseMap()
            .ForMember(d => d.ThumbnailSize, o => o.MapFrom(s => s.ThumbnailSize.ToSize()));
    }
}