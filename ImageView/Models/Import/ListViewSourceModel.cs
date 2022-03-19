using System;
using System.Collections.Generic;
using AutoMapper;
using ImageViewer.DataContracts.Import;

namespace ImageViewer.Models.Import
{
    /// <summary>
    /// ListViewSourceModel
    /// </summary>
    public class ListViewSourceModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

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
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the folders.
        /// </summary>
        /// <value>
        /// The folders.
        /// </value>
        public List<ListViewSourceModel> Folders { get; set; }

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
        /// Creates the mapping.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ListViewSourceModel, SourceFolderDataModel>()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.FullName, o => o.MapFrom(d => d.FullName))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Name))
                .ForMember(s => s.Folders, o => o.MapFrom(d => d.Folders))
                .ReverseMap()
                .ForMember(s => s.Id, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Name))
                .ForMember(s => s.FullName, o => o.MapFrom(d => d.FullName))
                .ForMember(s => s.Folders, o => o.MapFrom(d => d.Folders));
        }
    }
}