using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using GeneralToolkitLib.Converters;
using ImageViewer.DataContracts;
using ImageViewer.Models;
using ImageViewer.Repositories;
using JetBrains.Annotations;
using Serilog;
using Serilog.Context;

namespace ImageViewer.Managers
{
    ///// <summary>
    /////     ThumbnailManager
    ///// </summary>
    ///// <seealso cref="ImageViewer.Managers.ManagerBase" />
    ///// <seealso cref="System.IDisposable" />
    //[UsedImplicitly]
    //public class ThumbnailManager : ManagerBase, IDisposable
    //{
    //    /// <summary>
    //    ///     The image search pattern
    //    /// </summary>
    //    private const string ImageSearchPattern = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";

    //    /// <summary>
    //    ///     The file manager
    //    /// </summary>
    //    private readonly FileManager _fileManager;

    //    /// <summary>
    //    ///     The file name reg exp
    //    /// </summary>
    //    private readonly Regex _fileNameRegExp;

    //    /// <summary>
    //    ///     The image manager
    //    /// </summary>
    //    private readonly ImageManager _imageManager;

    //    /// <summary>
    //    ///     The thumbnail repository
    //    /// </summary>
    //    private readonly ThumbnailRepository _thumbnailRepository;

    //    /// <summary>
    //    ///     The abort scan
    //    /// </summary>
    //    private bool _abortScan;

    //    /// <summary>
    //    ///     Initializes a new instance of the <see cref="ThumbnailManager" /> class.
    //    /// </summary>
    //    /// <param name="thumbnailRepository">The thumbnail repository.</param>
    //    /// <param name="fileManager">The file manager.</param>
    //    /// <param name="imageManager">The image manager.</param>
    //    public ThumbnailManager(ThumbnailRepository thumbnailRepository, FileManager fileManager, ImageManager imageManager)
    //    {
    //        _thumbnailRepository = thumbnailRepository;
    //        _fileManager = fileManager;
    //        _imageManager = imageManager;

    //        _fileNameRegExp = new Regex(ImageSearchPattern, RegexOptions.IgnoreCase);
    //        CurrentState = ThumbnailServiceState.Idle;
    //    }

    //    /// <summary>
    //    ///     Gets a value indicating whether this instance is modified.
    //    /// </summary>
    //    /// <value>
    //    ///     <c>true</c> if this instance is modified; otherwise, <c>false</c>.
    //    /// </value>
    //    public bool IsModified { get; private set; }

    //    /// <summary>
    //    ///     Gets a value indicating whether this instance is loaded.
    //    /// </summary>
    //    /// <value>
    //    ///     <c>true</c> if this instance is loaded; otherwise, <c>false</c>.
    //    /// </value>
    //    public bool DataSourceReady { get; private set; }

    //    /// <summary>
    //    ///     Gets the state of the current.
    //    /// </summary>
    //    /// <value>
    //    ///     The state of the current.
    //    /// </value>
    //    public ThumbnailServiceState CurrentState { get; private set; }

    //    /// <summary>
    //    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        GC.Collect();
    //    }

    //    /// <summary>
    //    ///     Starts the thumbnail scan.
    //    /// </summary>
    //    /// <param name="path">The path.</param>
    //    /// <param name="progress">The progress.</param>
    //    /// <param name="scanSubdirectories">if set to <c>true</c> [scan subdirectories].</param>
    //    /// <returns></returns>
    //    public async Task<bool> StartThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
    //    {
    //        if (CurrentState != ThumbnailServiceState.Idle)
    //        {
    //            Log.Warning($"Aborting StartThumbnailScan because CurrentState = {CurrentState}");
    //            return false;
    //        }

    //        CurrentState = ThumbnailServiceState.ScanningThumbnails;
    //        _abortScan = false;

    //        if (!Directory.Exists(path))
    //        {
    //            Log.Warning("Aborting StartThumbnailScan because of trying to scan a non existing directory: " + path);
    //            return false;
    //        }

    //        var dirList = scanSubdirectories ? GetSubDirList(path) : new List<string>();
    //        dirList.Add(path);

    //        //int scannedFiles = dirList.TakeWhile(directory => !_abortScan).Sum(directory => PerformThumbnailScan(directory, progress));
    //        int totalFileCount = GetFileCount(dirList);
    //        int scannedFiles = await StartThumbnailDirectoryScan(dirList, totalFileCount, progress);

    //        bool saveResult = await _thumbnailRepository.SaveThumbnailDatabaseAsync();

    //        progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = scannedFiles, ScannedFiles = scannedFiles, IsComplete = true });
    //        return saveResult;
    //    }

    //    /// <summary>
    //    ///     Starts the thumbnail directory scan.
    //    /// </summary>
    //    /// <param name="dirList">The dir list.</param>
    //    /// <param name="totalFileCount">The total file count.</param>
    //    /// <param name="progress">The progress.</param>
    //    /// <returns></returns>
    //    private async Task<int> StartThumbnailDirectoryScan(List<string> dirList, int totalFileCount, IProgress<ThumbnailScanProgress> progress)
    //    {
    //        int scannedFiles = 0;
    //        return await Task.Factory.StartNew(() =>
    //            {
    //                if (progress != null && scannedFiles % 100 == 100)
    //                {
    //                    progress.Report(new ThumbnailScanProgress { IsComplete = false, ScannedFiles = scannedFiles, TotalAmountOfFiles = totalFileCount });
    //                }

    //                return scannedFiles;
    //            }
    //        );
    //    }

    //    /// <summary>
    //    ///     Gets the file count.
    //    /// </summary>
    //    /// <param name="dirList">The dir list.</param>
    //    /// <returns></returns>
    //    public int GetFileCount(IEnumerable<string> dirList)
    //    {
    //        int fileCount = 0;
    //        foreach (string dir in dirList)
    //        {
    //            fileCount += Directory.GetFiles(dir).Length;
    //        }

    //        return fileCount;
    //    }

    //    /// <summary>
    //    ///     Stops the thumbnail scan.
    //    /// </summary>
    //    public void StopThumbnailScan()
    //    {
    //        if ((CurrentState & (ThumbnailServiceState.ScanningDirectory | ThumbnailServiceState.ScanningThumbnails)) != 0)
    //        {
    //            _abortScan = true;
    //            Log.Information($"Stopping thumbnail scan when CurrentState is: {CurrentState}");
    //        }
    //        else
    //        {
    //            Log.Warning($"Failed call to StopThumbnailScan because CurrentState is: {CurrentState}");
    //        }
    //    }

    //    /// <summary>
    //    ///     Loads the thumbnail database.
    //    /// </summary>
    //    /// <returns></returns>
    //    public async Task<bool> LoadThumbnailDatabase()
    //    {
    //        if (CurrentState != ThumbnailServiceState.Idle)
    //        {
    //            throw new InvalidOperationException($"Load Database was called when Current state was {CurrentState}");
    //        }

    //        try
    //        {
    //            DataSourceReady = await Task<bool>.Factory.StartNew(() => _thumbnailRepository.LoadThumbnailDatabase());
    //            return DataSourceReady;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error(ex, "ThumbnailManager.LoadFromFile(string filename, string password) : " + ex.Message, ex);
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    ///     Loads the thumbnail image.
    //    /// </summary>
    //    /// <param name="fullPath">The full path.</param>
    //    /// <returns></returns>
    //    public Image LoadThumbnailImage(string fullPath)
    //    {
    //        if (_thumbnailRepository.IsCached(fullPath))
    //        {
    //            Image imgFromCache = _thumbnailRepository.GetThumbnailImage(fullPath);
    //            return imgFromCache;
    //        }

    //        Image thumbnailImage = _imageManager.CreateThumbnail(fullPath, new Size(512, 512));

    //        return _thumbnailRepository.AddThumbnailImage(fullPath, thumbnailImage);
    //    }

    //    /// <summary>
    //    ///     Gets the number of cached thumbnails.
    //    /// </summary>
    //    /// <returns></returns>
    //    public int GetNumberOfCachedThumbnails()
    //    {
    //        return _thumbnailRepository.GetFileCacheCount();
    //    }

    //    /// <summary>
    //    ///     Gets the sub dir list.
    //    /// </summary>
    //    /// <param name="path">The path.</param>
    //    /// <returns></returns>
    //    private List<string> GetSubDirList(string path)
    //    {
    //        var dirList = new List<string>();
    //        var directories = Directory.GetDirectories(path);

    //        foreach (string directory in directories)
    //        {
    //            dirList.Add(directory);
    //            dirList.AddRange(GetSubDirList(directory));
    //        }

    //        return dirList;
    //    }

    //    //private async Task<int> PerformThumbnailMultiThreadScan(IEnumerable<string> dirList, int totalNumberOfFiles, IProgress<ThumbnailScanProgress> progress)
    //    //{
    //    //    Queue<string> dirQueue = new Queue<string>(dirList);
    //    //    ConcurrentQueue<ThumbnailEntry> scannedThumbnailEntries = new ConcurrentQueue<ThumbnailEntry>();

    //    //    int threads = Environment.ProcessorCount * 2;
    //    //    int filesProcessed = 0;

    //    //    /*
    //    //     * Directory list contains main dir and all sub dirs
    //    //     * So work must be divided into two segments but only one dedicated thread can list files per directory
    //    //     */

    //    //    // Work Scheduler Task
    //    //    return await Task.Factory.StartNew(() =>
    //    //    {
    //    //        while (dirQueue.Count > 0 && !_abortScan)
    //    //        {
    //    //            var filenames = GetImageFilenamesInDirectory(dirQueue.Dequeue());

    //    //            while (filenames.Count < threads * 4 && dirQueue.Count > 0)
    //    //            {
    //    //                filenames.AddRange(GetImageFilenamesInDirectory(dirQueue.Dequeue()));
    //    //            }

    //    //            filesProcessed += filenames.Count;
    //    //            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = threads };
    //    //            var cancellationToken = parallelOptions.CancellationToken;

    //    //            Queue<string> filenameQueue = new Queue<string>(filenames);

    //    //            // Create work batch
    //    //            Task dataStorageTask = null;
    //    //            var taskList = new List<Task>();
    //    //            while (filenameQueue.Count > 0 && !_abortScan)
    //    //            {
    //    //                while (taskList.Count < threads && filenameQueue.Count > 0)
    //    //                {
    //    //                    taskList.Add(Task.Factory.StartNew(() =>
    //    //                    {
    //    //                        if (filenameQueue.Count > 0)
    //    //                        {
    //    //                            bool result = _thumbnailRepository.CreateThumbnail(filenameQueue.Dequeue()); // (), scannedThumbnailEntries); //     GetThumbnailEntry();
    //    //                            if (result)
    //    //                            {
    //    //                                scannedThumbnailEntries.TryDequeue()
    //    //                            }
    //    //                        }

    //    //                    }, cancellationToken));
    //    //                }

    //    //                Task.WaitAny(taskList.ToArray());
    //    //                taskList.RemoveAll(task => task.IsCompleted);

    //    //                if (dataStorageTask == null || dataStorageTask.IsCompleted)
    //    //                {
    //    //                    if (scannedThumbnailEntries.Count > 100)
    //    //                    {
    //    //                        dataStorageTask = Task.Factory.StartNew(() => { ProcessThumbnailData(scannedThumbnailEntries); }, cancellationToken);
    //    //                    }
    //    //                }
    //    //                progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = totalNumberOfFiles, ScannedFiles = filesProcessed, IsComplete = false });
    //    //            }

    //    //        }

    //    //        ProcessThumbnailData(scannedThumbnailEntries);

    //    //        return filesProcessed;
    //    //    });
    //    //}

    //    /// <summary>
    //    ///     Processes the thumbnail data.
    //    /// </summary>
    //    /// <param name="thumbnailDataQueue">The thumbnail data queue.</param>
    //    private void ProcessThumbnailData(ConcurrentQueue<ThumbnailEntry> thumbnailDataQueue)
    //    {
    //        while (thumbnailDataQueue.Count > 0)
    //        {
    //            if (thumbnailDataQueue.TryDequeue(out var data))
    //            {
    //                if (!_thumbnailRepository.IsCached(data.FullPath))
    //                {
    //                    _thumbnailRepository.AddThumbnailItem(data);
    //                }
    //            }
    //        }
    //    }

    //    /// <summary>
    //    ///     Performs the thumbnail scan.
    //    /// </summary>
    //    /// <param name="path">The path.</param>
    //    /// <param name="progress">The progress.</param>
    //    /// <returns></returns>
    //    private int PerformThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress)
    //    {
    //        var files = Directory.GetFiles(path);
    //        if (!Directory.Exists(path))
    //            return 0;

    //        if (!path.EndsWith("\\"))
    //            path += "\\";

    //        int filesToScan = files.Length;
    //        int scannedFiles = 0;

    //        if (_abortScan)
    //            return 0;

    //        foreach (string fullPath in files)
    //        {
    //            string fileName = GeneralConverters.GetFileNameFromPath(fullPath);
    //            if (_abortScan)
    //                break;

    //            if (!_fileNameRegExp.IsMatch(fileName) || _thumbnailRepository.IsCached(fullPath)) continue;
    //            Image img = null;

    //            try
    //            {
    //                img = _thumbnailRepository.GetThumbnailImage(fullPath);
    //            }
    //            catch (Exception exception)
    //            {
    //                Log.Error(exception, "Error loading file: " + fullPath);
    //            }

    //            if (img == null)
    //                continue;

    //            var fileInfo = new FileInfo(fullPath);

    //            var thumbnail = new ThumbnailEntryModel
    //            {
    //                Date = DateTime.Now,
    //                SourceImageDate = fileInfo.LastWriteTime,
    //                FileName = fileName,
    //                Directory = path,
    //                SourceImageLength = fileInfo.Length
    //            };

    //            IsModified = true;

    //            // Update progress
    //            scannedFiles++;
    //            progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = filesToScan, ScannedFiles = scannedFiles });
    //        }

    //        return scannedFiles;
    //    }

    //    /// <summary>
    //    ///     Loads the image from database.
    //    /// </summary>
    //    /// <param name="filename">The filename.</param>
    //    /// <returns></returns>
    //    private RawImage LoadImageFromDatabase(string filename)
    //    {
    //        ThumbnailEntryModel thumbnail = _thumbnailRepository.GeThumbnailEntry(filename);
    //        try
    //        {
    //            return _fileManager.ReadRawImageFromDatabase(thumbnail);
    //        }
    //        catch (Exception ex)
    //        {
    //            using (LogContext.PushProperty("Data", thumbnail))
    //            {
    //                Log.Error(ex, "LoadImageFromDatabase failed");
    //            }
    //        }

    //        return null;
    //    }

    //    /// <summary>
    //    ///     Adds the image to cache.
    //    /// </summary>
    //    /// <param name="img">The img.</param>
    //    /// <param name="path">The path.</param>
    //    /// <param name="fileName">Name of the file.</param>
    //    private void AddImageToCache(RawImage img, string path, string fileName)
    //    {
    //        _thumbnailRepository.AddThumbnailItem(new ThumbnailEntry());
    //    }

    //    /// <summary>
    //    ///     Gets the size of the thumbnail database file.
    //    /// </summary>
    //    /// <returns></returns>
    //    public long GetThumbnailDbFileSize()
    //    {
    //        return _thumbnailRepository.GetFileCacheSize();
    //    }

    //    /// <summary>
    //    ///     Removes all missing files and recreate database.
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool RemoveAllMissingFilesAndRecreateDb()
    //    {
    //        return _thumbnailRepository.RemoveAllEntriesNotLocatedOnDisk();
    //    }

    //    /// <summary>
    //    ///     Reduces the size of the cache. Prioritize removing the smallest original files size images first since they are
    //    ///     easiest to process yet take up equal amount of
    //    ///     storage as the large files.
    //    /// </summary>
    //    /// <param name="maxFileSize">Maximum size of the file.</param>
    //    /// <returns></returns>
    //    public bool ReduceCacheSize(long maxFileSize)
    //    {
    //        return _thumbnailRepository.ReduceCacheSize(maxFileSize);
    //    }

    //    /// <summary>
    //    ///     Clears the database.
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool ClearDatabase()
    //    {
    //        if (CurrentState != ThumbnailServiceState.Idle)
    //        {
    //            Log.Warning($"Tried to clear database when CurrentState was: {CurrentState}");
    //            return false;
    //        }

    //        try
    //        {
    //        }
    //        catch (Exception exception)
    //        {
    //            Log.Error(exception, "Failed to clear database");
    //            return false;
    //        }

    //        return true;
    //    }

    //    /// <summary>
    //    ///     Saves the thumbnail database asynchronous.
    //    /// </summary>
    //    /// <returns></returns>
    //    public async Task<bool> SaveThumbnailDatabaseAsync()
    //    {
    //        return await _thumbnailRepository.SaveThumbnailDatabaseAsync();
    //    }

    //    /// <summary>
    //    ///     Optimizes the database.
    //    /// </summary>
    //    public void OptimizeDatabase()
    //    {
    //        _thumbnailRepository.OptimizeDatabase();
    //    }
    //}

    
}