using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GeneralToolkitLib.Storage.Registry;
using ImageViewer.DataContracts;
using ImageViewer.Events;
using ImageViewer.Interfaces;
using ImageViewer.Library.EventHandlers;
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
        private readonly AppSettingsFileRepository _fileRepository;

        public string CompanyName { get; } = Application.CompanyName;

        public string ProductName { get; } = Application.ProductName;


        private ImageViewApplicationSettings _applicationSettings;
        private RegistryAppSettings _registryAppSettings;


        public ApplicationSettingsService(AppSettingsFileRepository appSettingsFileRepository, IRegistryAccess registryAccess)
        {
            _registryRepository = registryAccess;
            _fileRepository = appSettingsFileRepository;

            try
            {
                bool result = _fileRepository.LoadSettings();
                if (!result)
                {
                    _fileRepository = new AppSettingsFileRepository();
                    _fileRepository.SaveSettings();
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

            _fileRepository.LoadSettingsCompleted += _appSettingsFileRepository_LoadSettingsCompleted;
        }


        // Unit tests
        public static ApplicationSettingsService CreateService(AppSettingsFileRepository appSettingsFileRepository)
        {
            return new ApplicationSettingsService(appSettingsFileRepository, new LocalStorageRegistryAccess(Application.CompanyName, Application.ProductName));
        }

        public void SetSettingsStateModified()
        {
            _fileRepository.NotifySettingsChanged();
        }

        public ImageViewApplicationSettings Settings
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
            if (_applicationSettings != null && !_fileRepository.IsDirty)
                return true;

            return _fileRepository.LoadSettings();
        }


        private void _appSettingsFileRepository_LoadSettingsCompleted(object sender, EventArgs e)
        {
            if (_fileRepository.AppSettings != null)
            {
                _applicationSettings = _fileRepository.AppSettings;
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
                result = _fileRepository.SaveSettings();
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

        public void UpdateOrInsertFormState(FormSizeAndPositionModel formState)
        {
            if (_fileRepository.AppSettings.ExtendedAppSettings.FormStateDictionary == null)
            {
                _fileRepository.AppSettings.ExtendedAppSettings.InitFormDictionary();
            }

            if (_fileRepository.AppSettings.ExtendedAppSettings.FormStateDictionary.ContainsKey(formState.FormType))
            {
                _fileRepository.AppSettings.ExtendedAppSettings.FormStateDictionary[formState.FormType] = formState;
            }
            else
            {
                _fileRepository.AppSettings.ExtendedAppSettings.FormStateDictionary.Add(formState.FormType, formState);
            }
        }

        public void RegisterFormStateOnClose(Form form)
        {
            string formName = form.GetType().Name;
            var fileDbAppSettings = _fileRepository.AppSettings;
            bool existingForm = fileDbAppSettings.ExtendedAppSettings.FormStateDictionary.Any(x => x.Key == formName);
            if (existingForm)
            {
                fileDbAppSettings.ExtendedAppSettings.FormStateDictionary[formName] = RestoreFormState.GetFormState(form);
            }
            else
            {
                var formState = RestoreFormState.GetFormState(form);
                fileDbAppSettings.ExtendedAppSettings.FormStateDictionary.Add(formName, formState);
            }

            _fileRepository.NotifySettingsChanged();
            _fileRepository.SaveSettings();
        }
    }
}