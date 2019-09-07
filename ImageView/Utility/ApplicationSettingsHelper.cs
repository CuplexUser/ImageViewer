using System;
using ImageViewer.DataContracts;

namespace ImageViewer.Utility
{
    public static class ApplicationSettingsHelper
    {
        public static AppSettingsExtendedDataModel Create()
        {

            var appSettings = new AppSettingsExtendedDataModel
            {
                AppSettingsUUID = Guid.NewGuid(),
                BookmarksShowMaximizedImageArea = false,
                BookmarksShowOverlayWindow = false
            };
            appSettings.InitFormDictionary();

            return appSettings;
        }
    }
}