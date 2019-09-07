using System;
using System.Drawing;
using System.Threading.Tasks;
using GeneralToolkitLib.Configuration;
using ImageViewer.Managers;
using ImageViewer.Models;
using Serilog;

namespace ImageViewer.Services
{
    public sealed class ThumbnailService : ServiceBase, IDisposable
    {
        private readonly ThumbnailManager _thumbnailManager;

        private bool _isRunningScan;

        public ThumbnailService(ThumbnailManager thumbnailManager)
        {
            _thumbnailManager = thumbnailManager;
            string databaseDirectory = ApplicationBuildConfig.UserDataPath;
            BasePath = databaseDirectory;
        }

        public bool IsRunningScan
        {
            get => _isRunningScan;
            private set
            {
                if (_isRunningScan && !value)
                    CompletedThumbnailScan?.Invoke(this, new EventArgs());
                else
                    StartedThumbnailScan?.Invoke(this, new EventArgs());

                _isRunningScan = value;
            }
        }

        public string BasePath { get; }

        public void Dispose()
        {
            _thumbnailManager.Dispose();
        }

        public event EventHandler StartedThumbnailScan;
        public event EventHandler CompletedThumbnailScan;

        public async void ScanDirectory(string path, bool scanSubdirectories)
        {
            if (IsRunningScan) return;
            IsRunningScan = true;
            await _thumbnailManager.StartThumbnailScan(path, null, scanSubdirectories);
            _thumbnailManager.StopThumbnailScan();
            IsRunningScan = false;
        }

        /// <summary>
        ///     Scans the directory asynchronous. Update 2018-01-02 Implemented multi threaded scan which should decrease execution
        ///     time by a factor of 10
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="progress">The progress.</param>
        /// <param name="scanSubdirectories">if set to <c>true</c> [scan subdirectories].</param>
        /// <returns></returns>
        public async Task ScanDirectoryAsync(string path, IProgress<ThumbnailScanProgress> progress, bool scanSubdirectories)
        {
            if (IsRunningScan) return;

            try
            {
                await Task.Factory.StartNew(() =>
                {
                    IsRunningScan = true;
                    var scanTask = _thumbnailManager.StartThumbnailScan(path, progress, scanSubdirectories);
                    Task.WaitAll(scanTask);
                    _thumbnailManager.SaveThumbnailDatabase();
                    IsRunningScan = false;
                });
            }
            catch (Exception ex)
            {
                _thumbnailManager.StopThumbnailScan();
                Log.Error(ex, "Exception in ScanDirectoryAsync()");
                IsRunningScan = false;
            }
        }

        public void StopThumbnailScan()
        {
            _thumbnailManager.StopThumbnailScan();
        }

        public async Task OptimizeDatabaseAsync()
        {
            await Task.Factory.StartNew(() => { _thumbnailManager.OptimizeDatabase(); });
        }

        public void OptimizeDatabase()
        {
            _thumbnailManager.OptimizeDatabase();
        }


        public bool SaveThumbnailDatabase()
        {
            return _thumbnailManager.SaveThumbnailDatabase();
        }

        public bool LoadThumbnailDatabase()
        {
            return _thumbnailManager.LoadThumbnailDatabase();
        }

        public int GetNumberOfCachedThumbnails()
        {
            if (!_thumbnailManager.IsLoaded) _thumbnailManager.LoadThumbnailDatabase();

            return _thumbnailManager.GetNumberOfCachedThumbnails();
        }

        public Image GetThumbnail(string filename)
        {
            return _thumbnailManager.LoadThumbnailImage(filename);
        }

        public long GetThumbnailDbSize()
        {
            return _thumbnailManager.GetThumbnailDbFileSize();
        }

        public bool RemoveAllNonAccessibleFilesAndSaveDb()
        {
            try
            {
                if (_thumbnailManager.RemoveAllMissingFilesAndRecreateDb())
                {
                    _thumbnailManager.OptimizeDatabase();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "RemoveAllNonAccessibleFilesAndSaveDb Exception");
            }

            return false;
        }

        /// <summary>
        ///     Truncates the size of the cache in Mb.
        /// </summary>
        /// <param name="maxSize">The maximum size.</param>
        public bool TruncateCacheSize(long maxSize)
        {
            bool result;
            try
            {
                result = _thumbnailManager.ReduceCacheSize(maxSize);
                SaveThumbnailDatabase();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "TruncateCacheSize encountered an exception. Message: {Message}", ex.Message);
                return false;
            }

            return result;
        }

        public bool ClearDatabase()
        {
            if (IsRunningScan) return false;

            return _thumbnailManager.ClearDatabase();
        }
    }
}