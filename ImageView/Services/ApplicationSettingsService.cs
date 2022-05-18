using ImageViewer.Models;
using ImageViewer.Repositories;
using ImageViewer.Utility;
using Serilog;


namespace ImageViewer.Services
{
    public class ApplicationSettingsService : ServiceBase
    {
        private readonly AppSettingsRepository _appSettingsRepository;
        private ApplicationSettingsModel _applicationSettings;
        private const long MIN_CACHE_SIZE = 1024 * 1024 * 32;
        private const long MAX_CACHE_SIZE = 1024 * 1024 * 1024;

        public ApplicationSettingsService(AppSettingsRepository appSettingsRepository)
        {
            _appSettingsRepository = appSettingsRepository;

            try
            {
                _applicationSettings = _appSettingsRepository.LoadSettings();
                if (_applicationSettings == null)
                {
                    _applicationSettings = AppSettingsRepository.GetDefaultApplicationSettings();
                    _appSettingsRepository.SaveSettings(_applicationSettings);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Fatal error encountered when accessing the registry settings");
                throw new IOException("Application Settings could not be loaded and could not be set to default and saved");
            }

            _appSettingsRepository.LoadSettingsCompleted += _appSettingsFileRepository_LoadSettingsCompleted;
        }

        public void SetSettingsStateModified()
        {
        }

        public ApplicationSettingsModel Settings => _applicationSettings ?? (_applicationSettings = LoadLocalStorageSettings());


        public event EventHandler OnSettingsLoaded;
        public event EventHandler OnSettingsSaved;


        public bool LoadSettings()
        {
            bool loadedSuccessively = false;

            try
            {
                _applicationSettings = _appSettingsRepository.LoadSettings();
                ValidateSettings();
                OnSettingsLoaded?.Invoke(this, EventArgs.Empty);
                loadedSuccessively = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ErrorLoading AppSettings");
            }

            return loadedSuccessively;
        }

        private ApplicationSettingsModel LoadLocalStorageSettings()
        {
            LoadSettings();
            return _applicationSettings;
        }

        private void ValidateSettings()
        {
            var defSettings = AppSettingsRepository.GetDefaultApplicationSettings();
            ModelValidator validator = new ModelValidator(_applicationSettings);
            if (!validator.ValidateModel())
            {
                Log.Warning("Loaded application settings are invalid. {ErrorMessage}", validator.ValidationResults.First().ErrorMessage);
                if (_applicationSettings.ImageCacheSize < MIN_CACHE_SIZE || _applicationSettings.ImageCacheSize > MAX_CACHE_SIZE)
                {
                    _applicationSettings.ImageCacheSize = defSettings.ImageCacheSize;
                    Log.Debug("ImageCacheSize was invalid. Value changed to: {ImageCacheSize}", _applicationSettings.ImageCacheSize);
                }

                if (_applicationSettings.AutoHideCursorDelay < 100)
                {
                    _applicationSettings.AutoHideCursorDelay = defSettings.AutoHideCursorDelay;
                    Log.Debug("AutoHideCursorDelay was invalid. Value changed to: {AutoHideCursorDelay}", _applicationSettings.AutoHideCursorDelay);
                }

                if (_applicationSettings.SlideshowInterval < 1000)
                {
                    _applicationSettings.SlideshowInterval = defSettings.SlideshowInterval;
                }

                //SaveSettings();
            }
        }

        private void _appSettingsFileRepository_LoadSettingsCompleted(object sender, EventArgs e)
        {
            if (_applicationSettings != null)
            {
                _applicationSettings = new ApplicationSettingsModel();
            }
        }

        public static ModelValidator CreateModelValidator(ApplicationSettingsModel model)
        {
            var modelValidator = new ModelValidator(model);
            return modelValidator;
        }

        public bool SaveSettings()
        {
            bool result;

            if (_applicationSettings == null)
            {
                throw new InvalidOperationException("Cant save uninitialized Null settings");
            }

            try
            {
                result = _appSettingsRepository.SaveSettings(_applicationSettings);
                if (result)
                    OnSettingsSaved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SaveSettings threw en exception on");
                result = false;
            }

            return result;
        }
    }
}