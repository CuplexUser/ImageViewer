using AutoMapper;
using ImageViewer.Models;
using ImageViewer.Models.FormMenuHistory;
using ImageViewer.Models.Import;

namespace ImageViewer.Library.AutoMapperProfile
{
    public class ImageViewMapperProfile : Profile
    {
        public ImageViewMapperProfile()
        {
            ThumbnailMetadataDbModel.CreateMapping(this);
            ThumbnailEntryModel.CreateMapping(this);
            SourceFolderModel.CreateMapping(this);
            OutputDirectoryModel.CreateMapping(this);
            ImageRefModel.CreateMapping(this);
            RecentFileModel.CreateMapping(this);
            RecentFilesCollection.CreateMapping(this);
        }
    }
}