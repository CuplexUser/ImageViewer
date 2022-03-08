using System;
using System.Collections.Generic;

namespace ImageViewer.Models.Import
{
    /// <summary>
    /// SourceCollectionBase
    /// </summary>
    public abstract class SourceCollectionBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; protected set; }


        /// <summary>
        /// The folders
        /// </summary>
        private List<SourceFolderModel> _folders;

        /// <summary>
        /// Gets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        public List<SourceFolderModel> Folders
        {
            get => _folders ?? (_folders = GetSourceFolders());
        }

        public abstract string GetFolderPath();

        /// <summary>
        /// Gets the source folders.
        /// </summary>
        /// <returns></returns>
        protected abstract List<SourceFolderModel> GetSourceFolders();

        protected SourceCollectionBase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}