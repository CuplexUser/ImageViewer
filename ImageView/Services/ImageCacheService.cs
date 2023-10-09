using ImageViewer.Library.EventHandlers;
using ImageViewer.Models;
using ImageViewer.Providers;
using ImageViewer.Repositories;
using ImageViewer.Resources;

namespace ImageViewer.Services;

[UsedImplicitly]
public class ImageCacheService : ServiceBase
{
    public enum CacheTruncatePriority
    {
        RemoveOldest,
        RemoveLargest
    }

    public const long DefaultCacheSize = 0x8000000; // 128 Mb
    public const long MinCacheSize = 0x2000000; //32 Mb
    public const long MaxCacheSize = 0x20000000; // 512 Mb

    private static readonly object CacheLock = new();
    private readonly ImageCacheRepository _imageCacheRepository;
    private readonly ImageProvider _imageProvider;

    public ImageCacheService(ApplicationSettingsService applicationSettingsService, ImageCacheRepository imageCacheRepository, ImageLoaderService imageLoaderService,
        ImageProvider imageProvider)
    {
        _imageCacheRepository = imageCacheRepository;
        _imageProvider = imageProvider;

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


    public int CachedImages => _imageCacheRepository.CachedImages;

    public long CacheSpaceAllocated => _imageCacheRepository.CacheSpaceAllocated;

    public long CacheSize
    {
        get => _imageCacheRepository.CacheSize;
        set => _imageCacheRepository.SetCacheSize(value, CacheTruncatePriority.RemoveOldest);
    }

    protected virtual void ImageLoaderService_OnImportComplete(object sender, ProgressEventArgs e)
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
            var image = _imageCacheRepository.GetImageFromCache(fileName).GetImage(_imageProvider.RestoreImageFromCache);

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
                        var bitmap = Icons.No_Camera_Image;
                        var imageFootprint = new RectangleF(Point.Empty, new SizeF(bitmap.Width, bitmap.Height));
                        image = Icons.No_Camera_Image.Clone(imageFootprint, bitmap.PixelFormat);
                    }
                }
            }

            return image;
        }
    }

    public void ReloadSettings()
    {
    }
}