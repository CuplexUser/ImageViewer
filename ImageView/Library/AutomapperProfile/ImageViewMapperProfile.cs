using AutoMapper;
using ImageViewer.Models;
using ImageViewer.Models.Import;

namespace ImageViewer.Library.AutoMapperProfile
{
    public class ImageViewMapperProfile : Profile
    {
        public ImageViewMapperProfile()
        {
            ThumbnailDatabase.CreateMapping(this);
            ThumbnailEntry.CreateMapping(this);
            SourceFolderModel.CreateMapping(this);
            OutputDirectoryModel.CreateMapping(this);
            ImageRefModel.CreateMapping(this);
        }
    }
}