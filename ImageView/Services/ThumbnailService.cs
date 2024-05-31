using GeneralToolkitLib.Configuration;
using ImageMagick;
using ImageViewer.Events;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Providers;
using ImageViewer.Repositories;
using System.Text.RegularExpressions;

namespace ImageViewer.Services;

[UsedImplicitly]
public sealed class ThumbnailService : ServiceBase
{
    //public const string ImageSearchPattern = @"^.+((\.jpe?g$)|(\.webp$)|(\.gif$)|(\.bmp$)|(\.png$))";
    private readonly ImageCacheService _cacheService;


    /// <summary>
    ///     The file manager
    /// </summary>
    private readonly FileManager _fileManager;

    /// <summary>
    ///     The file name regexp
    /// </summary>
    private readonly Regex _fileNameRegExp = new(ImageSearchPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

    private readonly ImageProvider _imageProvider;


    /// <summary>
    ///     The thumbnail repository
    /// </summary>
    private readonly ThumbnailRepository _thumbnailRepository;

    private readonly Size _thumbnailSize;


    /// <summary>
    ///     The abort scan
    /// </summary>
    private bool _abortScan;

    private CancellationTokenSource _tokenSource;


    public ThumbnailService(FileManager fileManager, ThumbnailRepository thumbnailRepository, ImageCacheService cacheService, ImageProvider imageProvider)
    {
        _imageProvider = imageProvider;
        _fileManager = fileManager;
        _thumbnailRepository = thumbnailRepository;
        _cacheService = cacheService;
        _thumbnailSize = new Size(512, 512);
        _tokenSource = new CancellationTokenSource();
        _tokenSource.Token.Register(CancelScanCallback);

        // Init database
        Task.Run(thumbnailRepository.InitDatabase).Wait();

        string databaseDirectory = ApplicationBuildConfig.UserDataPath;
        ServiceState = ThumbnailServiceState.Idle;
        BasePath = databaseDirectory;
    }

    public ThumbnailServiceState ServiceState { get; private set; }

    public bool RunningThumbnailScan => (ServiceState & (ThumbnailServiceState.ScanningDirectory | ThumbnailServiceState.ScanningThumbnails)) > 0;

    public string BasePath { get; }

    public async Task<bool> ThumbnailDirectoryScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories, CancellationToken token)
    {
        bool saveResult = false;
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
            ServiceState = ThumbnailServiceState.Idle;
            return false;
        }

        try
        {
            var dirList = scanSubdirectories ? GetSubDirList(path) : new List<string>();
            dirList.Add(path);

            if (_tokenSource.IsCancellationRequested)
            {
                _tokenSource = new CancellationTokenSource();
                _tokenSource.Token.Register(CancelScanCallback);
            }


            //int scannedFiles = dirList.TakeWhile(directory => !_abortScan).Sum(directory => PerformThumbnailScan(directory, progress));
            int totalFileCount = GetFileCount(dirList);
            int scannedFiles = await StartThumbnailDirectoryScanAsync(dirList, totalFileCount, progress, token);

            saveResult = await _thumbnailRepository.SaveThumbnailDatabaseAsync();

            progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = scannedFiles, ScannedFiles = scannedFiles, IsComplete = true });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "ThumbnailDirectoryScan exception for path {path}", path);
        }

        return saveResult;
    }

    private async Task<int> StartThumbnailDirectoryScanAsync(List<string> directories, int totalFileCount, IProgress<ThumbnailScanProgress> progress, CancellationToken token)
    {
        var scannedFiles = 0;

        if (_abortScan) return scannedFiles;

        foreach (string directory in directories)
        {
            var filesToScan = new DirectoryInfo(directory).GetFiles().Where(file => _fileNameRegExp.IsMatch(file.Name)).ToList();

            async Task ProcessFileAsync(FileInfo fileInfo)
            {
                try
                {
                    var thumbnailImage = await _thumbnailRepository.GetOrCreateThumbnailImageAsync(fileInfo.FullName, _thumbnailSize, _tokenSource.Token);

                    if (progress != null && scannedFiles % 20 == 0) progress.Report(new ThumbnailScanProgress { IsComplete = false, ScannedFiles = scannedFiles, TotalAmountOfFiles = totalFileCount });

                    if (thumbnailImage == null) throw new InvalidDataException("GetOrCreateThumbnailImage() returned a null image");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Exception during processing file: {file} in directory: {directory}", fileInfo.Name, directory);
                }

                Interlocked.Increment(ref scannedFiles);
            }

            await Task.WhenAll(filesToScan.Select(ProcessFileAsync));
        }

        progress?.Report(new ThumbnailScanProgress { IsComplete = true, ScannedFiles = scannedFiles, TotalAmountOfFiles = totalFileCount });

        return scannedFiles;
    }

    private void CancelScanCallback()
    {
        ServiceState = ThumbnailServiceState.Idle;
    }


    private List<string> GetSubDirList(string path)
    {
        var dirList = new List<string>();
        var dirInfo = new DirectoryInfo(path);

        foreach (var directory in dirInfo.EnumerateDirectories())
            if ((directory.Attributes & FileAttributes.Hidden) == 0)
            {
                dirList.Add(directory.Name);
                dirList.AddRange(GetSubDirList(directory.FullName));
            }

        return dirList;
    }

    public int GetFileCount(IEnumerable<string> dirList)
    {
        return dirList.Sum(dir => Directory.GetFiles(dir).Length);
    }


    public void StopThumbnailScan()
    {
        if ((ServiceState & (ThumbnailServiceState.ScanningDirectory | ThumbnailServiceState.ScanningThumbnails)) != 0)
        {
            _abortScan = true;
            _tokenSource.CancelAfter(250);
            _tokenSource.TryReset();
            Log.Information("Stopping thumbnail scan when ServiceState is: {ServiceState}", ServiceState);
        }
        else
        {
            Log.Warning("Failed call to StopThumbnailScan because ServiceState is: {ServiceState}", ServiceState);
        }
    }


    public bool OptimizeDatabase()
    {
        return _thumbnailRepository.OptimizeDatabase();
    }

    public async Task<bool> OptimizeDatabaseAsync()
    {
        return await Task.Factory.StartNew(() => _thumbnailRepository.OptimizeDatabase());
    }


    public async Task<bool> SaveThumbnailDatabaseAsync()
    {
        bool result = false;

        if (ServiceState == ThumbnailServiceState.Idle)
        {
            ServiceState = ThumbnailServiceState.SavingDatabase;
            result = await _thumbnailRepository.SaveThumbnailDatabaseAsync();
            ServiceState = ThumbnailServiceState.Idle;
        }

        return result;
    }

    public bool SaveThumbnailDatabase()
    {
        return SaveThumbnailDatabaseAsync().GetAwaiter().GetResult();;
    }

    public bool LoadThumbnailDatabase()
    {
        return LoadThumbnailDatabaseAsync().GetAwaiter().GetResult();
    }

    public async Task<bool> LoadThumbnailDatabaseAsync()
    {
        return await _thumbnailRepository.InitDatabase();
    }

    public int GetNumberOfCachedThumbnails()
    {
        return _thumbnailRepository.GetFileCacheCount();
    }

    public Image GetThumbnail(string filename, int timeout)
    {
        return GetThumbnailAsync(filename).WaitAsync(TimeSpan.FromMilliseconds(timeout)).Result;
    }

    public async Task<Image> GetThumbnailAsync(string filename)
    {
        return await _thumbnailRepository.GetOrCreateThumbnailImage(new FileInfo(filename), _thumbnailSize);
    }

    public string GetThumbnailDbFilePath()
    {
        return _thumbnailRepository.GetThumbnailDbFilePath();
    }

    private async Task<bool> DecreaseDatabaseSize(int maxThumbnailCount)
    {
        bool result;
        try
        {
            ServiceState = ThumbnailServiceState.DatabaseMaintenance;
            int currentCount = _thumbnailRepository.GetFileCacheCount();
            result = currentCount < maxThumbnailCount ? false : _thumbnailRepository.DecreaseDatabaseSize(maxThumbnailCount);
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
        //Enqueue job to clear database
        if (ServiceState == ThumbnailServiceState.Idle)
        {
            return _thumbnailRepository.ClearDatabase();
        }

        //TODO Add second attempt

        return true; 
    }

    public long GetDatabaseSize()
    {
        return _thumbnailRepository.GetDatabaseSize();
    }

    public Image GetFullScaleImage(string filename)
    {
        return _cacheService.GetImageFromCache(filename);
    }

    public MagickImage CreateThumbnail(string imagePath, Size size)
    {
        var img = _cacheService.GetImageFromCache(imagePath);

        var result = _imageProvider.CreateThumbnailFromImage(img, size);

        return result;
    }

    public bool CheckIfCached(string imageFilepath)
    {
        return _thumbnailRepository.CheckIfCached(imageFilepath);
    }
}