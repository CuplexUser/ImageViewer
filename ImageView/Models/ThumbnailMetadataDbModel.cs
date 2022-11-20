using AutoMapper;
using ImageViewer.DataContracts;


namespace ImageViewer.Models
{
    public class ThumbnailMetadataDbModel
    {
        public ThumbnailMetadataDbModel()
        {

        }

        public static ThumbnailMetadataDbModel CreateModel(string BinaryBlobFilePath)
        {
            var model= new ThumbnailMetadataDbModel
            {
                DatabaseId = Guid.NewGuid().ToString("D"),
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                ThumbnailEntries = new List<ThumbnailEntryModel>(),
                BinaryBlobFilename = BinaryBlobFilePath
            };

            return model;
        }

        public string DatabaseId { get; set; }

        public string BinaryBlobFilename { get; set; }

        public DateTime LastUpdated { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<ThumbnailEntryModel> ThumbnailEntries { get; private set; }

        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ThumbnailMetadataDbModel, ThumbnailMetadataDbDataModel>()
                .ForMember(d => d.DatabaseId, o => o.MapFrom(s => s.DatabaseId))
                .ForMember(d => d.LastUpdated, o => o.MapFrom(s => s.LastUpdated))
                .ForMember(d => d.CreatedDate, o => o.MapFrom(s => s.CreatedDate))
                .ForMember(d => d.BinaryBlobFilename, o => o.MapFrom(s => s.BinaryBlobFilename))
                .ForMember(d => d.ThumbnailEntries, o => o.MapFrom(s => s.ThumbnailEntries))
                .ReverseMap();
        }

        public override string ToString()
        {
            return $"{nameof(DatabaseId)}: {DatabaseId}, {nameof(BinaryBlobFilename)}: {BinaryBlobFilename}, {nameof(ThumbnailEntries)}: {ThumbnailEntries?.Count}";
        }
    }
}