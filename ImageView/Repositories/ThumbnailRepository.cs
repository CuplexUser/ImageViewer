﻿using System.Collections.Concurrent;
using System.Security;
using System.Security.Cryptography;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Models.Import;
using ImageViewer.Providers;

namespace ImageViewer.Repositories;

[UsedImplicitly]
public class ThumbnailRepository : RepositoryBase, IDisposable
{
    private const string DatabaseKeyComponent = "4565A56D-81B5-4FB7-B4D6-BB0A5B2CEB18.51893DE4-0FDB-4F9E-AA19-BBBD0FAE4D88.69537D29-60A2-4629-A2CF-0031826E1E59.5ECCD30C-C305-4742-9C99-AAB06548F311";

    private const string Salt = "46A973ED2C28A101CFB5E1986881A938E2DA0EA2458C1FBFA2AD2964D4973A9739DB2F90BB7014EE4DBF9F531FE5A7920A6D09B1D2E8DA4CD6AE973B7541ECBE";
    private const string Salt2 = "98037BE4E40DB3DD52081986E823CAACEC6011AE84F6EB20E34D856987314BFD840F82B8F27FDCDF0469030C527951F2D597C6088414D8ECBBA623C50D199425";
    private const string MetadataModelDbFileName = "thumbnail.db";
    private const string BinaryBlobFileName = "thumbnail.bin";

    private readonly FileManager _fileManager;
    private readonly ImageProvider _imageProvider;
    private readonly IMapper _mapper;
    private readonly StorageManager _storageManager;
    private readonly ConcurrentDictionary<string, ThumbnailEntryModel> _thumbnailDictionary;
    private bool _isModified;
    private ThumbnailMetadataDbModel _metadataDb;

    public ThumbnailRepository(IMapper mapper, FileManager fileManager, ImageProvider imageProvider)
    {
        _mapper = mapper;
        _fileManager = fileManager;
        _imageProvider = imageProvider;
        _thumbnailDictionary = new ConcurrentDictionary<string, ThumbnailEntryModel>();
        DbReaderWriterLock = new ReaderWriterLockSlim();
        _storageManager = CreateStorageManager();
    }

    public bool Initialized { get; private set; }

    [NotNull] private ReaderWriterLockSlim DbReaderWriterLock { get; }

    public bool IsModified
    {
        set => _isModified = value;
    }


    [SecuritySafeCritical]
    private void GetDatabaseKey(ref SecureString secureString)
    {
        byte[] saltBytes = GeneralConverters.HexStringToByteArray(Salt);


        using (var deriveBytes = new Rfc2898DeriveBytes(DatabaseKeyComponent, saltBytes, 5207, HashAlgorithmName.SHA512))
        {
            byte[] buffer = deriveBytes.GetBytes(512);


            try
            {
                secureString.Clear();
                buffer = SHA256.HashData(buffer);
                var buffer2 = new char[buffer.Length * 2];
                int size = Convert.ToBase64CharArray(buffer, 0, buffer.Length, buffer2, 0);

                for (var i = 0; i < size; i += 2) secureString.AppendChar(buffer2[i]);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetDatabaseKey exception, message: {Message}", ex.Message);
            }
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
        bool requireSave = false;

        try
        {
            if (File.Exists(filePath))
                _metadataDb = await _storageManager.DeserializeObjectFromFileAsync<ThumbnailMetadataDbModel>(filePath, null);
            else
            {
                _metadataDb = ThumbnailMetadataDbModel.CreateModel(blobDbPath);
                requireSave = true;
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex, "InitDatabase() Exception. Message: {message}", ex.Message);
        }

        if (_metadataDb == null)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            _metadataDb = ThumbnailMetadataDbModel.CreateModel(blobDbPath);
            requireSave = true;
        }

        if (requireSave)
            await SaveThumbnailDatabaseAsync();

        Initialized = true;
        return true;
    }

    public async Task<bool> SaveThumbnailDatabaseAsync()
    {
        try
        {
            string filePath = Path.Join(GlobalSettings.Instance.GetUserDataDirectoryPath(), MetadataModelDbFileName);

            // Save Data model
            var dataModel = _mapper.Map<ThumbnailMetadataDbDataModel>(_metadataDb);
            return await _storageManager.SerializeObjectToFileAsync(dataModel, filePath, null);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "SaveThumbnailDatabaseAsync exception. Message: {message}", ex.Message);
        }

        return false;
    }

    public Image GetOrCreateThumbnailImage(FileInfo file, Size size)
    {
        if (_thumbnailDictionary.ContainsKey(file.FullName))
        {
            var item = _thumbnailDictionary[file.FullName];
            if (file.LastWriteTime == item.OriginalImageModel.LastModified && file.Length == item.OriginalImageModel.FileSize)
            {
                // return cached thumbnail
                

            }


        }

        var image = _imageProvider.CreateThumbnail(file, size);

        // Add Thumbnail to cache
        AddThumbnailImgToCache(image, file);


        return image;
    }

    private void AddThumbnailImgToCache(Image image, FileInfo fileInfo)
    {
        if (!_thumbnailDictionary.ContainsKey(fileInfo.FullName))
        {
            var thumbModel = new ThumbnailEntryModel
            {
                Length = Convert.ToInt32(fileInfo.Length),
                CreateDate = DateTime.Now,
                ThumbnailSize = image.Size
            };

            var model = _mapper.Map<ImageRefModel>(fileInfo);
            thumbModel.OriginalImageModel = model;


            var rawImage = _fileManager.CreateRawImageFromImage(image);
            thumbModel.FilePosition = AddDataToBlobStorage(rawImage);
            thumbModel.Length = rawImage.ImageData.Length;

            if (!_thumbnailDictionary.TryAdd(fileInfo.FullName, thumbModel))
                Log.Warning("Failed to add thumbnail model to dictionary. fileName: {name}", fileInfo.FullName);
        }

    }

    private long AddDataToBlobStorage(RawImage rawImage)
    {
        return 1;


    }

    public async Task<bool> OptimizeDatabaseAsync()
    {
        // TODO
        return await Task<bool>.Factory.StartNew(() => false);

    }

    #region Public Methods

    public long GetDatabaseSize()
    {
        return GetThumbnailDiskSize();
    }

    public int GetFileCacheCount()
    {
        return _metadataDb.ThumbnailEntries.Count;
    }


    private long GetThumbnailDiskSize()
    {
        // Or you could get the file size of _metadataDb.BinaryBlobFilename

        return _metadataDb.ThumbnailEntries.Select(x => x.Length).Sum();
    }


    //public async Task<Image> GetOrCreateThumbnailImageAsync(string fullPath, Size size)
    //{
    //    Image img=null;
    //    if (_thumbnailDictionary.ContainsKey(fullPath))
    //    {
    //        ThumbnailEntryModel entry = _thumbnailDictionary[fullPath];
    //        if (entry.ThumbnailSize == size)
    //        {
    //            var data = await  ReadBinaryBlobDataAsync(entry.FilePosition, entry.Length);
    //            var ms = new MemoryStream(data);
    //            img = Image.FromStream(ms);
    //            return img;
    //        }
    //    }


    //    var result = CreateThumbnailEntry(fullPath, img);
    //    return result.Item2;
    //}


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

    public async Task<bool> SaveThumbnailDatabaseAsync()
    {
        try
        {
            if (DbReaderWriterLock.IsWriteLockHeld)
            {
                return false;
            }

            DbReaderWriterLock.EnterWriteLock();

            StorageManager storageManager = CreateStorageManager();
            string fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
            bool successful = await storageManager.SerializeObjectToFileAsync(_thumbnailDatabase, fileName, null);

            if (successful)
            {
                IsModified = false;
            }


            IsModified = false;
            return successful;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ThumbnailManager.SaveThumbnailDatabase() :{Message} ", ex.Message);
            return false;
        }
        finally
        {
            if (DbReaderWriterLock.IsWriteLockHeld)
            {
                DbReaderWriterLock.ExitWriteLock();
            }
        }
    }
    */

    #endregion

    #region IDisposable Support

    private bool hasBeenDisposed; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
        if (!hasBeenDisposed)
        {
            if (disposing) _thumbnailDictionary.Clear();


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
}