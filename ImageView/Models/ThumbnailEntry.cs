using System;
using AutoMapper;
using ImageViewer.DataContracts;

namespace ImageViewer.Models
{
    /// <summary>
    /// ThumbnailEntry
    /// </summary>
    public class ThumbnailEntry
    {
        private Guid _uniqueId = Guid.Empty;

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
        public Guid UniqueId
        {
            get
            {

                if (_uniqueId == Guid.Empty)
                {
                    UniqueId = Guid.NewGuid();
                }

                return _uniqueId;

            }
            private set => _uniqueId = value;
        }

        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ThumbnailEntry, ThumbnailEntryModel>()
                .ForMember(d => d.FullPath, o => o.MapFrom(s => s.FullPath))
                .ForMember(d => d.FileName, o => o.MapFrom(s => s.FileName))
                .ForMember(d => d.Directory, o => o.MapFrom(s => s.Directory))
                .ForMember(d => d.FilePosition, o => o.MapFrom(s => s.FilePosition))
                .ForMember(d => d.Length, o => o.MapFrom(s => s.Length))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
                .ForMember(d => d.SourceImageDate, o => o.MapFrom(s => s.SourceImageDate))
                .ForMember(d => d.SourceImageLength, o => o.MapFrom(s => s.SourceImageLength))
                .ForMember(d => d.UniqueId, o => o.MapFrom(s => s.UniqueId)).ReverseMap();
        }
    }
}