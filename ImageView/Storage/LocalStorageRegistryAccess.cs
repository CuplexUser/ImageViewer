using System;
using System.Collections.Generic;
using System.IO;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using GeneralToolkitLib.Storage.Registry;
using Serilog;

namespace ImageViewer.Storage
{
    public class LocalStorageRegistryAccess : FileBasedPropertyCollection, IRegistryAccess
    {
        private const string LocalStoragePassword = "!e#G[N8K-1?@c?]bh7QzhvQAh6pHbo-5m!RBGD6Z>DB6qOpv4@";
        private readonly string _localStorageFilePath;
        private Dictionary<string, object> _storageDictionary;
        private string ProductName { get; }
        private string CompanyName { get; }

        public LocalStorageRegistryAccess(string companyName, string productName)
        {
            CompanyName = companyName;
            ProductName = productName.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(companyName))
                throw new ArgumentException(@"Invalid argument. CompanyName cant be null or whitespace.", nameof(companyName));
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException(@"Invalid argument. productName cant be null or whitespace.", nameof(productName));

            SubKey = "SOFTWARE\\" + CompanyName.Trim() + "\\" + ProductName.Trim();
            _localStorageFilePath = Path.Combine(ApplicationBuildConfig.UserDataPath, "localRegStorage.dat");


            SecureLoadDatabase();
        }

        public bool ShowError { get; set; }
        public string SubKey { get; }

        private bool LoadLocalDatabase()
        {
            if (File.Exists(_localStorageFilePath))
            {
                try
                {
                    var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, LocalStoragePassword);
                    var storageManager = new StorageManager(settings);
                    _storageDictionary = storageManager.DeserializeObjectFromFile<Dictionary<string, object>>(_localStorageFilePath, null);
                    return true;

                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Exception when trying to deserialize LocalStorageRegistryAccess.dbfile");
                }
            }
            else
            {
                _storageDictionary = new Dictionary<string, object>();
            }

            
            return false;
        }

        private bool SaveLocalDatabase()
        {
            if (_storageDictionary == null || !IsDirty)
            {
                return false;
            }

            if (File.Exists(_localStorageFilePath))
            {
                File.Delete(_localStorageFilePath);
            }

            try
            {
                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, LocalStoragePassword);
                var storageManager = new StorageManager(settings);
                bool res = storageManager.SerializeObjectToFile(_storageDictionary, _localStorageFilePath, null);

                if (!res)
                {
                    Log.Warning("Failed to serialize and save local reg dbfile");
                }

                return res;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception when trying to deserialize LocalStorageRegistryAccess.dbfile");
            }

            return false;
        }

        public void SetupSubKeyPathAndAccessRights()
        {
        }

        public T ReadObjectFromRegistry<T>() where T : new()
        {
            string key = typeof(T).Name;

            if (_storageDictionary.ContainsKey(key))
            {
                object localObjInDictionary = _storageDictionary[key];
                return CreateTemplateObjFromLocalObject<T>(localObjInDictionary);
            }

            return new T();
        }

        public bool TryReadObjectFromRegistry<T>(out T objFromRegistry) where T : new()
        {
            try
            {
                objFromRegistry = ReadObjectFromRegistry<T>();
                return objFromRegistry != null;
            }
            catch
            {
                objFromRegistry = default(T);
                return false;
            }
        }

        private T CreateTemplateObjFromLocalObject<T>(object objToConvert) where T : new()
        {
            try
            {
                return (T)Convert.ChangeType(objToConvert, typeof(T));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LocalStorageRegistryAccess.CreateTemplateObjFromLocalObject threw an exception");
            }

            return new T();
        }


        public void SaveObjectToRegistry(object objToSave)
        {
            string key = objToSave.GetType().Name;

            if (_storageDictionary.ContainsKey(key))
            {
                _storageDictionary[key] = objToSave;
            }
            else
            {
                _storageDictionary.Add(key, objToSave);
            }

            ModelUpdated();
        }

        /// <summary>
        /// Deletes the key.  Only for internal use
        /// </summary>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        public bool DeleteKey(string keyName)
        {
            if (_storageDictionary.ContainsKey(keyName))
            {
                _storageDictionary.Remove(keyName);
                ModelUpdated();
                return true;
            }

            return false;
        }

        /// <summary>
        /// To delete a sub key and any child.
        /// input: void
        /// output: true or false
        /// </summary>
        /// <returns></returns>
        public bool DeleteSubKeyTree()
        {
            return true;
        }

        /// <summary>
        /// Retrive the count of subkeys at the current key.
        /// input: void
        /// output: number of subkeys
        /// </summary>
        /// <returns></returns>
        public int SubKeyCount()
        {
            return 0;
        }

        /// <summary>
        /// Retrive the count of values in the key.
        /// input: void
        /// output: number of keys
        /// </summary>
        /// <returns></returns>
        public int ValueCount()
        {
            return _storageDictionary.Count;
        }

        protected override bool LoadFromFileDatabase()
        {
            return LoadLocalDatabase();
        }

        protected override bool SaveToFileDatabase()
        {
            return SaveLocalDatabase();
        }
    }
}
