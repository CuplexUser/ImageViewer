using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Castle.Components.DictionaryAdapter;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Storage;
using ImageViewer.Utility;
using JetBrains.Annotations;
using Serilog;
using SHA256 = GeneralToolkitLib.Hashing.SHA256;

namespace ImageViewer.Repositories
{
    [UsedImplicitly]
    public sealed class AppSettingsFileRepository : RepositoryBase
    {
        private const string AppSettingsFilename = "localSettings.dat";
        private const string AppSettingsPassword = "kTntQYxg4eQSkDiJnev/qoO9Dm9MrpGKbp7WQ/QfaEeP9j48HtvZ8pji/YXQ2ejx";

        private bool _dataFileLocked;

        public event EventHandler LoadSettingsCompleted;
        public event EventHandler SaveSettingsCompleted;

        private ImageViewApplicationSettings _appSettings;
        private ImageViewApplicationSettings _cmpSettings;
        private readonly ManualResetEvent _fileAccessWaitHandle;
        private bool _isDirty;


        public ImageViewApplicationSettings AppSettings
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

        public AppSettingsFileRepository()
        {
            _fileAccessWaitHandle = new ManualResetEvent(false);
            _fileAccessWaitHandle.Set();
        }

        public bool IsDirty
        {
            get => _isDirty;
            private set => _isDirty = value;
        }

        public void NotifySettingsChanged()
        {
            IsDirty = true;
        }

        private bool EvaluateIsDirty()
        {
            if (_cmpSettings == null)
            {
                return true;
            }

            DataContractComparer<ImageViewApplicationSettings> compareSettings = new DataContractComparer<ImageViewApplicationSettings>(_cmpSettings);
            return compareSettings.Equals(_appSettings);
        }

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

        public bool LoadSettings()
        {
            // Thread will continue without waiting if the file is not locked
            _fileAccessWaitHandle.WaitOne(TimeSpan.FromSeconds(7.5));
            if (_dataFileLocked)
                return false;

            return LoadSettingsInternal();

        }

        private bool SaveSettingsInternal()
        {
            if (_dataFileLocked)
            {
                return false;
            }

            bool result = false;
            _dataFileLocked = true;
            _fileAccessWaitHandle.Reset();

            try
            {
                string path = GetFullPathToSettingsFile();

                var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SecurityHelper.GetSecurePassword(AppSettingsPassword)));
                result = storageManager.SerializeObjectToFile(_appSettings, path, null);
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

        private void SaveNewSettings()
        {
            try
            {
                string path = GetFullPathToSettingsFile();

                var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SecurityHelper.GetSecurePassword(AppSettingsPassword)));
                storageManager.SerializeObjectToFile(_appSettings, path, null);
                IsDirty = false;
                OnSaveSettingsCompleted();
            }
            catch (Exception exception)
            {
                Log.Error(exception, "AppSettingsFileRepository SaveSettings Exception: {Message}", exception.Message);
            }
        }
               
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
                    _appSettings = ImageViewApplicationSettings.CreateDefaultSettings();
                    SaveNewSettings();
                }
                else
                {
                    while (_appSettings == null)
                    {
                        try
                        {

                            var storageManager = new StorageManager(new StorageManagerSettings(false, 1, true, SecurityHelper.GetSecurePassword(AppSettingsPassword)));
                            _appSettings = storageManager.DeserializeObjectFromFile<ImageViewApplicationSettings>(path, null);

                            if (_appSettings.ExtendedAppSettings.FormStateDictionary == null)
                            {
                                _appSettings.ExtendedAppSettings.InitFormDictionary();
                            }
                        }
                        catch
                        {
                            File.Delete(path);
                            _appSettings = ImageViewApplicationSettings.CreateDefaultSettings();
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

        private void ValidateLoadedSettings()
        {
            if (_appSettings.LastUsedSearchPaths == null)
            {
                _appSettings.LastUsedSearchPaths= new List<string>();
            }

            if (_appSettings.LastFolderLocation == null)
            {
                _appSettings.LastFolderLocation = "";
            }
        }


        private string GetFullPathToSettingsFile()
        {
            return Path.Combine(ApplicationBuildConfig.UserDataPath, AppSettingsFilename);
        }

        private void OnLoadSettingsCompleted()
        {
            LoadSettingsCompleted?.Invoke(this, EventArgs.Empty);
            _cmpSettings = _appSettings;
        }

        private void OnSaveSettingsCompleted()
        {
            SaveSettingsCompleted?.Invoke(this, EventArgs.Empty);
            _cmpSettings = _appSettings;
        }
    }
}