using GeneralToolkitLib.Configuration;
using ImageViewer.Managers;
using ImageViewer.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageViewer.Events;
using ImageViewer.Repositories;
using JetBrains.Annotations;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public sealed class ThumbnailService : ServiceBase, IDisposable
    {
        /// <summary>
        //    ///     The image search pattern
        //    /// </summary>
        private const string ImageSearchPattern = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";

        /// <summary>
        ///     The file manager
        /// </summary>
        private readonly FileManager _fileManager;

        private ThumbnailServiceState _serviceState;

        /// <summary>
        ///     The file name reg exp
        /// </summary>
#pragma warning disable CS0169 // The field 'ThumbnailService._fileNameRegExp' is never used
        private readonly Regex _fileNameRegExp;
#pragma warning restore CS0169 // The field 'ThumbnailService._fileNameRegExp' is never used

        private readonly ThumbnailDbManager _dbManager;

        /// <summary>
        ///     The thumbnail repository
        /// </summary>
        private readonly ThumbnailRepository _thumbnailRepository;

        private readonly ImageCacheService _cacheService;


        /// <summary>
        ///     The abort scan
        /// </summary>
#pragma warning disable CS0414 // The field 'ThumbnailService._abortScan' is assigned but its value is never used
        private bool _abortScan;
#pragma warning restore CS0414 // The field 'ThumbnailService._abortScan' is assigned but its value is never used


        public ThumbnailService(FileManager fileManager, ThumbnailRepository thumbnailRepository, ThumbnailDbManager dbManager, ImageCacheService cacheService)
        {
            _fileManager = fileManager;
            _thumbnailRepository = thumbnailRepository;
            _dbManager = dbManager;
            _cacheService = cacheService;

            string databaseDirectory = ApplicationBuildConfig.UserDataPath;
            BasePath = databaseDirectory;
        }

        public ThumbnailServiceState ServiceState
        {
            get => _serviceState;
            private set => _serviceState = value;
        }

        public bool RunningThumbnailScan => (_serviceState & (ThumbnailServiceState.ScanningDirectory | ThumbnailServiceState.ScanningThumbnails)) > 0;

        public string BasePath { get; }

        public void Dispose()
        {
        }

#pragma warning disable CS0067 // The event 'ThumbnailService.StartedThumbnailScan' is never used
        public event EventHandler StartedThumbnailScan;
#pragma warning restore CS0067 // The event 'ThumbnailService.StartedThumbnailScan' is never used

#pragma warning disable CS0067 // The event 'ThumbnailService.CompletedThumbnailScan' is never used
        public event EventHandler CompletedThumbnailScan;
#pragma warning restore CS0067 // The event 'ThumbnailService.CompletedThumbnailScan' is never used

        private async Task<bool> ScanDirectoryAsync(string path, bool scanSubdirectories)
        {
            if (RunningThumbnailScan)
            {
                Log.Warning($"ScanDirectory was called when Service state was: {ServiceState}");
                return false;
            }


            return await ThumbnailDirectoryScan(path, null, scanSubdirectories);
        }

        public async Task<bool> ThumbnailDirectoryScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            if (ServiceState != ThumbnailServiceState.Idle)
            {
                Log.Warning($"Aborting StartThumbnailScan because ServiceState = {ServiceState}");
                return false;
            }

            ServiceState = ThumbnailServiceState.ScanningDirectory;
            _abortScan = false;

            if (!Directory.Exists(path))
            {
                Log.Warning("Aborting StartThumbnailScan because of trying to scan a non existing directory: " + path);
                return false;
            }

            var dirList = scanSubdirectories ? GetSubDirList(path) : new List<string>();
            dirList.Add(path);

            //int scannedFiles = dirList.TakeWhile(directory => !_abortScan).Sum(directory => PerformThumbnailScan(directory, progress));
            int totalFileCount = GetFileCount(dirList);
            int scannedFiles = await StartThumbnailDirectoryScan(dirList, totalFileCount, progress);

            bool saveResult = await _thumbnailRepository.SaveThumbnailDatabaseAsync();

            progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = scannedFiles, ScannedFiles = scannedFiles, IsComplete = true });
            return saveResult;
        }

        private async Task<int> StartThumbnailDirectoryScan(List<string> dirList, int totalFileCount, IProgress<ThumbnailScanProgress> progress)
        {
            var scannedFiles = 0;
            return await Task.Factory.StartNew(() =>
                {
                    if (progress != null && scannedFiles % 100 == 100) progress.Report(new ThumbnailScanProgress { IsComplete = false, ScannedFiles = scannedFiles, TotalAmountOfFiles = totalFileCount });

                    return scannedFiles;
                }
            );
        }

        private List<string> GetSubDirList(string path)
        {
            var dirList = new List<string>();
            var directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {
                dirList.Add(directory);
                dirList.AddRange(GetSubDirList(directory));
            }

            return dirList;
        }

        public int GetFileCount(IEnumerable<string> dirList)
        {
            var fileCount = 0;
            foreach (string dir in dirList) fileCount += Directory.GetFiles(dir).Length;

            return fileCount;
        }


        public void StopThumbnailScan()
        {
            if ((ServiceState & (ThumbnailServiceState.ScanningDirectory | ThumbnailServiceState.ScanningThumbnails)) != 0)
            {
                _abortScan = true;
                Log.Information($"Stopping thumbnail scan when ServiceState is: {ServiceState}");
            }
            else
            {
                Log.Warning($"Failed call to StopThumbnailScan because ServiceState is: {ServiceState}");
            }
        }

        public Image LoadThumbnailImage(string fullPath)
        {
            if (_thumbnailRepository.IsCached(fullPath))
            {
                var imgFromCache = _thumbnailRepository.GetThumbnailImage(fullPath);
                return imgFromCache;
            }

            var thumbnailImage = _fileManager.CreateThumbnail(fullPath, new Size(512, 512));

            return _thumbnailRepository.AddThumbnailImage(fullPath, thumbnailImage);
        }


        public async Task<bool> OptimizeDatabaseAsync()
        {
            return await _thumbnailRepository.OptimizeDatabaseAsync();
        }

        public bool OptimizeDatabase()
        {
            ConfiguredTaskAwaitable<bool> result = _thumbnailRepository.OptimizeDatabaseAsync().ConfigureAwait(true);
            return result.GetAwaiter().GetResult();
        }


        public bool SaveThumbnailDatabase()
        {
            bool result = false;

            if (ServiceState == ThumbnailServiceState.Idle)
            {
                ServiceState = ThumbnailServiceState.SavingDatabase;
                result = _thumbnailRepository.SaveThumbnailDatabase();
                ServiceState = ThumbnailServiceState.Idle;
            }

            return result;
        }

        public async Task<bool> SaveThumbnailDatabaseAsync()
        {
            if (ServiceState == ThumbnailServiceState.Idle)
            {
                ServiceState = ThumbnailServiceState.SavingDatabase;
                bool result = await _thumbnailRepository.SaveThumbnailDatabaseAsync();
                ServiceState = ThumbnailServiceState.Idle;
                return result;
            }

            return false;
        }

        public bool LoadThumbnailDatabase()
        {
            return _thumbnailRepository.LoadThumbnailDatabase();
        }

        public int GetNumberOfCachedThumbnails()
        {
            return _thumbnailRepository.GetFileCacheCount();
        }

        public Image GetThumbnail(string filename)
        {
            if (_thumbnailRepository.IsCached(filename)) return _thumbnailRepository.GetThumbnailImage(filename);

            var thumbnailImage = _fileManager.CreateThumbnail(filename, new Size(512, 512));
            return _thumbnailRepository.AddThumbnailImage(filename, thumbnailImage);
        }


        private bool DoMaintenanceTask(Func<WorkParameters, bool> maintenanceMethod, WorkParameters parameters)
        {
            bool result;
            try
            {
                ServiceState = ThumbnailServiceState.DatabaseMaintenance;
                result = maintenanceMethod(parameters);
                if (result)
                {
                    ServiceState |= ThumbnailServiceState.SavingDatabase;
                    result = SaveThumbnailDatabase();
                    if (!result)
                    {
                        Log.Error("DoMaintenanceTask failed to save database");
                    }

                    ServiceState ^= ThumbnailServiceState.SavingDatabase;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DoMaintenanceTask exception");
                result = false;
            }
            finally
            {
                ServiceState ^= ThumbnailServiceState.DatabaseMaintenance;
            }

            return result;
        }

        public bool RemoveAllNonAccessibleFilesFromDb()
        {
            if (ServiceState != ThumbnailServiceState.Idle)
            {
                Log.Warning($"ThumbnailService RemoveAllNonAccessibleFilesFromDb was called when Service State was: {ServiceState}");
                return false;
            }

            return DoMaintenanceTask(_dbManager.RemoveAllNonAccessibleFiles, WorkParameters.Empty);
        }

        /// <summary>
        ///     Truncates the size of the cache in Mb.
        /// </summary>
        /// <param name="maxSize">The maximum size.</param>
        public bool TruncateCacheSize(long maxSize)
        {
            if (ServiceState != ThumbnailServiceState.Idle)
            {
                Log.Warning($"ThumbnailService TruncateCacheSize was called when Service State was: {ServiceState}");
                return false;
            }

            return DoMaintenanceTask(_dbManager.ReduceCacheSize, new WorkParameters(maxSize));
        }

        public bool ClearDatabase()
        {
            if (ServiceState != ThumbnailServiceState.Idle)
            {
                Log.Warning($"ThumbnailService Clear Database was called when Service State was: {ServiceState}");
                return false;
            }

            return DoMaintenanceTask(_dbManager.ClearDatabase, WorkParameters.Empty);
        }

        public long GetDatabaseSize()
        {
            return _thumbnailRepository.GetDatabaseSize();
        }

        public Image GetFullScaleImage(string filename)
        {
            return _cacheService.GetImageFromCache(filename);
        }
    }

    /// <summary>
    /// </summary>
    [Flags]
    public enum ThumbnailServiceState
    {
        /// <summary>
        ///     The none
        /// </summary>
        Idle = 0x1,

        /// <summary>
        ///     Loading database state
        /// </summary>
        LoadingDatabase = 0x2,

        /// <summary>
        ///     Saving database state
        /// </summary>
        SavingDatabase = 0x4,

        /// <summary>
        ///     Scanning thumbnails state
        /// </summary>
        ScanningThumbnails = 0x8,

        /// <summary>
        ///     Scanning directory state
        /// </summary>
        ScanningDirectory = 0x10,

        /// <summary>
        /// Database maintenance state
        /// </summary>
        DatabaseMaintenance = 0x20
    }
}