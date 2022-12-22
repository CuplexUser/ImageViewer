using System.Text.RegularExpressions;
using GeneralToolkitLib.Configuration;
using ImageViewer.Events;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Providers;
using ImageViewer.Repositories;

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
    ///     The file name reg exp
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


    public ThumbnailService(FileManager fileManager, ThumbnailRepository thumbnailRepository, ImageCacheService cacheService, ImageProvider imageProvider)
    {
        _imageProvider = imageProvider;
        _fileManager = fileManager;
        _thumbnailRepository = thumbnailRepository;
        _cacheService = cacheService;
        _thumbnailSize = new Size(512, 512);

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
            ServiceState = ThumbnailServiceState.Idle;
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
                if (progress != null && scannedFiles % 100 == 100) progress.Report(new ThumbnailScanProgress { IsComplete = false, ScannedFiles = scannedFiles, TotalAmountOfFiles = totalFileCount });

                ParallelLoopResult result = Parallel.ForEach(dirList, (directoryPath, state, index) =>
                {
                    try
                    {
                        var files = new DirectoryInfo(directoryPath).GetFiles().Where(x => _fileNameRegExp.IsMatch(x.Name)).ToList();


                        // process files in parallel
                        var taskCount = 8;
                        if (files.Count < taskCount)
                            taskCount = files.Count;

                        var tasks = new Task[taskCount];
                        var taskIndex = 0;
                        var needWait = false;

                        foreach (FileInfo file in files)
                        {
                            Task t = Task.Factory.StartNew(() =>
                                {
                                    Image img = _thumbnailRepository.GetOrCreateThumbnailImage(file, _thumbnailSize);
                                }
                            );

                            taskIndex = -1;
                            for (var i = 0; i < taskCount; i++)
                                if (tasks[i] == null)
                                {
                                    taskIndex = i;
                                    break;
                                }

                            if (taskIndex >= 0)
                            {
                                tasks[taskIndex] = t;
                            }
                            else
                            {
                                taskIndex = Task.WaitAny(tasks);
                                tasks[taskIndex] = t;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, " Parallel.ForEach exception on Path: {path}", directoryPath);
                        state.Break();
                    }
                });


                return scannedFiles;
            }
        );
    }

    private List<string> GetSubDirList(string path)
    {
        var dirList = new List<string>();
        var dirInfo = new DirectoryInfo(path);

        foreach (DirectoryInfo directory in dirInfo.EnumerateDirectories())
            if ((directory.Attributes & FileAttributes.Hidden) == 0)
            {
                dirList.Add(directory.Name);
                dirList.AddRange(GetSubDirList(directory.FullName));
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
            Log.Information("Stopping thumbnail scan when ServiceState is: {ServiceState}", ServiceState);
        }
        else
        {
            Log.Warning("Failed call to StopThumbnailScan because ServiceState is: {ServiceState}", ServiceState);
        }
    }

    //public Image LoadThumbnailImage(string fullPath)
    //{
    //    Image thumbnailImage = _thumbnailRepository.GetOrCreateThumbnailImage(fullPath,_thumbnailSize);

    //    return thumbnailImage;
    //}


    public async Task<bool> OptimizeDatabaseAsync()
    {
        return await _thumbnailRepository.OptimizeDatabaseAsync();
    }

    //public bool OptimizeDatabase()
    //{
    //    var result = _thumbnailRepository.OptimizeDatabaseAsync().ConfigureAwait(true);
    //    return result.GetAwaiter().GetResult();
    //}


    public async Task<bool> SaveThumbnailDatabase()
    {
        var result = false;

        if (ServiceState == ThumbnailServiceState.Idle)
        {
            ServiceState = ThumbnailServiceState.SavingDatabase;
            result = await _thumbnailRepository.SaveThumbnailDatabaseAsync();
            ServiceState = ThumbnailServiceState.Idle;
        }

        return result;
    }

    public async Task<bool> SaveThumbnailDatabaseAsync()
    {
        await Task.Delay(10);
        return true;
    }

    public async Task<bool> LoadThumbnailDatabase()
    {
        return await _thumbnailRepository.InitDatabase();
    }

    public int GetNumberOfCachedThumbnails()
    {
        return _thumbnailRepository.GetFileCacheCount();
    }

    public Image GetThumbnail(string filename)
    {
        Image thumbnailImage = _thumbnailRepository.GetOrCreateThumbnailImage(new FileInfo(filename), _thumbnailSize);


        return thumbnailImage;
    }


    private async Task<bool> DoMaintenanceTask(Func<WorkParameters, bool> maintenanceMethod, WorkParameters parameters)
    {
        bool result;
        try
        {
            ServiceState = ThumbnailServiceState.DatabaseMaintenance;
            result = maintenanceMethod(parameters);
            if (result)
            {
                ServiceState |= ThumbnailServiceState.SavingDatabase;
                result = await SaveThumbnailDatabase();
                if (!result) Log.Error("DoMaintenanceTask failed to save database");

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