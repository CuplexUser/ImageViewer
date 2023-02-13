namespace ImageViewer.Models.Import;

/// <summary>
///     SourceCollectionBase
/// </summary>
public abstract class SourceCollectionBase
{
    /// <summary>
    ///     The folders
    /// </summary>
    private List<SourceFolderModel> _folders;

    protected SourceCollectionBase()
    {
        Id = Guid.NewGuid().ToString();
    }

    /// <summary>
    ///     Gets or sets the identifier.
    /// </summary>
    /// <value>
    ///     The identifier.
    /// </value>
    public string Id { get; protected set; }

    /// <summary>
    ///     Gets the folders.
    /// </summary>
    /// <value>
    ///     The folders.
    /// </value>
    public List<SourceFolderModel> Folders => _folders ?? (_folders = GetSourceFolders());

    public abstract string GetFolderPath();

    /// <summary>
    ///     Gets the source folders.
    /// </summary>
    /// <returns></returns>
    protected abstract List<SourceFolderModel> GetSourceFolders();
}