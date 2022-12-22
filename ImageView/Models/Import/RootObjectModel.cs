using ImageViewer.DataBinding;

namespace ImageViewer.Models.Import;

/// <summary>
///     RootObjectModel
/// </summary>
/// <seealso cref="ImageViewer.Models.Import.SourceCollectionBase" />
public class RootObjectModel : SourceCollectionBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RootObjectModel" /> class.
    /// </summary>
    /// <param name="fromDrive">From drive.</param>
    private RootObjectModel(DriveModel fromDrive)
    {
        Id = $"RootObjectModel-{Guid.NewGuid().ToString()}";
        Name = fromDrive.VolumeLabel;
        SourceDrive = fromDrive;
        RootDirectory = fromDrive.RootDirectory.FullName;
    }

    /// <summary>
    ///     Gets the source drive.
    /// </summary>
    /// <value>
    ///     The source drive.
    /// </value>
    public DriveModel SourceDrive { get; }

    /// <summary>
    ///     Gets the identifier.
    /// </summary>
    /// <value>
    ///     The identifier.
    /// </value>
    public new string Id { get; }

    /// <summary>
    ///     Gets the name.
    /// </summary>
    /// <value>
    ///     The name.
    /// </value>
    public string Name { get; }

    /// <summary>
    ///     Gets the root directory.
    /// </summary>
    /// <value>
    ///     The root directory.
    /// </value>
    public string RootDirectory { get; }


    public override string GetFolderPath()
    {
        return RootDirectory;
    }

    protected override List<SourceFolderModel> GetSourceFolders()
    {
        return ImportSourceDataLoader.GetSubfolders(this);
    }

    /// <summary>
    ///     Creates the root object.
    /// </summary>
    /// <param name="fromDrive">From drive.</param>
    /// <returns></returns>
    public static RootObjectModel CreateRootObject(DriveModel fromDrive)
    {
        return new RootObjectModel(fromDrive);
    }
}