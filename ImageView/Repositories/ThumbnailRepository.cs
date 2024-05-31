using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Models.Import;
using ImageViewer.Providers;
using Microsoft.VisualBasic.Devices;
using System.Security;
using System.Security.Cryptography;

namespace ImageViewer.Repositories;

[UsedImplicitly]
public class ThumbnailRepository : RepositoryBase, IDisposable
{
    private const string DatabaseKeyComponent =
        "4565A56D-81B5-4FB7-B4D6-BB0A5B2CEB18.51893DE4-0FDB-4F9E-AA19-BBBD0FAE4D88.69537D29-60A2-4629-A2CF-0031826E1E59.5ECCD30C-C305-4742-9C99-AAB06548F311";

    private const string Salt = "46A973ED2C28A101CFB5E1986881A938E2DA0EA2458C1FBFA2AD2964D4973A9739DB2F90BB7014EE4DBF9F531FE5A7920A6D09B1D2E8DA4CD6AE973B7541ECBE";
    private const string MetadataModelDbFileName = "thumbnail.db";
    private const string BinaryBlobFileName = "thumbnail.bin";
    private readonly BlobStorageProvider _blobStorageProvider;

    private readonly FileManager _fileManager;
    private readonly ImageProvider _imageProvider;
    private readonly IMapper _mapper;
    private readonly StorageManager _storageManager;
    private readonly Size thumbnailSize = new(512, 512);
    private readonly ReaderWriterLock _lock = new ReaderWriterLock();

    //private readonly ConcurrentDictionary<string, ThumbnailEntryModel> _thumbnailDictionary;
    private ThumbnailMetadataDbModel _metadataDb;

    public ThumbnailRepository(IMapper mapper, FileManager fileManager, ImageProvider imageProvider, BlobStorageProvider blobStorageProvider)
    {
        _mapper = mapper;
        _fileManager = fileManager;
        _imageProvider = imageProvider;
        _blobStorageProvider = blobStorageProvider;
        _storageManager = CreateStorageManager();
    }

    public bool Initialized { get; private set; }
    
    public bool IsModified
    {
        get;
        private set;
    }


    [SecuritySafeCritical]
    private static void GetDatabaseKey(ref SecureString secureString)
    {
        byte[] saltBytes = GeneralConverters.HexStringToByteArray(Salt);


        using var deriveBytes = new Rfc2898DeriveBytes(DatabaseKeyComponent, saltBytes, 5207, HashAlgorithmName.SHA512);
        byte[] buffer = deriveBytes.GetBytes(512);

        try
        {
            secureString.Clear();
            buffer = SHA256.HashData(buffer);
            var buffer2 = new char[buffer.Length * 2];
            int size = Convert.ToBase64CharArray(buffer, 0, buffer.Length, buffer2, 0);

            for (var i = 0; i < size; i += 2)
                secureString.AppendChar(buffer2[i]);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "GetDatabaseKey exception, message: {Message}", ex.Message);
        }
    }

    private StorageManager CreateStorageManager()
    {
        var secureStr = new SecureString();
        GetDatabaseKey(ref secureStr);

        var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, secureStr.ToString());
        var storageManager = new StorageManager(settings);


        return storageManager;
    }

    public async Task<bool> InitDatabase()
    {
        string blobDbPath = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), BinaryBlobFileName);
        string filePath = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), MetadataModelDbFileName);
        var requireSave = false;
        _blobStorageProvider.OpenStorageFile();

        try
        {
            if (File.Exists(filePath))
            {
                var dataModel = await _storageManager.DeserializeObjectFromFileAsync<ThumbnailMetadataDbDataModel>(filePath, null);
                _metadataDb = _mapper.Map<ThumbnailMetadataDbModel>(dataModel);
            }
            else
            {
                _metadataDb = ThumbnailMetadataDbModel.CreateModel(blobDbPath);
                requireSave = true;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "InitDatabase() Exception. Message: {message}", ex.Message);
            _metadataDb = null;
        }

        if (_metadataDb == null)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            if (!_blobStorageProvider.ClearStorage()) throw new InvalidOperationException("BlobStorageProvider.ClearStorage() Failed!");


            _metadataDb = ThumbnailMetadataDbModel.CreateModel(blobDbPath);
            requireSave = true;
        }

        if (requireSave) return await SaveThumbnailDatabaseAsync();

        Initialized = true;
        IsModified= true;
        return true;
    }

    public async Task<bool> SaveThumbnailDatabaseAsync()
    {
        try
        {
            string userDataDirectory = GlobalSettings.Instance.GetUserDataDirectoryPath();
            string metadataFilePath = Path.Join(userDataDirectory, MetadataModelDbFileName);

            var metadataDataModel = _mapper.Map<ThumbnailMetadataDbDataModel>(_metadataDb);

            bool saveResult = await _storageManager.SerializeObjectToFileAsync(metadataDataModel, metadataFilePath, null);
            bool blobSaveResult = await _blobStorageProvider.SaveFileToDiskAsync();

            if (saveResult && blobSaveResult)
                IsModified = false;

            return saveResult && blobSaveResult;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to save thumbnail database. Message: {message}", ex.Message);
            return false;
        }
    }

    public async Task<Image> GetOrCreateThumbnailImage(FileInfo file, Size size)
    {
        if (_metadataDb.ThumbnailEntries.TryGetValue(file.FullName, out var item))
            if (file.LastWriteTime == item!.OriginalImageModel.LastModified && file.Length == item.OriginalImageModel.FileSize)
                try
                {
                    // return cached thumbnail
                    byte[] imgBytes = await _blobStorageProvider.ReadBlobDataAsync(item.FilePosition, item.FileSize).ConfigureAwait(true);
                    var image = _imageProvider.RestoreImageFromCache(imgBytes);

                    if (image == null)
                    {
                        // TODO deal with failure
                        bool result = _metadataDb.ThumbnailEntries.TryRemove(file.FullName, out _);
                    }
                    else
                    {
                        return image;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "GetOrCreateThumbnailImage exception. Message: {message}", ex.Message);
                }

        var createdImage = _imageProvider.CreateThumbnail(file, size);

        // Add Thumbnail to cache
        await AddThumbnailImgToCacheAsync(createdImage, file);


        return createdImage;
    }

    private async Task AddThumbnailImgToCacheAsync(Image image, FileInfo fileInfo)
    {
        if (_metadataDb.ThumbnailEntries.ContainsKey(fileInfo.FullName))
        {
            var thumbModel = new ThumbnailEntryModel
            {
                FileSize = Convert.ToInt32(fileInfo.Length),
                CreateDate = DateTime.Now,
                ThumbnailSize = image.Size
            };

            var model = _mapper.Map<ImageRefModel>(fileInfo);
            thumbModel.OriginalImageModel = model;


            var rawImage = _fileManager.CreateRawImageFromImage(image);
            thumbModel.FilePosition = await _blobStorageProvider.WriteBlobDataAsync(rawImage.ImageData);
            thumbModel.FileSize = rawImage.ImageData.Length;
            thumbModel.FullName = fileInfo.FullName;

            if (_metadataDb.ThumbnailEntries.TryAdd(thumbModel.FullName, thumbModel))
                IsModified = true;
        }
    }

    private void AddDataToBlobStorage(RawImage rawImage)
    {
        _blobStorageProvider.WriteBlobData(rawImage.ImageData);
    }

    #region Public Methods

    // Just reorder the database items according to Size Ascending. Which will also defrag the database
    // since unused data blocks in the blob data storage will be removed automatically.
    public bool OptimizeDatabase()
    {
        _lock.AcquireWriterLock(-1);
        if (!_lock.IsWriterLockHeld)
            return false;

        try
        {
            var thumbnailList = _metadataDb.ThumbnailEntries.Values.ToList().OrderBy(x => x.FileSize);
            _blobStorageProvider.SaveFileToDiskAsync().GetAwaiter().GetResult();

            IsModified = true;
            return true;
        }
        finally
        {
            _lock.ReleaseWriterLock();
        }   
        
       
    }

    public long GetDatabaseSize()
    {
        return GetThumbnailDiskSize();
    }

    public int GetFileCacheCount()
    {
        return _metadataDb.ThumbnailEntries.Count;
    }

    public bool ClearDatabase()
    {
        bool result = _blobStorageProvider.ClearStorage();
        _metadataDb.ThumbnailEntries.Clear();
        return result;
    }

    private long GetThumbnailDiskSize()
    {
        // Or you could get the file size of _metadataDb.BinaryBlobFilename

        return _blobStorageProvider.GetFileSize();
    }


    public async Task<Image> GetOrCreateThumbnailImageAsync(string fullPath, Size size, CancellationToken token)
    {
        Image img = null;
        if (_metadataDb.ThumbnailEntries.TryGetValue(fullPath, out var entry))
        {
            if (entry!.ThumbnailSize == size)
            {
                byte[] data = await _blobStorageProvider.ReadBlobDataAsync(entry.FilePosition, entry.FileSize);
                var ms = new MemoryStream(data);
                img = Image.FromStream(ms);
                return img;
            }
        }
        else
        {
            try
            {
                var fi = new FileInfo(fullPath);
                byte[] data = _imageProvider.CreateThumbnailToByteArray(fi, thumbnailSize);

                var imageRef = _mapper.Map<ImageRefModel>(fi);

                var model = new ThumbnailEntryModel { FileSize = Convert.ToInt32(data.Length), CreateDate = DateTime.Now, ThumbnailSize = size, OriginalImageModel = imageRef };
                int position = await _blobStorageProvider.WriteBlobDataAsync(data);
                model.FilePosition = position;
                model.FullName = fullPath;

                if (!_metadataDb.ThumbnailEntries.TryAdd(model.FullName, model)) Log.Warning("Failed to add ThumbnailEntryModel to _metadataDb.ThumbnailEntries in GetOrCreateThumbnailImageAsync()");

                img = _imageProvider.LoadFromByteArray(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetOrCreateThumbnailImageAsync Exception, Path: {path}", fullPath);
            }
        }

        return img;
    }

    public string GetThumbnailDbFilePath()
    {
        return Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), MetadataModelDbFileName);
    }

    public bool CheckIfCached(string imagePath)
    {
        return _metadataDb.ThumbnailEntries.ContainsKey(imagePath);
    }


    /*
    public async Task<bool> ReduceCacheSizeAsync(long maxFileSize)
    {
        try
        {
            if (DbReaderWriterLock.IsWriteLockHeld)
            {
                return false;
            }

            DbReaderWriterLock.EnterWriteLock();

            var filesToProcess = _thumbnailDictionary.Values.Where(x => x.Length == 0).Select(x => x.OriginalImageModel.CompletePath).ToList();
            var concurrentQueue = new ConcurrentQueue<ThumbnailEntryModel>();
            foreach (string fileName in filesToProcess)
            {
                if (!_thumbnailDictionary.TryRemove(fileName, out ThumbnailEntryModel entry))
                {
                    concurrentQueue.Enqueue(entry);
                }
            }

            while (concurrentQueue.Count > 0)
            {
                if (concurrentQueue.TryDequeue(out ThumbnailEntryModel thumbnailEntry))
                {
                    if (!_thumbnailDictionary.TryRemove(thumbnailEntry.OriginalImageModel.CompletePath, out ThumbnailEntryModel entry))
                    {
                        concurrentQueue.Enqueue(thumbnailEntry);
                    }
                    else
                    {
                        await Task.Delay(1);
                    }
                }
            }

            var fileEntryList = _thumbnailDictionary.Values.OrderBy(x => x.Length).ToList();
            long currentSize = fileEntryList.Sum(x => x.Length);

            while (currentSize > maxFileSize)
            {
                ThumbnailEntryModel element = fileEntryList.FirstOrDefault();
                if (element != null)
                {
                    fileEntryList.Remove(element);
                    currentSize -= element.Length;
                }
                else
                {
                    break;
                }
            }

            _thumbnailDictionary = new ConcurrentDictionary<string, ThumbnailEntryModel>(fileEntryList.ToDictionary(x => x.OriginalImageModel.CompletePath, x => x));

            //_thumbnailDatabase.ThumbnailEntries.Clear();
            //_thumbnailDatabase.ThumbnailEntries.AddRange(_thumbnailDictionary.Values);

            await _fileManager.RecreateDatabaseAsync(_thumbnailDictionary.Values.ToList());
            await SaveThumbnailDatabaseAsync();
            IsModified = true;
            return true;
        }
        catch (Exception e)
        {
            Log.Error(e, "ReduceCacheSizeAsync failed using {maxFileSize}", maxFileSize);
        }
        finally
        {
            if (DbReaderWriterLock.IsWriteLockHeld)
            {
                DbReaderWriterLock.ExitWriteLock();
            }
        }

        return false;
    }
    */

    #endregion

    #region IDisposable Support

    private bool hasBeenDisposed; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!hasBeenDisposed)
        {
            hasBeenDisposed = true;
            GC.Collect(GC.GetGeneration(new WeakReference(this)), GCCollectionMode.Optimized);
        }
    }


    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    #endregion


    public bool DecreaseDatabaseSize(int maxThumbnailCount)
    {
        throw new NotImplementedException();
    }
}