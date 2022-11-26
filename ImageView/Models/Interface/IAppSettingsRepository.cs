namespace ImageViewer.Models.Interface
{
    public interface IAppSettingsRepository
    {
        ApplicationSettingsModel LoadSettings();

        bool SaveSettings(ApplicationSettingsModel settings);
    }
}