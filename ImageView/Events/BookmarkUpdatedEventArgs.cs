namespace ImageViewer.Events;

/// <summary>
/// </summary>
/// <seealso cref="System.EventArgs" />
public class BookmarkUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BookmarkUpdatedEventArgs" /> class.
    /// </summary>
    /// <param name="bookmarkAction">The bookmark action.</param>
    /// <param name="bookmarkType">Type of the bookmark.</param>
    public BookmarkUpdatedEventArgs(BookmarkActions bookmarkAction, Type bookmarkType)
    {
        BookmarkAction = bookmarkAction;
        BookmarkType = bookmarkType;
    }


    /// <summary>
    ///     Gets the bookmark action.
    /// </summary>
    /// <value>
    ///     The bookmark action.
    /// </value>
    public BookmarkActions BookmarkAction { get; }

    /// <summary>
    ///     Gets the type of the bookmark.
    /// </summary>
    /// <value>
    ///     The type of the bookmark.
    /// </value>
    public Type BookmarkType { get; }
}