using System;
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
    ///     Image Memory cache
    /// </summary>
    /// <seealso cref="ImageViewer.Repositories.RepositoryBase" />
    /// <seealso cref="System.IDisposable" />
    [UsedImplicitly]
    public class ImageCacheRepository : RepositoryBase
    {
        /// <summary>
        ///     The cached images
        /// </summary>
        private readonly Dictionary<string, CachedImage> _cachedImages;

        private readonly FileManager _fileManager;


        private CacheUsage _cashInfo;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageCacheRepository" /> class.
        /// </summary>
        public ImageCacheRepository(FileManager fileManager)
        {
            _fileManager = fileManager;
            _cachedImages = new Dictionary<string, CachedImage>();
            _cashInfo = new CacheUsage { MaxCacheSize = ImageCacheService.DefaultCacheSize };
        }

        /// <summary>
        ///     Gets the size of the cache space used.
        /// </summary>
        /// <value>
        ///     The size of the cache.
        /// </value>
        public long CacheSize
        {
            get
            {
                if (_cashInfo == null)
                {
                    _cashInfo = new CacheUsage();
                }


                if (!_cashInfo.IsUpdated)
                {
                    _cashInfo.CacheSize = _cachedImages.Select(x => x.Value.Size).Sum();
                    _cashInfo.IsUpdated = true;
                }

                return _cashInfo.CacheSize;
            }

        }

        /// <summary>
        ///     Gets the number of items in cache.
        /// </summary>
        /// <value>
        ///     The cached images.
        /// </value>
        public int CachedImages => _cachedImages.Count;

        #region Public Methods

        /// <summary>
        ///     Gets the image from cache.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public CachedImage LoadAndCacheImage(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                return _cachedImages[fileName];
            }

            var imageModel = CreateCachedImageModel(fileName);


            return imageModel;
        }

        /// <summary>
        ///     Removes the image from cache.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void RemoveImageFromCache(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                _cachedImages.Remove(fileName);
                _cashInfo.IsUpdated = false;
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <summary>
        ///     Sets the size of the cache.
        /// </summary>
        /// <param name="cacheSize">Size of the cache.</param>
        /// <param name="truncatePriority">The truncate priority.</param>
        /// <exception cref="ArgumentException">
        ///     Cache size can not be lower then
        ///     {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MinCacheSize, 0)}
        ///     or
        ///     Cache size can not be higher then
        ///     {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MaxCacheSize, 0)}
        /// </exception>
        public void SetCacheSize(long cacheSize, ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            if (cacheSize < ImageCacheService.MinCacheSize)
            {
                throw new ArgumentException($"Cache size can not be lower then {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MinCacheSize, 0)}");
            }

            if (cacheSize > ImageCacheService.MaxCacheSize)
            {
                throw new ArgumentException($"Cache size can not be higher then {GeneralConverters.FileSizeToStringFormatter.ConvertFileSizeToString(ImageCacheService.MaxCacheSize, 0)}");
            }

            if (cacheSize < CacheSize)
            {
                TruncateCache(truncatePriority);
            }
        }

        #endregion

        private void AutoTruncateCacheIfCloseToFull()
        {
            if (IsCacheCloseToFull())
            {
                long currentSize = CacheSize;
                long truncatedSize = Convert.ToInt64(Convert.ToDouble(currentSize) * 0.7d) * currentSize;
                SetCacheSize(truncatedSize, ImageCacheService.CacheTruncatePriority.RemoveOldest);
                Log.Debug("Auto truncated cache size to {truncatedSize}:", truncatedSize);

                _cashInfo.IsUpdated = false;
            }
        }


        private bool IsCacheCloseToFull()
        {
            return _cashInfo.CacheSize * 1.2d >= _cashInfo.MaxCacheSize;
        }

        /// <summary>
        ///     Creates the cached image model.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private CachedImage CreateCachedImageModel(string fileName)
        {
            var imageModel = new CachedImage();
            imageModel.SetImage(ImageConverter, fileName);
            _cachedImages.Add(fileName, imageModel);
            _cashInfo.CacheSize += imageModel.Size;

            AutoTruncateCacheIfCloseToFull();

            return imageModel;
        }

        /// <summary>
        ///     Images the converter.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private byte[] ImageConverter(string fileName)
        {
            byte[] imgByteArray = _fileManager.GetImageByteArray(fileName, new JpegFormat());
            return imgByteArray;
        }

        /// <summary>
        ///     Truncates the cache.
        /// </summary>
        /// <param name="truncatePriority">The truncate priority.</param>
        private void TruncateCache(ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            long truncatedSize = Convert.ToInt64(_cashInfo.MaxCacheSize * 0.75d);

            if (truncatePriority == ImageCacheService.CacheTruncatePriority.RemoveOldest)
            {
                while (_cachedImages.Count > 0 && CacheSize > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.AddedToCacheTime).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                }
            }
            else
            {
                while (_cachedImages.Count > 0 && CacheSize > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.Size).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                }
            }

            _cashInfo.IsUpdated = false;
            Log.Information("Cache Truncated to " + CacheSize + " bytes");
        }

        private class CacheUsage
        {
            public long CacheSize { get; set; }

            public long MaxCacheSize { get; set; }

            public bool IsUpdated { get; set; }
        }
    }
}