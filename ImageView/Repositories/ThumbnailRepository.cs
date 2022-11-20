using AutoMapper;
using Castle.Components.DictionaryAdapter;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Managers;
using ImageViewer.Models;
using JetBrains.Annotations;
using Serilog;
using System.Collections.Concurrent;
using System.Security;
using System.Security.Cryptography;

namespace ImageViewer.Repositories
{
    /// <summary>
    /// </summary>
    /// <seealso cref="ImageViewer.Repositories.RepositoryBase" />
    /// <seealso cref="System.IDisposable" />
    [UsedImplicitly]
    public class ThumbnailRepository : RepositoryBase, IDisposable
    {
        /// <summary>
        ///     The database key
        /// </summary>
        private const string DatabaseKeyComponent = "47A52E85-C6529739-8BACCABE-C3077AF5-5384756F-5BB722E3-9CD9F3CB-DED32159-755BFC1A-96C07A00-8D9CFA2C-94858843-BE9A016C-DD35D4CB-23FB3EE9-E714D7F7";

        private const string Salt = "3B4AD4D2126B52E17212C49CA2280F9AB00237A7590EDF8D2B026DC3D21053416ABAFEDB7FA61B8190BB9F58C6A0AAC15D728CD71BE6E42980A618FFBF07C55F";
        private const string Salt2 = "5035D7563952191C1AB05A5F294EF4821D94E35BF6D0CFA73A1CC8941D163848AC45F71210D742CAF975F2D48F83EDC4DFBD3AB7657F2D65BE357013B22FBD95";

        private readonly FileManager _fileManager;


        /// <summary>
        ///     The mapper
        /// </summary>
        [NotNull] private readonly IMapper _mapper;

        /// <summary>
        ///     The is modified
        /// </summary>
        private bool _isModified;

        /// <summary>
        ///     The thumbnail database
        /// </summary>
        private ThumbnailMetadataDbModel _metadataDbModel;


        private ConcurrentQueue<BinaryDiskOperationModel> _binaryDiskOperationQueue;

        /// <summary>
        ///     The thumbnail dictionary
        /// </summary>
        private ConcurrentDictionary<string, ThumbnailEntryModel> _thumbnailDictionary;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ThumbnailRepository" /> class.
        /// </summary>
        /// <param name="fileManager">The file manager.</param>
        /// <param name="mapper">The mapper.</param>
        public ThumbnailRepository(IMapper mapper, FileManager fileManager)
        {
            _mapper = mapper;
            _fileManager = fileManager;
            _thumbnailDictionary = new ConcurrentDictionary<string, ThumbnailEntryModel>();
            DbReaderWriterLock = new ReaderWriterLockSlim();
            string fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
            _thumbnailDatabase = ThumbnailDatabase.Create(fileName);
        }

        [NotNull] private ReaderWriterLockSlim DbReaderWriterLock { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is modified.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is modified; otherwise, <c>false</c>.
        /// </value>
        public bool IsModified
        {
            get => _thumbnailDatabase.ThumbnailEntries.IsChanged | _isModified;
            set => _isModified = value;
        }

        #region Public Methods


        /// <summary>
        ///     Adds the thumbnail image.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="thumbnailImage">The thumbnail image.</param>
        /// <returns></returns>
        public Image AddThumbnailImage(string fullPath, Image thumbnailImage)
        {
            var entry = CreateThumbnailEntry(fullPath, thumbnailImage);
            if (entry == null)
                return null;

            AddThumbnailItem(entry.Item1);

            return entry.Item2;
        }

        /// <summary>
        ///     Adds the thumbnail item.
        /// </summary>
        /// <param name="thumbnailEntryModel">The thumbnail entryModel.</param>
        public void AddThumbnailItem(ThumbnailEntryModel thumbnailEntryModel)
        {
            if (!_thumbnailDictionary.ContainsKey(thumbnailEntryModel.FullPath))
            {
                _thumbnailDatabase.ThumbnailEntries.BeginEdit();
                _thumbnailDatabase.ThumbnailEntries.Add(thumbnailEntryModel);
                _thumbnailDatabase.ThumbnailEntries.EndEdit();

                while (!_thumbnailDictionary.ContainsKey(thumbnailEntryModel.FullPath) && !_thumbnailDictionary.TryAdd(thumbnailEntryModel.FullPath, thumbnailEntryModel)) Task.Delay(10);
            }
        }

        public long GetDatabaseSize()
        {
            return GetThumbnailDiskSize();
        }

        /// <summary>
        ///     Gets the file cache count.
        /// </summary>
        /// <returns></returns>
        public int GetFileCacheCount()
        {
            return _metadataDbModel.ThumbnailEntries.Count;
        }

        /// <summary>
        ///     Ges the thumbnail entryModel.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns></returns>
        public ThumbnailEntryDataModel GeThumbnailEntry(string fullPath)
        {
            ThumbnailEntryModel entryModel = GetThumbnailEntryFromCache(fullPath);
            ThumbnailEntryDataModel dataModel = _mapper.Map<ThumbnailEntryDataModel>(entryModel);

            return dataModel;
        }

        /// <summary>
        ///     Gets the size of the file cache.
        /// </summary>
        /// <returns></returns>
        private long GetThumbnailDiskSize()
        {
            return _metadataDbModel.ThumbnailEntries.Select(x => x.Length).Sum();
        }

        /// <summary>
        ///     Gets the thumbnail entryModel from cache.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <returns></returns>
        public ThumbnailEntryModel GetThumbnailEntryFromCache(string fullPath)
        {
            return _thumbnailDictionary.ContainsKey(fullPath) ? _thumbnailDictionary[fullPath] : null;
        }


        public async Task<Image> GetOrCreateThumbnailImageAsync(string fullPath, Size size)
        {
            if (_thumbnailDictionary.ContainsKey(fullPath))
            {
                var entry = _thumbnailDictionary[fullPath];
                if (entry.ThumbnailSize == size)
                {

                    var img = ReadBinaryBlobDataAsync(entry.BinaryStartPosition, entry.Length);

                }
            }

            
            var result = CreateThumbnailEntry(fullPath, img);
            return result.Item2;
        }

        private async Task<byte[]> ReadBinaryBlobDataAsync(long pos, int length)
        {
            



        }


        /// <summary>
        ///     Loads the thumbnail database.
        /// </summary>
        /// <returns></returns>
        public bool LoadThumbnailDatabase()
        {
            try
            {
                try
                {
                    var storageManager = CreateStorageManager();
                    string fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
                    ThumbnailDatabaseModel thumbnailDb = storageManager.DeserializeObjectFromFile<ThumbnailDatabaseModel>(fileName, null);
                    _thumbnailDatabase = _mapper.Map<ThumbnailDatabaseModel, ThumbnailDatabase>(thumbnailDb);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "LoadThumbnailDatabase failed");
                }

                if (_thumbnailDatabase == null)
                {
                    Log.Warning("LoadThumbnailDatabase failed, _thumbnailDatabase was null after deserialization and mapping.\n " +
                                "Creating New Thumbnail Database");

                    _thumbnailDatabase = ThumbnailDatabase.Create(ApplicationBuildConfig.UserDataPath);

                    return false;
                }

                if (_thumbnailDatabase.ThumbnailEntries == null) _thumbnailDatabase.ThumbnailEntries = new EditableList<ThumbnailEntryModel>();

                IsModified = false;

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception on LoadThumbnailDatabase");
                return false;
            }
        }

        /// <summary>
        ///     Optimizes the database.
        /// </summary>
        public async Task<bool> OptimizeDatabaseAsync()
        {
            try
            {
                var thumbnailsToRemove = new Queue<ThumbnailEntryModel>();
                _fileManager.ClearDirectoryAccessCache();
                foreach (var entry in _thumbnailDatabase.ThumbnailEntries)
                {
                    if (!_fileManager.HasAccessToDirectory(entry.Directory)) continue;
                    if (File.Exists(Path.Combine(entry.Directory, entry.FileName)) && FileManager.IsUpToDate(entry)) continue;
                    thumbnailsToRemove.Enqueue(entry);
                }


                while (thumbnailsToRemove.Count > 0)
                {
                    var entryModel = thumbnailsToRemove.Dequeue();
                    _thumbnailDatabase.ThumbnailEntries.Remove(entryModel);
                }

                //Remove possible duplicates due to data corruption.
                _thumbnailDatabase.ThumbnailEntries =
                    new EditableList<ThumbnailEntryModel>(_thumbnailDatabase.ThumbnailEntries.ToList());

                await _fileManager.RecreateDatabaseAsync(_thumbnailDatabase.ThumbnailEntries).ConfigureAwait(true);
                _thumbnailDictionary = new ConcurrentDictionary<string, ThumbnailEntryModel>(_thumbnailDatabase.ThumbnailEntries.ToDictionary(x => x.Directory + x.FileName, x => x));

                return await SaveThumbnailDatabaseAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Optimize thumbnail database error");
            }

            return false;
        }


        /// <summary>
        ///     Reduces the size of the cache.
        /// </summary>
        /// <param name="maxFileSize">Maximum size of the file.</param>
        /// <returns></returns>
        public async Task<bool> ReduceCacheSizeAsync(long maxFileSize)
        {
            try
            {
                if (DbReaderWriterLock.IsWriteLockHeld)
                {
                    return false;
                }

                DbReaderWriterLock.EnterWriteLock();

                // Update SourceImageLength is a new property and needs to be calculated post process when it is zero
                var filesToProcess = _thumbnailDictionary.Values.Where(x => x.Length == 0).Select(x => x.OriginalImageModel.CompletePath).ToList();
                var concurrentQueue = new ConcurrentQueue<ThumbnailEntryModel>();
                foreach (string fileName in filesToProcess)
                    if (!_thumbnailDictionary.TryRemove(fileName, out var entry))
                        concurrentQueue.Enqueue(entry);

                while (concurrentQueue.Count > 0)
                {
                    if (concurrentQueue.TryDequeue(out var thumbnailEntry))
                    {
                        if (!_thumbnailDictionary.TryRemove(thumbnailEntry.OriginalImageModel.CompletePath, out var entry))
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
                    var element = fileEntryList.FirstOrDefault();
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

                _thumbnailDatabase.ThumbnailEntries.Clear();
                _thumbnailDatabase.ThumbnailEntries.AddRange(_thumbnailDictionary.Values);

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

        public bool SaveThumbnailDatabase()
        {
            var tokenSource = new CancellationTokenSource(2000);
            var cancelToken = tokenSource.Token;
            var saveTask = Task.Factory.StartNew(SaveThumbnailDatabaseAsync, cancelToken);

            if (saveTask.IsCanceled)
            {
                Log.Warning("SaveThumbnailDatabaseAsync timed out after 2000 ms");
                return false;
            }

            return saveTask.Result.Result;
        }

        /// <summary>
        ///     Saves the thumbnail database asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveThumbnailDatabaseAsync()
        {
            try
            {
                if (DbReaderWriterLock.IsWriteLockHeld)
                {
                    return false;
                }

                DbReaderWriterLock.EnterWriteLock();

                var storageManager = CreateStorageManager();
                string fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
                bool successful = await storageManager.SerializeObjectToFileAsync(_thumbnailDatabase, fileName, null);

                if (successful) IsModified = false;

                // Also Save the raw db file

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

        /// <summary>
        ///     Validates the thumbnail database.
        /// </summary>
        /// <returns></returns>
        //public async Task<bool> ValidateThumbnailDatabaseAsync()
        //{
        //    bool updateToDisk = false;

        //    if (_thumbnailDatabase.ThumbnailEntries == null)
        //    {
        //        _thumbnailDatabase.ThumbnailEntries = new EditableList<ThumbnailEntryModel>();
        //        return true;
        //    }

        //    int itemsRemovedTotal = 0;

        //    //Check for duplicates
        //    var query = (from t in _thumbnailDatabase.ThumbnailEntries
        //                 group t by new { EntryFilePath = t.Directory + t.FileName }
        //        into g
        //                 select new { FilePath = g.Key, Count = g.Count() }).ToList();

        //    var duplicateKeys = query.Where(x => x.Count > 1).Select(x => x.FilePath.EntryFilePath).ToList();

        //    if (duplicateKeys.Count > 0) updateToDisk = true;

        //    foreach (string duplicateKey in duplicateKeys)
        //    {
        //        var removeItemsList = _thumbnailDatabase.ThumbnailEntries.Where(x => x.Directory + x.FileName == duplicateKey).ToList();

        //        itemsRemovedTotal += removeItemsList.Count - 1;
        //        removeItemsList.RemoveAt(0);
        //        foreach (var removableItem in removeItemsList)
        //        {
        //            _thumbnailDatabase.ThumbnailEntries.Remove(removableItem);
        //        }
        //    }

        //    if (itemsRemovedTotal > 0) Log.Information("Removed {itemsRemovedTotal} duplicate items from the thumbnail cache", itemsRemovedTotal);

        //    if (updateToDisk)
        //    {
        //        await SaveThumbnailDatabaseAsync().ConfigureAwait(true);
        //    }

        //    return true;
        //}

        #endregion

        /*
        [Obsolete]
        [SecuritySafeCritical]
        private void GetDatabaseKey(ref SecureString secureString)
        {
            if (SecurityContext.IsFlowSuppressed())
            {
                SecurityContext.RestoreFlow();
            }

            using (var context = SecurityContext.Capture())
            {
                var saltBytes = GeneralConverters.HexStringToByteArray(Salt);
                SecureString secure = new SecureString();
                ;

                SecurityContext.Run(context, new ContextCallback((object obj) =>
                {
                    using (var deriveBytes = new Rfc2898DeriveBytes(DatabaseKeyComponent, saltBytes, 5207, HashAlgorithmName.SHA256))
                    {
                        var buffer = deriveBytes.GetBytes(512);

                        foreach (char c in Convert.ToBase64String(buffer))
                        {
                            secure.AppendChar(c);
                        }
                    }
                }), saltBytes);


                secureString = secure.Copy();
                secure.Dispose();
            }
        }
        */

        [SecuritySafeCritical]
        private void GetDatabaseKey(ref SecureString secureString)
        {
            var saltBytes = GeneralConverters.HexStringToByteArray(Salt);


            using (var deriveBytes = new Rfc2898DeriveBytes(DatabaseKeyComponent, saltBytes, 5207, HashAlgorithmName.SHA512))
            {
                byte[] buffer = deriveBytes.GetBytes(512);


                try
                {
                    secureString.Clear();
                    buffer = SHA256.HashData(buffer);
                    char[] buffer2 = new char[buffer.Length * 2];
                    int size = Convert.ToBase64CharArray(buffer, 0, buffer.Length, buffer2, 0);

                    for (int i = 0; i < size; i += 2)
                    {
                        secureString.AppendChar(buffer2[i]);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "GetDatabaseKey exception, message: {Message}", ex.Message);
                }

            }

        }

        /// <summary>
        ///     Creates the storage manager.
        /// </summary>
        /// <returns></returns>
        private StorageManager CreateStorageManager()
        {
            StorageManager storageManager = null;
            var secureStr = new SecureString();
            GetDatabaseKey(ref secureStr);

            var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, secureStr.ToString());
            storageManager = new StorageManager(settings);


            return storageManager;
        }

        /// <summary>
        ///     Creates the thumbnail entryModel.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns></returns>
        private Tuple<ThumbnailEntryModel, Image> CreateThumbnailEntry(string fileName, Image thumbnail)
        {
            var entry = new ThumbnailEntryModel
            {
                FullPath = fileName,
                Directory = GeneralConverters.GetDirectoryNameFromPath(fileName, false)
            };

            var fi = new FileInfo(fileName);
            entry.Date = fi.LastWriteTime;
            entry.SourceImageLength = fi.Length;

            if (fi.Length == 0)
            {
                return null;
            }

            var image = thumbnail;
            var rawImage = _fileManager.CreateRawImage(image);
            _fileManager.WriteImage(rawImage);

            return new Tuple<ThumbnailEntryModel, Image>(entry, image);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _thumbnailDictionary.Clear();
                    _thumbnailDatabase?.ThumbnailEntries?.Clear();
                }

                _thumbnailDictionary = null;
                _thumbnailDatabase = null;

                disposedValue = true;
                GC.Collect(GC.GetGeneration(new WeakReference(this)), GCCollectionMode.Optimized);
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        public Image GetOrCreateThumbnailImage(string fullPath, Size size)
        {
            throw new NotImplementedException();
        }
    }
}