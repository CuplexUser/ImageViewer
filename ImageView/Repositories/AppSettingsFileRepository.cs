using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AutoMapper;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Models;
using ImageViewer.Storage;
using ImageViewer.Utility;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ImageViewer.Repositories.RepositoryBase" />
    [UsedImplicitly]
    public sealed class AppSettingsFileRepository : RepositoryBase
    {
        /// <summary>
        /// The application settings filename
        /// </summary>
        private const string AppSettingsFilename = "localSettings.dat";
        /// <summary>
        /// The application settings password
        /// </summary>
        private const string AppSettingsPassword = "kTntQYxg4eQSkDiJnev/qoO9Dm9MrpGKbp7WQ/QfaEeP9j48HtvZ8pji/YXQ2ejx";

        /// <summary>
        /// The data file locked
        /// </summary>
        private bool _dataFileLocked;

        /// <summary>
        /// Occurs when [load settings completed].
        /// </summary>
        public event EventHandler LoadSettingsCompleted;
        /// <summary>
        /// Occurs when [save settings completed].
        /// </summary>
        public event EventHandler SaveSettingsCompleted;

        /// <summary>
        /// The application settings
        /// </summary>
        private ApplicationSettingsModel _appSettings;
        /// <summary>
        /// The CMP settings
        /// </summary>
        private ApplicationSettingsModel _cmpSettings;
        /// <summary>
        /// The file access wait handle
        /// </summary>
        private readonly ManualResetEvent _fileAccessWaitHandle;
        /// <summary>
        /// The is dirty
        /// </summary>
        private bool _isDirty;

        private readonly IMapper _mapper;


        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <value>
        /// The application settings.
        /// </value>
        public ApplicationSettingsModel AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    LoadSettings();
                }
                _isDirty = EvaluateIsDirty();
                return _appSettings;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsFileRepository"/> class.
        /// </summary>
        public AppSettingsFileRepository(IMapper mapper)
        {
            _mapper = mapper;
            _fileAccessWaitHandle = new ManualResetEvent(false);
            _fileAccessWaitHandle.Set();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get => _isDirty;
            private set => _isDirty = value;
        }

        /// <summary>
        /// Notifies the settings changed.
        /// </summary>
        public void NotifySettingsChanged()
        {
            IsDirty = true;
        }

        /// <summary>
        /// Evaluates the is dirty.
        /// </summary>
        /// <returns></returns>
        private bool EvaluateIsDirty()
        {
            if (_cmpSettings == null)
            {
                return true;
            }

            return _appSettings.Equals(_cmpSettings);

        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <returns></returns>
        public bool SaveSettings()
        {
            if (!IsDirty)
            {
                return false;
            }

            // Thread will continue without waiting if the file is not locked
            _fileAccessWaitHandle.WaitOne(TimeSpan.FromSeconds(5));
            if (_dataFileLocked)
                return false;

            return SaveSettingsInternal();
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public bool LoadSettings()
        {
            bool result = LoadSettingsInternal();

            ValidateAndModifyInvalidSettings();

            return result;
        }

        /// <summary>
        /// Validates the and modify invalid settings.
        /// </summary>
        private void ValidateAndModifyInvalidSettings()
        {
            //RangeProperty range = _appSettings.AutoHideCursorDelay.GetType().GetProperty("AutoHideCursorDelay") as RangeProperty(AutoHideCursorDelay;



            if (true)
            {

            }
        }

        /// <summary>
        /// Saves the settings internal.
        /// </summary>
        /// <returns></returns>
        private bool SaveSettingsInternal()
        {
            if (_dataFileLocked)
            {
                return false;
            }

            bool result = false;
            _dataFileLocked = true;
            _fileAccessWaitHandle.Reset();

            if (_appSettings == null)
            {
                return false;
            }

            try
            {
                string path = GetFullPathToSettingsFile();

                var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SecurityHelper.GetSecurePassword(AppSettingsPassword)));
                ApplicationSettingsDataModel applicationSettingsData = _mapper.Map<ApplicationSettingsDataModel>(_appSettings);
                result = storageManager.SerializeObjectToFile(applicationSettingsData, path, null);
                if (result)
                {
                    IsDirty = false;
                    OnSaveSettingsCompleted();
                }

            }
            catch (Exception exception)
            {
                Log.Error(exception, "AppSettingsFileRepository SaveSettings Exception: {Message}", exception.Message);
                _dataFileLocked = false;
            }
            finally
            {

                _dataFileLocked = false;
                _fileAccessWaitHandle.Set();
            }

            return result;
        }

        /// <summary>
        /// Saves the new settings.
        /// </summary>
        private void SaveNewSettings()
        {
            try
            {
                string path = GetFullPathToSettingsFile();

                _mapper.ConfigurationProvider.AssertConfigurationIsValid();

                var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SecurityHelper.GetSecurePassword(AppSettingsPassword)));
                var applicationSettingsData = _mapper.Map<ApplicationSettingsDataModel>(_appSettings);
                storageManager.SerializeObjectToFile(applicationSettingsData, path, null);
                IsDirty = false;
                OnSaveSettingsCompleted();
            }
            catch (Exception exception)
            {
                Log.Error(exception, "AppSettingsFileRepository SaveSettings Exception: {Message}", exception.Message);
            }
        }

        /// <summary>
        /// Loads the settings internal.
        /// </summary>
        /// <returns></returns>
        private bool LoadSettingsInternal()
        {
            if (_dataFileLocked)
            {
                return false;
            }

            _dataFileLocked = true;
            _fileAccessWaitHandle.Reset();
            bool result;

            try
            {
                string path = GetFullPathToSettingsFile();
                if (!File.Exists(path))
                {
                    _appSettings = ApplicationSettingsModel.CreateDefaultSettings();
                    SaveNewSettings();
                }
                else
                {
                    while (_appSettings == null)
                    {
                        try
                        {

                            var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SecurityHelper.GetSecurePassword(AppSettingsPassword)));
                            _appSettings = storageManager.DeserializeObjectFromFile<ApplicationSettingsModel>(path, null) ?? ApplicationSettingsModel.CreateDefaultSettings();

                            if (_appSettings.FormStateDictionary == null)
                            {
                                _appSettings.InitFormStateDictionary();
                            }
                        }
                        catch
                        {
                            File.Delete(path);
                            _appSettings = ApplicationSettingsModel.CreateDefaultSettings();
                            SaveNewSettings();
                        }
                    }

                }

                IsDirty = false;
                result = true;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Exception in AppSettingsFileRepository LoadSettingsAsync {Message}", exception.Message);
                result = false;
            }
            finally
            {
                _dataFileLocked = false;
                ValidateLoadedSettings();
                _fileAccessWaitHandle.Set();
                OnLoadSettingsCompleted();
            }

            return result;
        }

        /// <summary>
        /// Validates the loaded settings.
        /// </summary>
        private void ValidateLoadedSettings()
        {
            if (_appSettings.LastUsedSearchPaths == null)
            {
                _appSettings.LastUsedSearchPaths = new List<string>();
            }

            if (_appSettings.LastFolderLocation == null)
            {
                _appSettings.LastFolderLocation = "";
            }
        }


        /// <summary>
        /// Gets the full path to settings file.
        /// </summary>
        /// <returns></returns>
        private string GetFullPathToSettingsFile()
        {
            return Path.Combine(ApplicationBuildConfig.UserDataPath, AppSettingsFilename);
        }

        /// <summary>
        /// Called when [load settings completed].
        /// </summary>
        private void OnLoadSettingsCompleted()
        {
            LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
            _cmpSettings = _appSettings;
        }

        /// <summary>
        /// Called when [save settings completed].
        /// </summary>
        private void OnSaveSettingsCompleted()
        {
            SaveSettingsCompleted?.Invoke(this, EventArgs.Empty);
            _cmpSettings = _appSettings;
        }
    }
}