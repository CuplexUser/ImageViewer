using System.Text.RegularExpressions;
using GeneralToolkitLib.Configuration;
using ImageViewer.Events;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Providers;
using ImageViewer.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public sealed class ThumbnailService : ServiceBase
    {
        //private const string ImageSearchPattern = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";
        private readonly ImageCacheService _cacheService;

        private readonly ImageProvider _imageProvider;

        private readonly Size _thumbnailSize;

        /// <summary>
        ///     The file manager
        /// </summary>
        private readonly FileManager _fileManager;

        /// <summary>
        ///     The file name reg exp
        /// </summary>
        private readonly Regex _fileNameRegExp = new(ImageSearchPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);


        /// <summary>
        ///     The thumbnail repository
        /// </summary>
        private readonly ThumbnailRepository _thumbnailRepository;


        /// <summary>
        ///     The abort scan
        /// </summary>
        private bool _abortScan;


        public ThumbnailService(FileManager fileManager, ThumbnailRepository thumbnailRepository, ImageCacheService cacheService, ImageProvider imageProvider)
        {
            _imageProvider = imageProvider;
            _fileManager = fileManager;
            _thumbnailRepository = thumbnailRepository;
            _cacheService = cacheService;
            _thumbnailSize= new Size(512,512);

            string databaseDirectory = ApplicationBuildConfig.UserDataPath;
            BasePath = databaseDirectory;
        }

        public ThumbnailServiceState ServiceState { get; private set; }

        public bool RunningThumbnailScan => (ServiceState & (ThumbnailServiceState.ScanningDirectory | ThumbnailServiceState.ScanningThumbnails)) > 0;

        public string BasePath { get; }

        public async Task<bool> ThumbnailDirectoryScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            if (ServiceState != ThumbnailServiceState.Idle)
            {
                Log.Warning("Aborting StartThumbnailScan because ServiceState = {ServiceState}", ServiceState);
                return false;
            }

            ServiceState = ThumbnailServiceState.ScanningDirectory;
            _abortScan = false;

            if (!Directory.Exists(path))
            {
                Log.Warning("Aborting StartThumbnailScan because of trying to scan a non existing directory: {path}", path);
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

            if (_abortScan)
                return scannedFiles;

            return await Task.Factory.StartNew(() =>
                {
                    if (progress != null && scannedFiles % 100 == 100)
                    {
                        progress.Report(new ThumbnailScanProgress { IsComplete = false, ScannedFiles = scannedFiles, TotalAmountOfFiles = totalFileCount });
                    }

                    return scannedFiles;
                }
            );
        }

        private List<string> GetSubDirList(string path)
        {
            var dirList = new List<string>();
            var dirInfo = new DirectoryInfo(path);

            foreach (DirectoryInfo directory in dirInfo.EnumerateDirectories())
            {
                if ((directory.Attributes & FileAttributes.Hidden) == 0)
                {
                    dirList.Add(directory.Name);
                    dirList.AddRange(GetSubDirList(directory.FullName));
                }
            }

            return dirList;
        }

        public int GetFileCount(IEnumerable<string> dirList)
        {
            var fileCount = 0;
            foreach (string dir in dirList)
            {
                fileCount += Directory.GetFiles(dir).Length;
            }

            return fileCount;
        }


        public void StopThumbnailScan()
        {
            if ((ServiceState & (ThumbnailServiceState.ScanningDirectory | ThumbnailServiceState.ScanningThumbnails)) != 0)
            {
                _abortScan = true;
                Log.Information("Stopping thumbnail scan when ServiceState is: {ServiceState}", ServiceState);
            }
            else
            {
                Log.Warning("Failed call to StopThumbnailScan because ServiceState is: {ServiceState}", ServiceState);
            }
        }

        public Image LoadThumbnailImage(string fullPath)
        {
            Image thumbnailImage = _thumbnailRepository.GetOrCreateThumbnailImage(fullPath,_thumbnailSize);

            return thumbnailImage;
        }


        public async Task<bool> OptimizeDatabaseAsync()
        {
            return await _thumbnailRepository.OptimizeDatabaseAsync();
        }

        public bool OptimizeDatabase()
        {
            var result = _thumbnailRepository.OptimizeDatabaseAsync().ConfigureAwait(true);
            return result.GetAwaiter().GetResult();
        }


        public bool SaveThumbnailDatabase()
        {
            var result = false;

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
            await Task.Delay(10);
            return true;
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
            Image thumbnailImage = _thumbnailRepository.GetOrCreateThumbnailImage(filename, _thumbnailSize);
            

            return thumbnailImage;
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
                Log.Warning("ThumbnailService RemoveAllNonAccessibleFilesFromDb was called when Service State was: {ServiceState}", ServiceState);
                return false;
            }

            return false; // DoMaintenanceTask(_dbManager.RemoveAllNonAccessibleFiles, WorkParameters.Empty);
        }

        /// <summary>
        ///     Truncates the size of the cache in Mb.
        /// </summary>
        /// <param name="maxSize">The maximum size.</param>
        public bool TruncateCacheSize(long maxSize)
        {
            if (ServiceState != ThumbnailServiceState.Idle)
            {
                Log.Warning("ThumbnailService TruncateCacheSize was called when Service State was: {ServiceState}", ServiceState);
                return false;
            }

            return false; // DoMaintenanceTask(_dbManager.ReduceCacheSize, new WorkParameters(maxSize));
        }

        public bool ClearDatabase()
        {
            if (ServiceState != ThumbnailServiceState.Idle)
            {
                Log.Warning("ThumbnailService Clear Database was called when Service State was: {ServiceState}", ServiceState);
                return false;
            }

            return false; // DoMaintenanceTask(_dbManager.ClearDatabase, WorkParameters.Empty);
        }

        public long GetDatabaseSize()
        {
            return _thumbnailRepository.GetDatabaseSize();
        }

        public Image GetFullScaleImage(string filename)
        {
            return _cacheService.GetImageFromCache(filename);
        }

        public Image CreateThumbnail(string imagePath, Size size)
        {
            Image img = _cacheService.GetImageFromCache(imagePath);

            return _imageProvider.CreateThumbnailFromImage(img, size);

            //return _fileManager.CreateThumbnail(imagePath, size);
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
        ///     Database maintenance state
        /// </summary>
        DatabaseMaintenance = 0x20
    }
}