using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageProcessor;
using ImageViewer.DataContracts;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Utility;
using JetBrains.Annotations;
using MoreLinq.Extensions;
using Serilog;

namespace ImageViewer.Repositories
{
    [UsedImplicitly]
    public class ThumbnailRepository : RepositoryBase, IDisposable
    {
        private const string DatabaseFilename = "thumbs.db";
        private const string DatabaseKey = "2C1D350D-B0E5-4181-8D60-CAE050132DC1";
        private readonly FileManager _fileManager;
        private readonly IMapper _mapper;
        private bool _isModified;
        private ThumbnailDatabase _thumbnailDatabase;
        private ConcurrentDictionary<string, ThumbnailEntry> _thumbnailDictionary;

        public ThumbnailRepository(FileManager fileManager, IMapper mapper)
        {
            _fileManager = fileManager;
            _mapper = mapper;
            _thumbnailDictionary = new ConcurrentDictionary<string, ThumbnailEntry>();
        }

        public bool IsModified
        {
            get => _thumbnailDatabase.ThumbnailEntries.IsChanged | _isModified;
            set => _isModified = value;
        }

        public void Dispose()
        {
            _fileManager?.Dispose();
        }

        public bool LoadThumbnailDatabase()
        {
            try
            {
                var storageManager = CreateStorageManager();
                try
                {
                    string fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
                    ThumbnailDatabaseModel thumbnailDb =  storageManager.DeserializeObjectFromFile<ThumbnailDatabaseModel>(fileName, null);
                    _thumbnailDatabase = _mapper.Map<ThumbnailDatabaseModel, ThumbnailDatabase>(thumbnailDb);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "LoadThumbnailDatabase failed");
                }


                if (_thumbnailDatabase == null)
                {
                    _thumbnailDatabase = new ThumbnailDatabase
                    {
                        DatabaseId = Guid.NewGuid().ToString(),
                        LastUpdated = DateTime.Now,
                        DataStoragePath = ApplicationBuildConfig.UserDataPath,
                        ThumbnailEntries = new EditableList<ThumbnailEntry>()
                    };

                    return false;
                }

                if (_thumbnailDatabase.ThumbnailEntries == null) _thumbnailDatabase.ThumbnailEntries = new EditableList<ThumbnailEntry>();

                IsModified = false;

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception on LoadThumbnailDatabase");
                return false;
            }
        }

        public bool SaveThumbnailDatabase()
        {
            try
            {
                var storageManager = CreateStorageManager();
                string fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseFilename);
                bool successful = storageManager.SerializeObjectToFile(_thumbnailDatabase, fileName, null);

                if (successful) IsModified = false;

                // Also Save the raw db file
                _fileManager.WriteToDisk();

                IsModified = false;
                return successful;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ThumbnailManager.SaveThumbnailDatabase() : " + ex.Message, ex);
                return false;
            }
        }

        public bool IsCached(string fullPath)
        {
            return _thumbnailDictionary.ContainsKey(fullPath);
        }

        public ThumbnailEntry GetThumbnailEntryFromCache(string fullPath)
        {
            return _thumbnailDictionary.ContainsKey(fullPath) ? _thumbnailDictionary[fullPath] : null;
        }

        public long GetFileCacheSize()
        {
            return _thumbnailDatabase.ThumbnailEntries.Select(x => x.Length).Sum();
        }

        public bool ValidateThumbnailDatabase()
        {
            bool updateToDisk = false;

            if (_thumbnailDatabase.ThumbnailEntries == null)
            {
                _thumbnailDatabase.ThumbnailEntries = new EditableList<ThumbnailEntry>();
                return true;
            }

            int itemsRemovedTotal = 0;

            //Check for duplicates
            var query = (from t in _thumbnailDatabase.ThumbnailEntries
                         group t by new { EntryFilePath = t.Directory + t.FileName }
                into g
                         select new { FilePath = g.Key, Count = g.Count() }).ToList();

            var duplicateKeys = query.Where(x => x.Count > 1).Select(x => x.FilePath.EntryFilePath).ToList();

            if (duplicateKeys.Count > 0) updateToDisk = true;

            foreach (string duplicateKey in duplicateKeys)
            {
                var removeItemsList = _thumbnailDatabase.ThumbnailEntries.Where(x => x.Directory + x.FileName == duplicateKey).ToList();

                itemsRemovedTotal += removeItemsList.Count - 1;
                removeItemsList.RemoveAt(0);
                foreach (var removableItem in removeItemsList) _thumbnailDatabase.ThumbnailEntries.Remove(removableItem);
            }

            if (itemsRemovedTotal > 0) Log.Information("Removed {itemsRemovedTotal} duplicate items from the thumbnail cache", itemsRemovedTotal);


            if (updateToDisk) SaveThumbnailDatabase();

            return true;
        }

        private StorageManager CreateStorageManager()
        {
            var settings = new StorageManagerSettings(true, Environment.ProcessorCount, true, DatabaseKey);
            var storageManager = new StorageManager(settings);
            return storageManager;
        }



        public Image GetThumbnailImage(string fullPath)
        {
            if (IsCached(fullPath))
            {
                return _fileManager.ReadImageFromDatabase(_thumbnailDictionary[fullPath]);
            }
            else
            {
                Image img = Image.FromFile(fullPath);
                var result = CreateThumbnailEntry(fullPath, img);
                return result.Item2;
            }
        }

        public ThumbnailEntryModel GeThumbnailEntry(string fullPath)
        {
            ThumbnailEntry entry = GetThumbnailEntryFromCache(fullPath);
            ThumbnailEntryModel model = _mapper.Map<ThumbnailEntryModel>(entry);

            return model;
        }

        private Tuple<ThumbnailEntry, Image> CreateThumbnailEntry(string fileName, Image thumbnail)
        {
            var entry = new ThumbnailEntry
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

            return new Tuple<ThumbnailEntry, Image>(entry, image);
        }

        public void OptimizeDatabase()
        {
            var thumbnailsToRemove = new Queue<ThumbnailEntry>();
            _fileManager.ClearDirectoryAccessCache();
            foreach (var entry in _thumbnailDatabase.ThumbnailEntries)
            {
                if (!_fileManager.HasAccessToDirectory(entry.Directory)) continue;
                if (File.Exists(Path.Combine(entry.Directory, entry.FileName)) && FileManager.IsUpToDate(entry)) continue;
                thumbnailsToRemove.Enqueue(entry);
            }

            if (_fileManager.LockDatabase())
            {
                while (thumbnailsToRemove.Count > 0)
                {
                    var entryModel = thumbnailsToRemove.Dequeue();
                    _thumbnailDatabase.ThumbnailEntries.Remove(entryModel);
                }

                //Remove possible duplicates due to data corruption.
                _thumbnailDatabase.ThumbnailEntries = new EditableList<ThumbnailEntry>(_thumbnailDatabase.ThumbnailEntries.DistinctBy(x => x.FullPath).AsEnumerable());

                _fileManager.RecreateDatabase(_thumbnailDatabase.ThumbnailEntries);
                _thumbnailDictionary = new ConcurrentDictionary<string, ThumbnailEntry>(_thumbnailDatabase.ThumbnailEntries.ToDictionary(x => x.Directory + x.FileName, x => x));
                _fileManager.UnlockDatabase();
                SaveThumbnailDatabase();
            }
        }

        public int GetFileCacheCount()
        {
            return _thumbnailDatabase.ThumbnailEntries.Count;
        }

        public void AddThumbnailItem(ThumbnailEntry thumbnailEntry)
        {
            if (!_thumbnailDictionary.ContainsKey(thumbnailEntry.FullPath))
            {
                _thumbnailDatabase.ThumbnailEntries.BeginEdit();
                _thumbnailDatabase.ThumbnailEntries.Add(thumbnailEntry);
                _thumbnailDatabase.ThumbnailEntries.EndEdit();

                while (!_thumbnailDictionary.ContainsKey(thumbnailEntry.FullPath) && !_thumbnailDictionary.TryAdd(thumbnailEntry.FullPath, thumbnailEntry)) Task.Delay(10);
            }
        }

        public bool ReduceCacheSize(long maxFileSize)
        {
            bool canLockDatabase = false;

            try
            {
                canLockDatabase = _fileManager.LockDatabase();
                if (!canLockDatabase)
                    return false;

                // Update SourceImageLength is a new property and needs to be calculated post process when it is zero
                var filesToProcess = _thumbnailDictionary.Values.Where(x => x.SourceImageLength == 0).Select(x => x.FullPath).ToList();
                var concurrentQueue = new ConcurrentQueue<ThumbnailEntry>();
                foreach (string fileName in filesToProcess)
                    if (!_thumbnailDictionary.TryRemove(fileName, out var entry))
                        concurrentQueue.Enqueue(entry);

                while (concurrentQueue.Count > 0)
                    if (concurrentQueue.TryDequeue(out var thumbnailEntry))
                    {
                        if (!_thumbnailDictionary.TryRemove(thumbnailEntry.FullPath, out var entry))
                        {
                            concurrentQueue.Enqueue(thumbnailEntry);
                            Task.Delay(1);
                        }
                        else
                        {
                            Task.Delay(2);
                        }
                    }

                var fileEntryList = _thumbnailDictionary.Values.OrderBy(x => x.SourceImageLength).ToList();
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

                _thumbnailDictionary = new ConcurrentDictionary<string, ThumbnailEntry>(fileEntryList.ToDictionary(x => x.Directory + x.FileName, x => x));

                _thumbnailDatabase.ThumbnailEntries.Clear();
                _thumbnailDatabase.ThumbnailEntries.AddRange(_thumbnailDictionary.Values);

                _fileManager.RecreateDatabase(_thumbnailDictionary.Values.ToList());
                SaveThumbnailDatabase();
                IsModified = true;
                return true;
            }
            finally
            {
                if (canLockDatabase) _fileManager.UnlockDatabase();
            }
        }

        public bool RemoveAllEntriesNotLocatedOnDisk()
        {
            var nonExistingFiles = new Queue<ThumbnailEntry>();

            if (nonExistingFiles.Count > 0)
            {
                _fileManager.LockDatabase();
                while (nonExistingFiles.Count > 0)
                {
                    var item = nonExistingFiles.Dequeue();
                }

                _fileManager.UnlockDatabase();
            }

            return true;
        }

        public Image AddThumbnailImage(string fullPath, Image thumbnailImage)
        {
            var entry = CreateThumbnailEntry(fullPath, thumbnailImage);
            if (entry == null)
                return null;

            AddThumbnailItem(entry.Item1);

            return entry.Item2;
        }
    }
}