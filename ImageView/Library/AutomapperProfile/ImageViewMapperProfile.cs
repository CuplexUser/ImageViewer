using AutoMapper;
using ImageViewer.Models;

namespace ImageViewer.Library.AutoMapperProfile
{
    public class ImageViewMapperProfile : Profile
    {
        public ImageViewMapperProfile()
        {
            ThumbnailDatabase.CreateMapping(this);
            ThumbnailEntry.CreateMapping(this);
        }
    }
}