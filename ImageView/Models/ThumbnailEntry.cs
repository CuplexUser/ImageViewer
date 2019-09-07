using System;
using AutoMapper;
using ImageViewer.DataContracts;
using Guid = System.Guid;

namespace ImageViewer.Models
{
    /// <summary>
    /// ThumbnailEntry
    /// </summary>
    public class ThumbnailEntry
    {
        public ThumbnailEntry()
        {
            if (UniqueId == Guid.Empty)
            {
                UniqueId = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FullPath { get; set; }


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
        /// Gets or sets the file position.
        /// </summary>
        /// <value>
        /// The file position.
        /// </value>
        public long FilePosition { get; set; }


        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; set; }


        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }


        /// <summary>
        /// Gets or sets the source image date.
        /// </summary>
        /// <value>
        /// The source image date.
        /// </value>
        public DateTime SourceImageDate { get; set; }


        /// <summary>
        /// Gets or sets the length of the source image.
        /// </summary>
        /// <value>
        /// The length of the source image.
        /// </value>
        public long SourceImageLength { get; set; }


        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid UniqueId { get; protected set; }

        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ThumbnailEntry, ThumbnailEntryModel>().ReverseMap();
        }
    }
}