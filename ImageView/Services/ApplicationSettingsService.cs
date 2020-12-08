using System;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using GeneralToolkitLib.Storage.Registry;
using ImageViewer.Collections;
using ImageViewer.Events;
using ImageViewer.Models;
using ImageViewer.Repositories;
using ImageViewer.Storage;
using ImageViewer.Utility;
using JetBrains.Annotations;
using Serilog;


namespace ImageViewer.Services
{
    [UsedImplicitly]
    public sealed class ApplicationSettingsService : ServiceBase
    {
        private readonly IRegistryAccess _registryRepository;
        private readonly AppSettingsFileRepository _appSettingsFileRepository;

        public string CompanyName { get; } = Application.CompanyName;

        public string ProductName { get; } = Application.ProductName;
        private ApplicationSettingsModel _applicationSettings;
        private RegistryAppSettings _registryAppSettings;
        private readonly ILifetimeScope _scope;

        public ApplicationSettingsService(AppSettingsFileRepository appSettingsAppSettingsFileRepository, IRegistryAccess registryAccess, VolatileSettingsCollection volatileSettings, ILifetimeScope scope)
        {
            _registryRepository = registryAccess;
            SessionStorage = volatileSettings;
            _scope = scope;

            _appSettingsFileRepository = appSettingsAppSettingsFileRepository;
            _applicationSettings=  ApplicationSettingsModel.CreateDefaultSettings();

            try
            {
                bool result = _appSettingsFileRepository.LoadSettings();
                if (!result)
                {
                    _appSettingsFileRepository.SaveSettings();
                }

                result = result & _registryRepository.TryReadObjectFromRegistry(out _registryAppSettings);
                if (!result || _registryAppSettings == null)
                {
                    _registryAppSettings = RegistryAppSettings.CreateNew(ProductName, CompanyName);
                    _registryRepository.SaveObjectToRegistry(_registryAppSettings);
                }

            }
            catch (Exception ex)
            {

                Log.Error(ex, "Fatal error encountered when accessing the registry settings");
                _registryAppSettings = RegistryAppSettings.CreateNew(ProductName, CompanyName);
                //MessageBox.Show(ex.Message, Resources.Fatal_error_encountered_when_accessing_the_registry_settings_please_restart_, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            _appSettingsFileRepository.LoadSettingsCompleted += AppSettingsAppSettingsFileRepositoryLoadSettingsCompleted;
        }


        public VolatileSettingsCollection SessionStorage { get; }

        public void SetSettingsStateModified()
        {
            _appSettingsFileRepository.NotifySettingsChanged();
        }

        public ApplicationSettingsModel Settings
        {
            get
            {
                while (_applicationSettings == null)
                {
                    if (!LoadLocalStorageSettings())
                        throw new InvalidOperationException();

                    LoadLocalStorageSettings();
                }

                return _applicationSettings;
            }
            private set => _applicationSettings = value;
        }

        public RegistryAppSettings AppSettingsInRegistry
        {
            get
            {
                if (_registryAppSettings == null)
                {
                    if (!LoadRegistrySettings())
                        throw new InvalidOperationException();
                }
                return _registryAppSettings;
            }
        }

        public event EventHandler OnSettingsLoaded;
        public event EventHandler OnSettingsSaved;
        public event AccessExceptionEvent OnRegistryAccessDenied;


        public bool LoadSettings()
        {
            bool loadedSuccessively = false;

            try
            {
                LoadRegistrySettings();
                LoadLocalStorageSettings();
                OnSettingsLoaded?.Invoke(this, EventArgs.Empty);
                loadedSuccessively = true;
            }
            catch (Exception ex)
            {
                OnRegistryAccessDenied?.Invoke(this, new AccessExceptionEventArgs(ex));
                Log.Error(ex, "ErrorLoading AppSettings");
            }

            return loadedSuccessively;
        }

        private bool LoadRegistrySettings()
        {
            _registryAppSettings = _registryRepository.ReadObjectFromRegistry<RegistryAppSettings>();
            return _registryAppSettings != null;
        }

        private bool LoadLocalStorageSettings()
        {
            if (_applicationSettings != null && !_appSettingsFileRepository.IsDirty)
                return true;

            return _appSettingsFileRepository.LoadSettings();
        }


        private void AppSettingsAppSettingsFileRepositoryLoadSettingsCompleted(object sender, EventArgs e)
        {
            if (_appSettingsFileRepository.AppSettings != null)
            {
                _applicationSettings = _appSettingsFileRepository.AppSettings;
            }

        }
        public bool SaveSettings()
        {
            bool result = true;

            if (_applicationSettings == null)
            {
                throw new InvalidOperationException("Cant save uninitialized Null settings");
            }

            if (_registryRepository is LocalStorageRegistryAccess registryAccessStorage)
            {
                result = registryAccessStorage.SecureSaveDatabaseToFile();

            }

            try
            {
                result = _appSettingsFileRepository.SaveSettings();
                if (!result)
                {
                    return false;
                }
                Settings.RemoveDuplicateEntriesWithIgnoreCase();
                _registryRepository.SaveObjectToRegistry(Settings);

                OnSettingsSaved?.Invoke(this, new EventArgs());

            }
            catch (Exception ex)
            {
                Log.Error(ex, "SaveSettings threw en exception on");
                return false;
            }

            return result;
        }

        public void SaveFormState(FormStateModel<Form> formState)
        {
            if (_appSettingsFileRepository.AppSettings.FormStateDictionary == null)
            {
                _appSettingsFileRepository.AppSettings.InitFormStateDictionary();
            }

            if (_appSettingsFileRepository.AppSettings.FormStateDictionary.ContainsKey(formState.GetType().Name))
            {
                _appSettingsFileRepository.AppSettings.FormStateDictionary[formState.GetType().Name] = formState;
            }
            else
            {
                _appSettingsFileRepository.AppSettings.FormStateDictionary.Add(formState.GetType().Name, formState);
            }
        }

        public void RegisterFormStateOnClose(Form form)
        {
            string formName = form.GetType().Name;
            var fileDbAppSettings = _appSettingsFileRepository.AppSettings;
            bool existingForm = fileDbAppSettings.FormStateDictionary.Any(x => x.Key == formName);

            form.SaveFormState(this);
            
            _appSettingsFileRepository.NotifySettingsChanged();
            _appSettingsFileRepository.SaveSettings();
        }

        internal void SaveFormSettings(Type type, FormMain form)
        {
            
        }

        public bool ToggleFullscreen()
        {
            return false;
        }
    }
}