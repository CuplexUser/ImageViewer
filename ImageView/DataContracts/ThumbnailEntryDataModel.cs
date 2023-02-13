namespace ImageViewer.DataContracts;

[Serializable]
[DataContract(Name = "ThumbnailEntryDataModel")]
public class ThumbnailEntryDataModel
{
    [DataMember(Name = "EntryId", Order = 1)]
    public Guid EntryId { get; set; }

    [DataMember(Name = "FullName", Order = 2)]
    public string FullName { get; set; }

    [DataMember(Name = "FilePosition", Order = 3)]
    public long BinaryStartPosition { get; set; }

    [DataMember(Name = "Length", Order = 4)]
    public int Length { get; set; }

    [DataMember(Name = "CreateDate", Order = 5)]
    public DateTime CreateDate { get; set; }

    [DataMember(Name = "Size", Order = 6)] 
    public SizeDataModel Size { get; set; }

    [DataMember(Name = "OriginalImageModel", Order = 7)]
    public ImageReferenceDataModel OriginalImageModel { get; set; }
}