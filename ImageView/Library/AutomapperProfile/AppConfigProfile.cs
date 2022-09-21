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
                .ForMember(s => s.ShowImageViewFormsInTaskBar, o => o.MapFrom(d => d.ShowImageViewFormsInTaskBar))
                .ForMember(s => s.LastUsedSearchPaths, o => o.MapFrom(d => d.LastUsedSearchPaths))
                .ForMember(s => s.NextImageAnimation, o => o.MapFrom(d => d.NextImageAnimation))
                .ForMember(s => s.SlideshowInterval, o => o.MapFrom(d => d.SlideshowInterval))
                .ForMember(s => s.PrimaryImageSizeMode, o => o.MapFrom(d => d.PrimaryImageSizeMode))
                .ForMember(s => s.ScreenMinXOffset, o => o.MapFrom(d => d.ScreenMinXOffset))
                .ForMember(s => s.ScreenWidthOffset, o => o.MapFrom(d => d.ScreenWidthOffset))
                .ForMember(s => s.PasswordProtectBookmarks, o => o.MapFrom(d => d.PasswordProtectBookmarks))
                .ForMember(s => s.PasswordDerivedString, o => o.MapFrom(d => d.PasswordDerivedString))
                .ForMember(s => s.ShowSwitchImageButtons, o => o.MapFrom(d => d.ShowSwitchImageButtons))
                .ForMember(s => s.ThumbnailSize, o => o.MapFrom(d => d.ThumbnailSize))
                .ForMember(s => s.MaxThumbnails, o => o.MapFrom(d => d.MaxThumbnails))
                .ForMember(s => s.MainWindowBackgroundColor, o => o.MapFrom(d => d.MainWindowBackgroundColor))
                .ForMember(s => s.LastUpdateCheck, o => o.MapFrom(d => d.LastUpdateCheck))
                .ForMember(s => s.ToggleSlideshowWithThirdMouseButton, o => o.MapFrom(d => d.ToggleSlideshowWithThirdMouseButton))
                .ForMember(s => s.BookmarksShowOverlayWindow, o => o.MapFrom(d => d.BookmarksShowOverlayWindow))
                .ForMember(s => s.BookmarksShowMaximizedImageArea, o => o.MapFrom(d => d.BookmarksShowMaximizedImageArea))
                .ForMember(s => s.AppSettingsGuid, o => o.MapFrom(d => d.AppSettingsGuid))
                .ForMember(s => s.FormStateDataModels, o => o.MapFrom(d => d.FormStateModels.Values.ToList()))
                .ForMember(s => s.RecentFilesCollection, o => o.MapFrom(d => d.RecentFilesCollection))
                .ReverseMap()
                .ForMember(s => s.FormStateModels, o => o.MapFrom(d => d.FormStateDataModels.ToDictionary(x => x.FormName)));

            CreateMap<FormStateModel, FormStateDataModel>()
                .ForMember(s => s.FormName, o => o.MapFrom(d => d.FormName))
                .ForMember(s => s.FormPosition, o => o.MapFrom(d => d.FormPosition))
                .ForMember(s => s.FormSize, o => o.MapFrom(d => d.FormSize))
                .ForMember(s => s.AdditionalParameters, o => o.MapFrom(d => d.AdditionalParameters))
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