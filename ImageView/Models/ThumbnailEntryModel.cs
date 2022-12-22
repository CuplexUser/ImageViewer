using ImageViewer.DataContracts;
using ImageViewer.Models.Import;

namespace ImageViewer.Models;

public class ThumbnailEntryModel
{
    public ThumbnailEntryModel()
    {
        if (EntryId == Guid.Empty)
            InitNew();
    }

    public Guid EntryId { get; protected set; }

    public long FilePosition { get; set; }

    public int Length { get; set; }

    public DateTime CreateDate { get; set; }

    public ImageRefModel OriginalImageModel { get; set; }

    public Size ThumbnailSize { get; set; }

    private void InitNew()
    {
        EntryId = Guid.NewGuid();
    }


    /// <summary>
    ///     Creates the mapping.
    /// </summary>
    /// <param name="expression">The expression.</param>
    public static void CreateMapping(IProfileExpression expression)
    {
        expression.CreateMap<ThumbnailEntryModel, ThumbnailEntryDataModel>()
            .ForMember(d => d.EntryId, o => o.MapFrom(s => s.EntryId))
            .ForMember(d => d.BinaryStartPosition, o => o.MapFrom(s => s.FilePosition))
            .ForMember(d => d.Length, o => o.MapFrom(s => s.Length))
            .ForMember(d => d.CreateDate, o => o.MapFrom(s => s.CreateDate))
            .ForMember(d => d.OriginalImageModel, o => o.MapFrom(s => s.OriginalImageModel))
            .ForMember(d => d.Size, o => o.MapFrom(s => SizeDataModel.CreateFromSize(s.ThumbnailSize)))
            .ReverseMap()
            .ForMember(d => d.ThumbnailSize, o => o.MapFrom(s => s.Size.ToSize()));
    }
}