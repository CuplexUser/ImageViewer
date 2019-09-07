using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using GeneralToolkitLib.Converters;
using ImageProcessor.Imaging.Formats;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Services;
using JetBrains.Annotations;

namespace ImageViewer.Repositories
{
    [UsedImplicitly]
    public class ImageCacheRepository : RepositoryBase, IDisposable
    {
        private readonly Dictionary<string, CachedImage> _cachedImages;
        private readonly ImageManager _imageManager;
        private long _cacheSize;
        private long _maxCacheSize;
        private bool _cacheSizeIsValid = false;


        public ImageCacheRepository(ImageManager imageManager)
        {
            _imageManager = imageManager;
            _cachedImages = new Dictionary<string, CachedImage>();
            _maxCacheSize = ImageCacheService.DefaultCacheSize;
        }

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

        public int CachedImages => _cachedImages.Count;

        public CachedImage GetImageFromCache(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                return _cachedImages[fileName];
            }

            var imageModel = CreateCachedImageModel(fileName);

            return imageModel;
        }

        private CachedImage CreateCachedImageModel(string fileName)
        {
            var imageModel = new CachedImage();
            imageModel.SetImage(ImageConverter, fileName);
            _cachedImages.Add(fileName, imageModel);
            _cacheSizeIsValid = false;
            return imageModel;
        }

        private byte[] ImageConverter(string fileName)
        {
            byte[] imgByteArray = _imageManager.GetImageByteArray(fileName, new JpegFormat());
            return imgByteArray;
        }

        public void Dispose()
        {

        }

        public void SetCacheSize(long cacheSize, ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            if (cacheSize < ImageCacheService.MinCacheSize)
            {
                throw new ArgumentException($"Cache size can not be lower then {GeneralConverters.FileSizeToStringFormater.ConvertFileSizeToString(ImageCacheService.MinCacheSize, 0)}");
            }

            if (cacheSize > ImageCacheService.MaxCacheSize)
            {
                throw new ArgumentException($"Cache size can not be higher then {GeneralConverters.FileSizeToStringFormater.ConvertFileSizeToString(ImageCacheService.MaxCacheSize, 0)}");
            }

            if (cacheSize < CacheSize)
            {
                TruncateCache(truncatePriority);
            }

            _cacheSize = cacheSize;
            _maxCacheSize = cacheSize;

        }
        private void TruncateCache(ImageCacheService.CacheTruncatePriority truncatePriority)
        {
            _cacheSizeIsValid = false;
            long truncatedSize = Convert.ToInt64((double)_maxCacheSize * 0.75d);

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

        public void RemoveImageFromCache(string fileName)
        {
            if (_cachedImages.ContainsKey(fileName))
            {
                _cachedImages.Remove(fileName);
            }
        }
    }
}