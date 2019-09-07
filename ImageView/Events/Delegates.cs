using System;
using System.Drawing;

namespace ImageViewer.Events
{
    public delegate void TransitionImageUpdateEventHandler(object sender, TransitionImageUpdateEventArgs e);

    public class TransitionImageUpdateEventArgs : EventArgs
    {
        public Image TransitionImage { get; set; }
    }

    public delegate void IntervalChangedDeligate(object sender, IntervalEventArgs e);

    public class IntervalEventArgs : EventArgs
    {
        public IntervalEventArgs()
        {
        }

        public IntervalEventArgs(int interval)
        {
            Interval = interval;
        }

        public int Interval { get; set; }
    }

    public delegate void BookmarkUpdatedEventHandler(object sender, BookmarkUpdatedEventArgs e);

    public class BookmarkUpdatedEventArgs : EventArgs
    {
        public BookmarkUpdatedEventArgs(BookmarkActions bookmarkAction, Type bookmarkType)
        {
            BookmarkAction = bookmarkAction;
            BookmarkType = bookmarkType;
        }


        public BookmarkActions BookmarkAction { get; private set; }

        public Type BookmarkType { get; private set; }
    }

    public delegate void AccessExceptionEvent(object sender, AccessExceptionEventArgs e);

    public class AccessExceptionEventArgs : EventArgs
    {
        private readonly Exception _exception;

        public AccessExceptionEventArgs(Exception ex)
        {
            _exception = ex;
        }

        public Exception GetException()
        {
            return _exception;
        }

        public string GetExceptionMessage()
        {
            return _exception?.Message;
        }
    }

    [Flags]
    public enum BookmarkActions
    {
        CreatedBookmark = 1,
        CreatedBookmarkFolder = 2,
        DeletedBookmark = 4,
        DeletedBookmarkFolder = 8,
        EditedBookmark = 16,
        EditedBookmarkFolder = 32,
        LoadedNewDataSource = 64,
    }
}