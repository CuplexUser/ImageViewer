using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ImageViewer.Properties;

namespace ImageViewer.Events
{
    /// <summary>
    /// Application eventHandlers and eventArgs
    /// </summary>
    public class WorkParameters
    {
        /// <summary>
        /// Gets the maximum size.
        /// </summary>
        /// <value>
        /// The maximum size.
        /// </value>
        public long MaxSize { get; }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static WorkParameters Empty { get; } = new WorkParameters();

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkParameters"/> class.
        /// </summary>
        public WorkParameters()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkParameters"/> class.
        /// </summary>
        /// <param name="maxSize">The maximum size.</param>
        public WorkParameters(long maxSize)
        {
            MaxSize = maxSize;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    public delegate bool MaintenanceDelegate(Func<WorkParameters, bool> parameters);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="TransitionImageUpdateEventArgs"/> instance containing the event data.</param>
    public delegate void TransitionImageUpdateEventHandler(object sender, TransitionImageUpdateEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="IntervalEventArgs"/> instance containing the event data.</param>
    public delegate void IntervalChangedDelegate(object sender, IntervalEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class IntervalEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalEventArgs"/> class.
        /// </summary>
        public IntervalEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalEventArgs"/> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        public IntervalEventArgs(int interval)
        {
            Interval = interval;
        }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public int Interval { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="BookmarkUpdatedEventArgs"/> instance containing the event data.</param>
    public delegate void BookmarkUpdatedEventHandler(object sender, BookmarkUpdatedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class BookmarkUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="bookmarkAction">The bookmark action.</param>
        /// <param name="bookmarkType">Type of the bookmark.</param>
        public BookmarkUpdatedEventArgs(BookmarkActions bookmarkAction, Type bookmarkType)
        {
            BookmarkAction = bookmarkAction;
            BookmarkType = bookmarkType;
        }


        /// <summary>
        /// Gets the bookmark action.
        /// </summary>
        /// <value>
        /// The bookmark action.
        /// </value>
        public BookmarkActions BookmarkAction { get; private set; }

        /// <summary>
        /// Gets the type of the bookmark.
        /// </summary>
        /// <value>
        /// The type of the bookmark.
        /// </value>
        public Type BookmarkType { get; private set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="AccessExceptionEventArgs"/> instance containing the event data.</param>
    public delegate void AccessExceptionEvent(object sender, AccessExceptionEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class AccessExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// The exception
        /// </summary>
        private readonly Exception _exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public AccessExceptionEventArgs(Exception ex)
        {
            _exception = ex;
        }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <returns></returns>
        public Exception GetException()
        {
            return _exception;
        }

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        /// <returns></returns>
        public string GetExceptionMessage()
        {
            return _exception?.Message;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="DisposePictureBox">The dispose PictureBox.</param>
    public delegate void DisposePictureBoxDelegate(PictureBox DisposePictureBox);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pictureBoxes">The picture boxes.</param>
    public delegate void DisposePictureBoxListDelegate(IList<PictureBox> pictureBoxes);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="DisposePicBoxEventArgs"/> instance containing the event data.</param>
    public delegate void DisposePictureBoxEvent(object sender, DisposePicBoxEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class DisposePicBoxEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the PictureBox awaiting disposal.
        /// </summary>
        /// <value>
        /// The PictureBox awaiting disposal.
        /// </value>
        public PictureBox PictureBoxAwaitingDisposal { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposePicBoxEventArgs"/> class.
        /// </summary>
        /// <param name="pictureBox">The picture box.</param>
        public DisposePicBoxEventArgs(PictureBox pictureBox)
        {
            if (pictureBox == null || pictureBox.Disposing || pictureBox.IsDisposed)
            {
                throw new ArgumentException(Resources.PictureBoxNullOrDisposed);
            }
            PictureBoxAwaitingDisposal = pictureBox;
        }

        /// <summary>
        /// Disposes the PictureBox.
        /// </summary>
        /// <param name="disposeDelegate">The dispose delegate.</param>
        public void DisposePictureBox(DisposePictureBoxDelegate disposeDelegate)
        {
            disposeDelegate?.Invoke(PictureBoxAwaitingDisposal);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum BookmarkActions
    {
        /// <summary>
        /// The created bookmark
        /// </summary>
        CreatedBookmark = 1,
        /// <summary>
        /// The created bookmark folder
        /// </summary>
        CreatedBookmarkFolder = 2,
        /// <summary>
        /// The deleted bookmark
        /// </summary>
        DeletedBookmark = 4,
        /// <summary>
        /// The deleted bookmark folder
        /// </summary>
        DeletedBookmarkFolder = 8,
        /// <summary>
        /// The edited bookmark
        /// </summary>
        EditedBookmark = 16,
        /// <summary>
        /// The edited bookmark folder
        /// </summary>
        EditedBookmarkFolder = 32,
        /// <summary>
        /// The loaded new data source
        /// </summary>
        LoadedNewDataSource = 64,
    }
}