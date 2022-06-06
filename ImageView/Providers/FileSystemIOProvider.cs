using System.Security;
using System.Security.Cryptography;
using System.Text;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using Serilog;

namespace ImageViewer.Providers
{
    public class FileSystemIOProvider : ProviderBase
    {
        public Guid InstanceId { get; }
        private static readonly byte[] saltBytes = { 0xCB, 0x19, 0xBB, 0x84, 0xF2, 0xC3, 0x72, 0xD2, 0x72, 0xE1, 0x6E, 0x0B, 0xBF, 0x1E, 0x63, 0xB9, 0xF5, 0x7C, 0x6E, 0x10, 0x50, 0xDC, 0x83, 0x02, 0x45, 0x9F, 0x49, 0x3C, 0x3D, 0x5B, 0x75, 0x28 };

        public FileSystemIOProvider()
        {
            InstanceId = Guid.NewGuid();
        }

        [SecurityCritical]
        private static StorageManager CreateStorageManager(string password)
        {
            var settings = new StorageManagerSettings
            {
                NumberOfThreads = Environment.ProcessorCount,
                UseEncryption = true,
                UseMultithreading = true
            };
            settings.SetPassword(GetPassword(password));
            return new StorageManager(settings);
        }

        public ApplicationSettingsDataModel LoadApplicationSettings(string filename, string password)
        {
            var model = DeserializeObject<ApplicationSettingsDataModel>(filename, password);
            return model;
        }

        public bool SaveApplicationSettings(string filename, ApplicationSettingsDataModel appSettings, string password)
        {
            return SerializeObjectToFile(filename, appSettings, password);
        }

        public T DeserializeObject<T>(string filePath, string password) where T : class
        {
            try
            {
                var storageManager = CreateStorageManager(password);
                T config = storageManager.DeserializeObjectFromFile<T>(filePath, null);

                return config;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "T DeserializeObject<T> Exception");
                throw new CryptographicUnexpectedOperationException("Failed to Deserialize Object");
            }
        }

        public bool SerializeObjectToFile(string filePath, object model, string password)
        {
            try
            {
                var storageManager = CreateStorageManager(password);
                return storageManager.SerializeObjectToFile(model, filePath, null);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Exception thrown in the internal SaveConfig function");
                return false;
            }
        }

        [SecurityCritical]
        private static string GetPassword(string password)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(password);
            buffer = SHA512.Create().ComputeHash(buffer);

            var derivedBytes = new Rfc2898DeriveBytes(buffer, saltBytes, 2499, HashAlgorithmName.SHA512);
            buffer = derivedBytes.GetBytes(64);

            var generatedStr = GeneralConverters.ByteArrayToBase64(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }

            return generatedStr;

        }

        public override string ToString()
        {
            return $"File System IO Provider. Instance Id: {InstanceId}";
        }
    }
}