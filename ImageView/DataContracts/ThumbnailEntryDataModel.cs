namespace ImageViewer.DataContracts;

/// <summary>
///     ThumbnailEntryDataModel
/// </summary>
[Serializable]
[DataContract(Name = "ThumbnailEntryDataModel", Namespace = "ImageViewer.DataContracts")]
public class ThumbnailEntryDataModel
{
    /// <summary>
    ///     Gets or sets the entry identifier.
    /// </summary>
    /// <value>
    ///     The entry identifier.
    /// </value>
    [DataMember(Name = "EntryId", Order = 1)]
    public Guid EntryId { get; set; }

    /// <summary>
    ///     Gets or sets the full name.
    /// </summary>
    /// <value>
    ///     The full name.
    /// </value>
    [DataMember(Name = "FullName", Order = 2)]
    public string FullName { get; set; }

    /// <summary>
    ///     Gets or sets the file position.
    /// </summary>
    /// <value>
    ///     The file position.
    /// </value>
    [DataMember(Name = "FilePosition", Order = 3)]
    public long FilePosition { get; set; }

    /// <summary>
    ///     Gets or sets the size of the file.
    /// </summary>
    /// <value>
    ///     The size of the file.
    /// </value>
    [DataMember(Name = "FileSize", Order = 4)]
    public int FileSize { get; set; }

    /// <summary>
    ///     Gets or sets the create date.
    /// </summary>
    /// <value>
    ///     The create date.
    /// </value>
    [DataMember(Name = "CreateDate", Order = 5)]
    public DateTime CreateDate { get; set; }

    /// <summary>
    ///     Gets or sets the size of the thumbnail.
    /// </summary>
    /// <value>
    ///     The size of the thumbnail.
    /// </value>
    [DataMember(Name = "ThumbnailSize", Order = 6)]
    public SizeDataModel ThumbnailSize { get; set; }

    /// <summary>
    ///     Gets or sets the original image model.
    /// </summary>
    /// <value>
    ///     The original image model.
    /// </value>
    [DataMember(Name = "OriginalImageModel", Order = 7)]
    public ImageReferenceDataModel OriginalImageModel { get; set; }
}