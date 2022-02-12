using System.Drawing;
using System.Linq;
using AutoMapper;
using ImageViewer.DataContracts;
using ImageViewer.Models;

namespace ImageViewer.Library.AutoMapperProfile
{
    public class AppConfigProfile : Profile
    {
        public AppConfigProfile()
        {
            CreateMap<ApplicationSettingsModel, ApplicationSettingsDataModel>()
                .ForMember(s => s.AlwaysOntop, o => o.MapFrom(d => d.AlwaysOntop))
                .ForMember(s => s.AutoHideCursor, o => o.MapFrom(d => d.AutoHideCursor))
                .ForMember(s => s.AutoHideCursorDelay, o => o.MapFrom(d => d.AutoHideCursorDelay))
                .ForMember(s => s.AutoRandomizeCollection, o => o.MapFrom(d => d.AutoRandomizeCollection))
                .ForMember(s => s.AutomaticUpdateCheck, o => o.MapFrom(d => d.AutomaticUpdateCheck))
                .ForMember(s => s.ConfirmApplicationShutdown, o => o.MapFrom(d => d.ConfirmApplicationShutdown))
                .ForMember(s => s.DefaultKey, o => o.MapFrom(d => d.DefaultKey))
                .ForMember(s => s.EnableAutoLoadFunctionFromMenu, o => o.MapFrom(d => d.EnableAutoLoadFunctionFromMenu))
                .ForMember(s => s.ImageCacheSize, o => o.MapFrom(d => d.ImageCacheSize))
                .ForMember(s => s.ImageTransitionTime, o => o.MapFrom(d => d.ImageTransitionTime))
                .ForMember(s => s.LastFolderLocation, o => o.MapFrom(d => d.LastFolderLocation))
                .ForMember(s => s.ShowNextPrevControlsOnEnterWindow, o => o.MapFrom(d => d.ShowNextPrevControlsOnEnterWindow))
                .ForMember(s => s.FormStateDataModels, o => o.MapFrom(d => d.FormStateModels.Values.ToList()))
                .ReverseMap()

                .ForMember(s => s.FormStateModels, o => o.MapFrom(d => d.FormStateDataModels.ToDictionary(x => x.FormName)));

            CreateMap<FormStateModel, FormStateDataModel>()
                .ForMember(s => s.FormName, o => o.MapFrom(d => d.FormName))
                .ForMember(s => s.FormPosition, o => o.MapFrom(d => d.FormPosition))
                .ForMember(s => s.FormSize, o => o.MapFrom(d => d.FormSize))
                .ForMember(s => s.WindowsState, o => o.MapFrom(d => d.WindowState))
                .ReverseMap();


            CreateMap<PointDataModel, Point>()
                .ForMember(s => s.X, o => o.MapFrom(d => d.X))
                .ForMember(s => s.Y, o => o.MapFrom(d => d.Y))
                .ReverseMap()
                .ForMember(s => s.X, o => o.MapFrom(d => d.X))
                .ForMember(s => s.Y, o => o.MapFrom(d => d.Y));

            CreateMap<VectorDataModel, Size>()
                .ForMember(s => s.Height, o => o.MapFrom(d => d.Height))
                .ForMember(s => s.Width, o => o.MapFrom(d => d.Width))
                .ReverseMap()
                .ForMember(s => s.Height, o => o.MapFrom(d => d.Height))
                .ForMember(s => s.Width, o => o.MapFrom(d => d.Width));

            CreateMap<ColorDataModel, Color>()
                .ConstructUsing(x => x.ToColor())
                .ReverseMap()
                .ConvertUsing(x => ColorDataModel.CreateFromColor(x));
        }
    }
}