using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
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
    [UsedImplicitly]
    public class ThumbnailManager : ManagerBase, IDisposable
    {

        private const string ImageSearchPattern = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";
        private readonly Regex _fileNameRegExp;
        private bool _abortScan;
        private bool _isRunningThumbnailScan;
        private readonly ThumbnailRepository _thumbnailRepository;
        private readonly FileManager _fileManager;
        private readonly ImageManager _imageManager;


        public ThumbnailManager(ThumbnailRepository thumbnailRepository, FileManager fileManager, ImageManager imageManager)
        {
            _thumbnailRepository = thumbnailRepository;
            _fileManager = fileManager;
            _imageManager = imageManager;

            _fileNameRegExp = new Regex(ImageSearchPattern, RegexOptions.IgnoreCase);
        }

        public bool IsModified { get; private set; }
        public bool IsLoaded { get; private set; }

        public void Dispose()
        {
            
            
            GC.Collect();
        }

        public async Task StartThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            if (_isRunningThumbnailScan)
            {
                return;
            }
            

            _isRunningThumbnailScan = true;
            _abortScan = false;

            if (!Directory.Exists(path))
                return;

            var dirList = scanSubdirectories ? GetSubDirList(path) : new List<string>();
            dirList.Add(path);

            //int scannedFiles = dirList.TakeWhile(directory => !_abortScan).Sum(directory => PerformThumbnailScan(directory, progress));
            int totalFileCount = GetFileCount(dirList);
            int scannedFiles = await StartThumbnailDirectoryScan(dirList, totalFileCount, progress);

            
            _thumbnailRepository.SaveThumbnailDatabase();

            _isRunningThumbnailScan = false;
            progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = scannedFiles, ScannedFiles = scannedFiles, IsComplete = true });

        }

        private async Task<int> StartThumbnailDirectoryScan(List<string> dirList, int totalFileCount, IProgress<ThumbnailScanProgress> progress)
        {
            int scannedFiles = 0;
            return await Task.Factory.StartNew<int>( () => {



                if (progress!=null && scannedFiles % 100 == 100)
                {
                    progress.Report(new ThumbnailScanProgress {IsComplete = false,ScannedFiles = scannedFiles,TotalAmountOfFiles = totalFileCount,});
                }
                return scannedFiles;}
            
            
            );
        


        


        }

        

        public int GetFileCount(List<string> dirList)
        {
            int fileCount = 0;
            foreach (string dir in dirList)
            {
                fileCount += Directory.GetFiles(dir).Length;
            }

            return fileCount;
        }

        public void StopThumbnailScan()
        {
            if (_isRunningThumbnailScan)
                _abortScan = true;
        }



        public bool LoadThumbnailDatabase()
        {
            try
            {
                IsLoaded = _thumbnailRepository.LoadThumbnailDatabase();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ThumbnailManager.LoadFromFile(string filename, string password) : " + ex.Message, ex);
            }
            finally
            {
  
            }

            return false;
        }

        public Image LoadThumbnailImage(string fullPath)
        {
            if (_thumbnailRepository.IsCached(fullPath))
            {
                Image imgFromCache = _thumbnailRepository.GetThumbnailImage(fullPath);
                return imgFromCache;
            }

            Image thumbnailImage= _imageManager.CreateThumbnail(fullPath, new Size(512,512));

            return _thumbnailRepository.AddThumbnailImage(fullPath, thumbnailImage);

        }

        

        public int GetNumberOfCachedThumbnails()
        {
            return _thumbnailRepository.GetFileCacheCount();
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

        //private async Task<int> PerformThumbnailMultiThreadScan(IEnumerable<string> dirList, int totalNumberOfFiles, IProgress<ThumbnailScanProgress> progress)
        //{
        //    Queue<string> dirQueue = new Queue<string>(dirList);
        //    ConcurrentQueue<ThumbnailEntry> scannedThumbnailEntries = new ConcurrentQueue<ThumbnailEntry>();

        //    int threads = Environment.ProcessorCount * 2;
        //    int filesProcessed = 0;

        //    /*
        //     * Directory list contains main dir and all sub dirs
        //     * So work must be divided into two segments but only one dedicated thread can list files per directory
        //     */

        //    // Work Scheduler Task
        //    return await Task.Factory.StartNew(() =>
        //    {
        //        while (dirQueue.Count > 0 && !_abortScan)
        //        {
        //            var filenames = GetImageFilenamesInDirectory(dirQueue.Dequeue());

        //            while (filenames.Count < threads * 4 && dirQueue.Count > 0)
        //            {
        //                filenames.AddRange(GetImageFilenamesInDirectory(dirQueue.Dequeue()));
        //            }

        //            filesProcessed += filenames.Count;
        //            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = threads };
        //            var cancellationToken = parallelOptions.CancellationToken;

        //            Queue<string> filenameQueue = new Queue<string>(filenames);

        //            // Create work batch 
        //            Task dataStorageTask = null;
        //            var taskList = new List<Task>();
        //            while (filenameQueue.Count > 0 && !_abortScan)
        //            {
        //                while (taskList.Count < threads && filenameQueue.Count > 0)
        //                {
        //                    taskList.Add(Task.Factory.StartNew(() =>
        //                    {
        //                        if (filenameQueue.Count > 0)
        //                        {
        //                            bool result = _thumbnailRepository.CreateThumbnail(filenameQueue.Dequeue()); // (), scannedThumbnailEntries); //     GetThumbnailEntry();
        //                            if (result)
        //                            {
        //                                scannedThumbnailEntries.TryDequeue()
        //                            }
        //                        }

        //                    }, cancellationToken));
        //                }

        //                Task.WaitAny(taskList.ToArray());
        //                taskList.RemoveAll(task => task.IsCompleted);

        //                if (dataStorageTask == null || dataStorageTask.IsCompleted)
        //                {
        //                    if (scannedThumbnailEntries.Count > 100)
        //                    {
        //                        dataStorageTask = Task.Factory.StartNew(() => { ProcessThumbnailData(scannedThumbnailEntries); }, cancellationToken);
        //                    }
        //                }
        //                progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = totalNumberOfFiles, ScannedFiles = filesProcessed, IsComplete = false });
        //            }

        //        }

        //        ProcessThumbnailData(scannedThumbnailEntries);

        //        return filesProcessed;
        //    });
        //}

        private void ProcessThumbnailData(ConcurrentQueue<ThumbnailEntry> thumbnailDataQueue)
        {
            while (thumbnailDataQueue.Count > 0)
            {
                if (thumbnailDataQueue.TryDequeue(out var data))
                {
                    if (!_thumbnailRepository.IsCached(data.FullPath))
                    {
                        _thumbnailRepository.AddThumbnailItem(data);
                    }
                }
            }
        }


        private int PerformThumbnailScan(string path, IProgress<ThumbnailScanProgress> progress)
        {
            var files = Directory.GetFiles(path);
            if (!Directory.Exists(path))
                return 0;

            if (!path.EndsWith("\\"))
                path += "\\";

            int filesToScan = files.Length;
            int scannedFiles = 0;

            if (_abortScan)
                return 0;

            foreach (string fullPath in files)
            {
                string fileName = GeneralConverters.GetFileNameFromPath(fullPath);
                if (_abortScan)
                    break;

                if (!_fileNameRegExp.IsMatch(fileName) || _thumbnailRepository.IsCached(fullPath)) continue;
                Image img = null;

                try
                {
                    img =_thumbnailRepository.GetThumbnailImage(fullPath);
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error loading file: " + fullPath);
                }

                if (img == null)
                    continue;

                var fileInfo = new FileInfo(fullPath);

                var thumbnail = new ThumbnailEntryModel
                {
                    Date = DateTime.Now,
                    SourceImageDate = fileInfo.LastWriteTime,
                    FileName = fileName,
                    Directory = path,
                    SourceImageLength = fileInfo.Length,
                };

                

                
                
                IsModified = true;

                // Update progress
                scannedFiles++;
                progress?.Report(new ThumbnailScanProgress { TotalAmountOfFiles = filesToScan, ScannedFiles = scannedFiles });
            }

            return scannedFiles;
        }


        private RawImage LoadImageFromDatabase(string filename)
        {
            ThumbnailEntryModel thumbnail = _thumbnailRepository.GeThumbnailEntry(filename);
            try
            {
                return _fileManager.ReadRawImageFromDatabase(thumbnail);
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty("Data", thumbnail))
                {
                    Log.Error(ex, "LoadImageFromDatabase failed");
                }
            }
            return null;
        }

        private void AddImageToCache(RawImage img, string path, string fileName)
        {
            _thumbnailRepository.AddThumbnailItem(new ThumbnailEntry());
        }

        public long GetThumbnailDbFileSize()
        {
            return _thumbnailRepository.GetFileCacheSize();
        }

        public bool RemoveAllMissingFilesAndRecreateDb()
        {
            try
            {
                return _thumbnailRepository.RemoveAllEntriesNotLocatedOnDisk();
            }
            finally
            {
            
            }
        }

        /// <summary>
        ///     Reduces the size of the cach. Prioritize removing the smallest original files size images first since they are
        ///     easiest to proces yet take up equal amount of
        ///     storage as the large files.
        /// </summary>
        /// <param name="maxFileSize">Maximum size of the file.</param>
        /// <returns></returns>
        public bool ReduceCacheSize(long maxFileSize)
        {
            return _thumbnailRepository.ReduceCacheSize(maxFileSize);
        }

        public bool ClearDatabase()
        {
            if (_isRunningThumbnailScan)
            {
                return false;
            }

            try
            {
            
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to clear database");
                return false;
            }
            finally
            {
            
            }

            return true;
        }

        public bool SaveThumbnailDatabase()
        {
            return _thumbnailRepository.SaveThumbnailDatabase();
        }

        public void OptimizeDatabase()
        {
            _thumbnailRepository.OptimizeDatabase();
        }
    }
}