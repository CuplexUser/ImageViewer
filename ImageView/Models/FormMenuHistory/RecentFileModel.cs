using AutoMapper;
using ImageViewer.DataContracts;

namespace ImageViewer.Models.FormMenuHistory
{
    /// <summary>
    /// RecentFileModel
    /// </summary>
    public class RecentFileModel : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecentFileModel"/> class.
        /// </summary>
        public RecentFileModel()
        {
            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        public Guid Guid { get; private set; }

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
        /// Gets or sets the full path.
        /// </summary>
        /// <value>
        /// The full path.
        /// </value>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<RecentFileModel, RecentFileDataModel>()
                .ForMember(s => s.Name, o => o.MapFrom(d => d.Name))
                .ForMember(s => s.CreatedDate, o => o.MapFrom(d => d.CreatedDate))
                .ForMember(s => s.FullPath, o => o.MapFrom(d => d.FullPath))
                .ForMember(s => s.Guid, o => o.MapFrom(d => d.Guid))
                .ForMember(s => s.SortOrder, o => o.MapFrom(d => d.SortOrder))
                .ReverseMap();
        }

        public object Clone()
        {
            RecentFileModel model = new RecentFileModel
            {
                Name = Name,
                CreatedDate = CreatedDate,
                Guid = Guid,
                SortOrder = SortOrder,
                FullPath = FullPath
            };

            model.Guid = Guid;
            return model;
        }
    }
}