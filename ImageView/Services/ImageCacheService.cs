using System;
using System.Drawing;
using System.IO;
using ImageViewer.Library.EventHandlers;
using ImageViewer.Managers;
using ImageViewer.Models;
using ImageViewer.Repositories;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Services
{
    [UsedImplicitly]
    public class ImageCacheService : ServiceBase
    {
        private readonly ImageCacheRepository _imageCacheRepository;
        private readonly ImageLoaderService _imageLoaderService;
        private static readonly object CacheLock = new object();
        public const long DefaultCacheSize = 16777216;
        public const long MinCacheSize = 5242880;
        public const long MaxCacheSize = 268435456;

        public ImageCacheService(ApplicationSettingsService applicationSettingsService, ImageCacheRepository imageCacheRepository, ImageLoaderService imageLoaderService)
        {
            _imageCacheRepository = imageCacheRepository;
            _imageLoaderService = imageLoaderService;
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


        public long CacheSize
        {
            get => _imageCacheRepository.CacheSize;
            set
            {
                _imageCacheRepository.SetCacheSize(value, CacheTruncatePriority.RemoveOldest);
            }
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
                            image = new Bitmap(500, 500);
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