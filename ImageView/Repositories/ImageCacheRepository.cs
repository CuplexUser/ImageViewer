using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneralToolkitLib.Converters;
using ImageProcessor.Imaging.Formats;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Services;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Repositories
{
    /// <summary>
    /// Image Memory cache
    /// </summary>
    /// <seealso cref="ImageViewer.Repositories.RepositoryBase" />
    /// <seealso cref="System.IDisposable" />
    [UsedImplicitly]
    public class ImageCacheRepository : RepositoryBase
    {
        /// <summary>
        /// The cached images
        /// </summary>
        private readonly Dictionary<string, CachedImage> _cachedImages;

        /// <summary>
        /// The file manager
        /// </summary>
        private readonly FileManager _fileManager;

        /// <summary>
        /// The cache stats
        /// </summary>
        private readonly FileCacheUsage _cacheStats;


        /// <summary>
        /// 
        /// </summary>
        private class FileCacheUsage
        {
            /// <summary>
            /// The cached images
            /// </summary>
            private readonly IQueryable<CachedImage> _cachedImages;
            /// <summary>
            /// The update cache usage
            /// </summary>
            private bool _updateCacheUsage;
            /// <summary>
            /// The cache usage
            /// </summary>
            private long _cacheUsage;

            /// <summary>
            /// Gets the cache usage.
            /// </summary>
            /// <value>
            /// The cache usage.
            /// </value>
            public long CacheUsage
            {
                get
                {
                    if (_updateCacheUsage)
                    {
                        _cacheUsage = GetCacheUsage();
                        _updateCacheUsage = false;
                    }

                    return _cacheUsage;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FileCacheUsage"/> class.
            /// </summary>
            /// <param name="cachedImages">The cached images.</param>
            public FileCacheUsage(IQueryable<CachedImage> cachedImages)
            {
                _cachedImages = cachedImages;
                _cacheUsage = 0;
                Invalidate();
            }

            /// <summary>
            /// Invalidates this instance.
            /// </summary>
            public void Invalidate()
            {
                _updateCacheUsage = true;
            }

            /// <summary>
            /// Gets the cache usage.
            /// </summary>
            /// <returns></returns>
            private long GetCacheUsage()
            {
                return _cachedImages.Sum(x => x.Size);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCacheRepository" /> class.
        /// </summary>
        /// <param name="fileManager">The file manager.</param>
        /// <param name="settings">The settings.</param>
        public ImageCacheRepository(FileManager fileManager, ApplicationSettingsService settings)
        {
            _fileManager = fileManager;
            _cachedImages = new Dictionary<string, CachedImage>();
            CacheSize = settings.Settings.ImageCacheSize;

            if (CacheSize < ImageCacheService.MinCacheSize || CacheSize > ImageCacheService.MaxCacheSize)
            {
                CacheSize = ImageCacheService.DefaultCacheSize;
            }

            _cacheStats = new FileCacheUsage(_cachedImages.Values.AsQueryable());
            _cacheStats.Invalidate();
        }

        /// <summary>
        /// Gets the number of items in cache.
        /// </summary>
        /// <value>
        /// The cached images.
        /// </value>
        public int CachedImages => _cachedImages.Count;


        // Referees to the maximum allocation limit
        /// <summary>
        /// Gets the size of the cache.
        /// </summary>
        /// <value>
        /// The size of the cache.
        /// </value>
        public long CacheSize { get; private set; }

        // Actual cache usage
        /// <summary>
        /// Gets the cache space allocated.
        /// </summary>
        /// <value>
        /// The cache space allocated.
        /// </value>
        public long CacheSpaceAllocated => _cacheStats.CacheUsage;

        #region Public Methods

        /// <summary>
        /// Gets the image from cache.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public CachedImage GetImageFromCache(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                return _cachedImages[fileName];
            }

            var imageModel = CreateCachedImageModel(fileName);
            _cacheStats.Invalidate();

            // Make sure we dont expand the cache size indefinitely by never removing anything from the image cache.
            if (_cacheStats.CacheUsage >= CacheSize)
            {
                Log.Debug("Truncating Image cache on Cache size: {CacheSize}", _cacheStats.CacheUsage);
                TruncateCache(ImageCacheService.CacheTruncatePriority.RemoveOldest);
            }

            return imageModel;
        }

        /// <summary>
        /// Removes the image from cache.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void RemoveImageFromCache(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                _cachedImages.Remove(fileName);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="newCacheSize">Size of the cache.</param>
        /// <param name="truncatePriority">The truncate priority.</param>
        /// <exception cref="System.ArgumentException">
        /// Cache size can not be lower then {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MinCacheSize, 0)}
        /// or
        /// Cache size can not be higher then {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MaxCacheSize, 0)}
        /// </exception>
        /// <exception cref="ArgumentException">Cache size can not be lower then
        /// {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MinCacheSize, 0)}
        /// or
        /// Cache size can not be higher then
        /// {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MaxCacheSize, 0)}</exception>
        public void SetCacheSize(long newCacheSize, ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            if (newCacheSize < ImageCacheService.MinCacheSize)
            {
                throw new ArgumentException($"Cache size can not be lower then {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MinCacheSize, 0)}");
            }

            if (newCacheSize > ImageCacheService.MaxCacheSize)
            {
                throw new ArgumentException($"Cache size can not be higher then {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MaxCacheSize, 0)}");
            }

            if (newCacheSize < _cacheStats.CacheUsage)
            {
                TruncateCache(truncatePriority);
            }

            CacheSize = newCacheSize;
        }

        #endregion

        /// <summary>
        /// Creates the cached image model.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private CachedImage CreateCachedImageModel(string fileName)
        {
            var imageModel = new CachedImage();
            imageModel.SetImage(ImageConverter, fileName);
            _cachedImages.Add(fileName, imageModel);
            _cacheStats.Invalidate();
            return imageModel;
        }

        /// <summary>
        /// Images the converter.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private byte[] ImageConverter(string fileName)
        {
            byte[] imgByteArray = _fileManager.GetImageByteArray(fileName, new JpegFormat());
            return imgByteArray;
        }

        /// <summary>
        /// Truncates the cache.
        /// </summary>
        /// <param name="truncatePriority">The truncate priority.</param>
        private void TruncateCache(ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            _cacheStats.Invalidate();
            long truncatedSize = Convert.ToInt64(CacheSize * 0.75d);

            if (truncatePriority == ImageCacheService.CacheTruncatePriority.RemoveOldest)
            {
                while (_cachedImages.Count > 0 && _cacheStats.CacheUsage > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.AddedToCacheTime).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                    _cacheStats.Invalidate();
                }
            }
            else
            {
                while (_cachedImages.Count > 0 && _cacheStats.CacheUsage > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.Size).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                    _cacheStats.Invalidate();
                }
            }
        }
    }
}