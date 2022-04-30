using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using Serilog;

namespace ImageViewer.Providers
{
    public class FileSystemIOProvider : ProviderBase
    {
        private readonly StorageManager _storageManager;

        public Guid InstanceId { get; }

        public FileSystemIOProvider()
        {
            InstanceId = Guid.NewGuid();
            var settings = new StorageManagerSettings();

            _storageManager = new StorageManager(settings);
        }

        public ApplicationSettingsDataModel LoadApplicationSettings(string filename)
        {
            var model = LoadConfig<ApplicationSettingsDataModel>(filename);
            return model;
        }

        public bool SaveApplicationSettings(string filename, ApplicationSettingsDataModel appSettings)
        {
            return SaveConfig(filename, appSettings);
        }

        private T LoadConfig<T>(string filePath) where T : class
        {
            try
            {
                T config = _storageManager.DeserializeObjectFromFile<T>(filePath, null);

                return config;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "T LoadConfig<T> Exception");
                return default(T);
            }
        }

        private bool SaveConfig(string filePath, object model)
        {
            try
            {
                return _storageManager.SerializeObjectToFile(model, filePath, null);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Exception thrown in the internal SaveConfig function.");
                return false;
            }
        }

        public override string ToString()
        {
            return $"File System IO Provider. Instance Id: {InstanceId}";
        }
    }
}