namespace ImageViewer.DataContracts;

[DataContract(Name = "RecentFileDataModel", Namespace = "ImageViewer.DataContracts")]
public class RecentFileDataModel
{
    [DataMember(Name = "Guid", Order = 1)] public Guid Guid { get; init; }

    /// <summary>
    ///     Gets or sets the sort order.
    /// </summary>
    /// <value>
    ///     The sort order.
    /// </value>
    [DataMember(Name = "SortOrder", Order = 2)]
    public int SortOrder { get; set; }

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    [DataMember(Name = "Name", Order = 3)]
    public string Name { get; set; }

    /// <summary>
    ///     Gets or sets the full path.
    /// </summary>
    /// <value>
    ///     The full path.
    /// </value>
    [DataMember(Name = "FullPath", Order = 4)]
    public string FullPath { get; set; }

    /// <summary>
    ///     Gets or sets the created date.
    /// </summary>
    /// <value>
    ///     The created date.
    /// </value>
    [DataMember(Name = "CreatedDate", Order = 5)]
    public DateTime CreatedDate { get; set; }
}