using System;
using AutoMapper;
using ImageViewer.DataContracts;
using ImageViewer.DataContracts.Import;
using ImageViewer.Utility;

namespace ImageViewer.Models.Import
{
    /// <summary>
    /// ImageRefModel
    /// </summary>
    public class ImageRefModel
    {
        /// <summary>
        /// Gets or sets the parent folder identifier.
        /// </summary>
        /// <value>
        /// The parent folder identifier.
        /// </value>
        public string ParentFolderId { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public string Directory { get; set; }
        /// <summary>
        /// Gets or sets the complete path.
        /// </summary>
        /// <value>
        /// The complete path.
        /// </value>
        public string CompletePath { get; set; }
        /// <summary>
        /// Gets or sets the type of the image.
        /// </summary>
        /// <value>
        /// The type of the image.
        /// </value>
        public string ImageType { get; set; }
        /// <summary>
        /// Gets or sets the last modifies.
        /// </summary>
        /// <value>
        /// The last modifies.
        /// </value>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public long FileSize { get; set; }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public int SortOrder { get; set; }


        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ImageRefModel, ImageReference>()
                .ForMember(s => s.Size, o => o.MapFrom(d => d.FileSize))
                .ForMember(s => s.CompletePath, o => o.MapFrom(d => d.CompletePath))
                .ForMember(s => s.FileName, o => o.MapFrom(d => d.FileName))
                .ForMember(s => s.LastWriteTime, o => o.MapFrom(d => d.LastModified))
                .ForMember(s => s.CreationTime, o => o.MapFrom(d => d.CreationTime))
                .ForMember(s => s.Directory, o => o.MapFrom(d => d.Directory))
                .ReverseMap()
                .ForMember(s => s.ImageType, o => o.MapFrom(d => SystemIOHelper.GetFileExtention(d.FileName)))
                .ForMember(s => s.SortOrder, o => o.MapFrom((reference, model, arg3) => arg3));
        }
    }
}