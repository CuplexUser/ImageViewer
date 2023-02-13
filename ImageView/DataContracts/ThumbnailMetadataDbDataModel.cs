namespace ImageViewer.DataContracts;

[Serializable]
[DataContract(Name = "ThumbnailMetadataDbDataModel")]
public class ThumbnailMetadataDbDataModel
{
    [DataMember(Name = "DatabaseId", Order = 1)]
    public string DatabaseId { get; set; }

    [DataMember(Name = "BinaryBlobFilename", Order = 2)]
    public string BinaryBlobFilename { get; set; }

    [DataMember(Name = "LastUpdated", Order = 3)]
    public DateTime LastUpdated { get; set; }

    [DataMember(Name = "CreatedDate", Order = 4)]
    public DateTime CreatedDate { get; set; }

    [DataMember(Name = "ThumbnailEntries", Order = 5)]
    public List<ThumbnailEntryDataModel> ThumbnailEntries { get; set; }
}