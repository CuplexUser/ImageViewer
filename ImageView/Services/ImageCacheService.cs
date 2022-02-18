using System;
using System.Drawing;
using System.IO;
using ImageViewer.Library.EventHandlers;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Properties;
using ImageViewer.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class ImageCacheService : ServiceBase
    {
        private readonly ImageCacheRepository _imageCacheRepository;

        private static readonly object CacheLock = new();
        public const long DefaultCacheSize = 134217728;// 128 Mb
        public const long MinCacheSize = 16777216;     //16 Mb
        public const long MaxCacheSize = 268435456;    // 256 Mb

        public ImageCacheService(ApplicationSettingsService applicationSettingsService, ImageCacheRepository imageCacheRepository, ImageLoaderService imageLoaderService)
        {
            _imageCacheRepository = imageCacheRepository;

            if (_imageCacheRepository.CacheSize < MinCacheSize)
            {
                _imageCacheRepository.SetCacheSize(MinCacheSize, CacheTruncatePriority.RemoveLargest);
                applicationSettingsService.SaveSettings();
            }

            applicationSettingsService.OnSettingsSaved += _applicationSettingsService_OnSettingsChanged;
            applicationSettingsService.OnSettingsLoaded += ApplicationSettingsService_OnSettingsLoaded;

            imageLoaderService.OnImageWasDeleted += ImageLoaderService_OnImageWasDeleted;
            imageLoaderService.OnImportComplete += ImageLoaderService_OnImportComplete;
        }

        private void ImageLoaderService_OnImportComplete(object sender, ProgressEventArgs e)
        {

        }

        private void ImageLoaderService_OnImageWasDeleted(object sender, ImageRemovedEventArgs e)
        {
            // Delete from cache
            _imageCacheRepository.RemoveImageFromCache(e.ImageReference.FileName);

        }

        private void ApplicationSettingsService_OnSettingsLoaded(object sender, EventArgs e)
        {
            if (sender is ApplicationSettingsService appSettingsService)
            {
                CacheSize = appSettingsService.Settings.ImageCacheSize;
            }
        }

        private void _applicationSettingsService_OnSettingsChanged(object sender, EventArgs e)
        {
            if (sender is ApplicationSettingsService appSettingsService)
            {
                CacheSize = appSettingsService.Settings.ImageCacheSize;
            }
        }


        public int CachedImages => _imageCacheRepository.CachedImages;

        public long CacheSpaceAllocated => _imageCacheRepository.CacheSpaceAllocated;

        public long CacheSize
        {
            get => _imageCacheRepository.CacheSize;
            set => _imageCacheRepository.SetCacheSize(value, CacheTruncatePriority.RemoveOldest);
        }

        public CachedImage GetCachedImage(string fileName)
        {
            lock (CacheLock)
            {
                return _imageCacheRepository.GetImageFromCache(fileName);
            }
        }

        public Image GetImageFromCache(string fileName)
        {
            lock (CacheLock)
            {
                var image = _imageCacheRepository.GetImageFromCache(fileName).GetImage(FileManager.GetImageFromByteArray);

                // Exception was thrown and handled
                if (image == null)
                {
                    _imageCacheRepository.RemoveImageFromCache(fileName);
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            image = Image.FromFile(fileName);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, $"Fail loading the current image file {fileName}");

                            // To avoid program failure
                            var bitmap = Resources.No_Camera_Image;
                            var imageFootprint = new RectangleF(Point.Empty, new SizeF(bitmap.Width, bitmap.Height));
                            image = Resources.No_Camera_Image.Clone(imageFootprint, bitmap.PixelFormat);
                        }
                    }
                }

                return image;
            }
        }

        public enum CacheTruncatePriority
        {
            RemoveOldest,
            RemoveLargest,
        }
    }
}