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

        /// <summary>
        ///     The cache size
        /// </summary>
        private long _cacheSize;

        /// <summary>
        ///     The cache size is valid
        /// </summary>
        private bool _cacheSizeIsValid;

        /// <summary>
        ///     The maximum cache size
        /// </summary>
        private long _maxCacheSize;


        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageCacheRepository" /> class.
        /// </summary>
        public ImageCacheRepository(FileManager fileManager)
        {
            _fileManager = fileManager;
            _cachedImages = new Dictionary<string, CachedImage>();
            _maxCacheSize = ImageCacheService.DefaultCacheSize;
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
                if (!_cacheSizeIsValid)
                {
                    _cacheSize = _cachedImages.Select(x => x.Value.Size).Sum();
                    _cacheSizeIsValid = true;
                }

                return _cacheSize;
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
        public CachedImage GetImageFromCache(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                return _cachedImages[fileName];
            }

            var imageModel = CreateCachedImageModel(fileName);

            // Make sure we dont expand the cache size indefinitely by never removing anything from the image cache.
            if (CacheSize >= _maxCacheSize)
            {
                Log.Debug("Truncating Image cache on Cache size: {CacheSize}", CacheSize);
                TruncateCache(ImageCacheService.CacheTruncatePriority.RemoveOldest);
            }

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

            _cacheSize = cacheSize;
            _maxCacheSize = cacheSize;
        }

        #endregion

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
            _cacheSizeIsValid = false;
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
            _cacheSizeIsValid = false;
            long truncatedSize = Convert.ToInt64(_maxCacheSize * 0.75d);

            if (truncatePriority == ImageCacheService.CacheTruncatePriority.RemoveOldest)
            {
                while (_cachedImages.Count > 0 && CacheSize > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.AddedToCacheTime).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                    _cacheSizeIsValid = false;
                }
            }
            else
            {
                while (_cachedImages.Count > 0 && CacheSize > truncatedSize)
                {
                    string oldestImageFilename = _cachedImages.Values.OrderByDescending(x => x.Size).First().Filename;
                    _cachedImages.Remove(oldestImageFilename);
                    _cacheSizeIsValid = false;
                }
            }
        }
    }
}