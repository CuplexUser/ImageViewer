using ImageViewer.Events;

namespace ImageViewer.Utility
{
    /// <summary>
    /// Thumbnail Data structure operation interface
    /// </summary>
    public interface IThumbnailOperation
    {
        /// <summary>
        /// Recreates the database.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        bool RecreateDatabase(WorkParameters parameters);

        /// <summary>
        /// Reduces the size of the cache.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        bool ReduceCacheSize(WorkParameters parameters);

        /// <summary>
        /// Removes all non accessible files.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        bool RemoveAllNonAccessibleFiles(WorkParameters parameters);

        /// <summary>
        /// Clears the database.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        bool ClearDatabase(WorkParameters parameters);
    }
}