using System.Drawing;

namespace ImageViewer.Models.Interface
{
    /// <summary>
    /// Interface used by a file Repository
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets a value indicating whether this instance is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is locked; otherwise, <c>false</c>.
        /// </value>
        bool IsLocked { get; }

        /// <summary>
        /// Reads the image.
        /// </summary>
        /// <param name="fileEntry">The file entry.</param>
        /// <returns></returns>
        Image ReadImage(FileEntry fileEntry);

        /// <summary>
        /// Writes the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns></returns>
        FileEntry WriteImage(Image img);

        /// <summary>
        /// Saves to disk.
        /// </summary>
        void SaveToDisk();

        /// <summary>
        /// Closes the stream.
        /// </summary>
        void CloseStream();

        /// <summary>
        /// Locks the database.
        /// </summary>
        /// <returns></returns>
        bool LockDatabase();

        /// <summary>
        /// Unlocks the database.
        /// </summary>
        void UnlockDatabase();
    }
}