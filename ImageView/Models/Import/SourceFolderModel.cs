using System;
using System.Collections.Generic;
using AutoMapper;
using ImageViewer.DataBinding;
using ImageViewer.DataContracts.Import;
using JetBrains.Annotations;

namespace ImageViewer.Models.Import
{
    /// <summary>
    /// SourceFolderModel
    /// </summary>
    /// <seealso cref="ImageViewer.Models.Import.SourceCollectionBase" />
    public class SourceFolderModel : SourceCollectionBase
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public new string Id { get; }

        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        public string ParentFolderId { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullPath { get; }

        public override string GetFolderPath()
        {
            return FullPath;
        }

        protected override List<SourceFolderModel> GetSourceFolders()
        {
            return ImportSourceDataLoader.GetSubfolders(this);
        }

        /// <summary>
        /// Gets or sets the image list.
        /// </summary>
        /// <value>
        /// The image list.
        /// </value>
        public List<ImageRefModel> ImageList { get; set; }


        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceFolderModel"/> class.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        public SourceFolderModel(string fullPath)
        {
            Id = Guid.NewGuid().ToString();
            FullPath = fullPath;

            if (string.IsNullOrEmpty(FullPath))
            {
                throw new ArgumentException("fullPath canot be Null");
            }
        }

        [UsedImplicitly]
        public SourceFolderModel()
        {

        }

        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<SourceFolderModel, SourceFolderDataModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.FullName, o => o.MapFrom(d => d.FullPath))
                .ForMember(s => s.ParentFolderId, o => o.MapFrom(d => d.ParentFolderId))
                .ReverseMap()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Name))
                .ForMember(s => s.FullPath, o => o.MapFrom(d => d.FullName));

            expression.CreateMap<SourceFolderModel, ListViewSourceModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Name))
                .ForMember(s => s.FullName, o => o.MapFrom(d => d.FullPath))
                .ForMember(s => s.SortOrder, o => o.MapFrom(d => d.SortOrder))
                .ForMember(s => s.Folders, o => o.MapFrom(d => d.Folders))
                .ForMember(s => s.ImageList, o => o.MapFrom(d => d.ImageList))
                .ReverseMap()
                .ForMember(s => s.FullPath, o => o.MapFrom(d => d.FullName));
        }
    }
}