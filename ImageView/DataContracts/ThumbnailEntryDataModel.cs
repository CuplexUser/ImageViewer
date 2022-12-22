namespace ImageViewer.DataContracts;

[Serializable]
[DataContract(Name = "ThumbnailEntryDataModel")]
public class ThumbnailEntryDataModel
{
    [DataMember(Name = "EntryId", Order = 1)]
    public Guid EntryId { get; set; }

    [DataMember(Name = "FilePosition", Order = 2)]
    public long BinaryStartPosition { get; set; }

    [DataMember(Name = "Length", Order = 3)]
    public int Length { get; set; }

    [DataMember(Name = "CreateDate", Order = 4)]
    public DateTime CreateDate { get; set; }

    [DataMember(Name = "Size", Order = 5)] public SizeDataModel Size { get; set; }

    [DataMember(Name = "OriginalImageModel", Order = 6)]
    public ImageReferenceDataModel OriginalImageModel { get; set; }
}