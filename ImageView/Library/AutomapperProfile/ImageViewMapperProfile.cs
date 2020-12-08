using System.Windows.Forms;
using AutoMapper;
using ImageViewer.DataContracts;
using ImageViewer.Models;

namespace ImageViewer.Library.AutoMapperProfile
{
    public class ImageViewMapperProfile : Profile
    {
        public ImageViewMapperProfile()
        {
            ThumbnailDatabase.CreateMapping(this);
            ThumbnailEntry.CreateMapping(this);
            ApplicationSettingsModel.CreateMapping(this);
            FormStateModel<Form>.CreateMapping(this);
        }
    }
}