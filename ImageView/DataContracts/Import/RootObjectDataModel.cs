namespace ImageViewer.DataContracts.Import;

/// <summary>
///     RootObjectDataModel
/// </summary>
[DataContract(Name = "RootObjectDataModel")]
public class RootObjectDataModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RootObjectDataModel" /> class.
    /// </summary>
    public RootObjectDataModel()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RootObjectDataModel" /> class.
    /// </summary>
    /// <param name="fromDrive">From drive.</param>
    private RootObjectDataModel(DriveDataModel fromDrive)
    {
        Id = $"RootObjectModel-{Guid.NewGuid().ToString()}";
        FullName = fromDrive.RootDirectory.FullName;
        Name = fromDrive.VolumeLabel;
        FromDrive = fromDrive;
    }

    /// <summary>
    ///     Gets or sets from drive.
    /// </summary>
    /// <value>
    ///     From drive.
    /// </value>
    [DataMember(Name = "FromDrive")]
    public DriveDataModel FromDrive { get; set; }

    /// <summary>
    ///     Gets or sets the folders.
    /// </summary>
    /// <value>
    ///     The folders.
    /// </value>
    [DataMember(Name = "Folders")]
    public List<SourceFolderDataModel> Folders { get; set; }

    /// <summary>
    ///     Gets or sets the identifier.
    /// </summary>
    /// <value>
    ///     The identifier.
    /// </value>
    [DataMember(Name = "Id")]
    public string Id { get; set; }

    /// <summary>
    ///     Gets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    [DataMember(Name = "Name")]
    public string Name { get; }

    /// <summary>
    ///     Gets or sets the sort order.
    /// </summary>
    /// <value>
    ///     The sort order.
    /// </value>
    [DataMember(Name = "SortOrder")]
    public int SortOrder { get; set; }

    /// <summary>
    ///     Gets the full name.
    /// </summary>
    /// <value>
    ///     The full name.
    /// </value>
    [DataMember(Name = "FullName")]
    public string FullName { get; }

    /// <summary>
    ///     Gets or sets the parent folder identifier.
    /// </summary>
    /// <value>
    ///     The parent folder identifier.
    /// </value>
    [DataMember(Name = "ParentFolderId")]
    public string ParentFolderId { get; set; }
}