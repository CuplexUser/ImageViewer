namespace ImageViewer.Events;

/// <summary>
/// </summary>
[Flags]
public enum BookmarkActions
{
    /// <summary>
    ///     The created bookmark
    /// </summary>
    CreatedBookmark = 1,

    /// <summary>
    ///     The created bookmark folder
    /// </summary>
    CreatedBookmarkFolder = 2,

    /// <summary>
    ///     The deleted bookmark
    /// </summary>
    DeletedBookmark = 4,

    /// <summary>
    ///     The deleted bookmark folder
    /// </summary>
    DeletedBookmarkFolder = 8,

    /// <summary>
    ///     The edited bookmark
    /// </summary>
    EditedBookmark = 16,

    /// <summary>
    ///     The edited bookmark folder
    /// </summary>
    EditedBookmarkFolder = 32,

    /// <summary>
    ///     The loaded new data source
    /// </summary>
    LoadedNewDataSource = 64
}