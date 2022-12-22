using System.Drawing.Imaging;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Converters;
using ImageMagick;
using ImageViewer.Models;
using ImageViewer.Providers;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Managers;

/// <summary>
///     File Manager has all the low level responsibility in allowing asynchronous read from disk but exclusively locked
///     writes.
/// </summary>
/// <seealso cref="ImageViewer.Managers.ManagerBase" />
/// <seealso cref="System.IDisposable" />
[UsedImplicitly]
public class FileManager : ManagerBase, IDisposable
{
    public enum AddOrUpdateStatus
    {
        Added,
        Updated,
        Unchanged
    }

    /// <summary>
    ///     The temporary database filename
    /// </summary>
    private const string TemporaryDatabaseFilename = "temp.ibd";

    /// <summary>
    ///     The database img data filename
    /// </summary>
    private const string DatabaseImgDataFilename = "thumbs.ibd";

    /// <summary>
    ///     The image manager
    /// </summary>
    private readonly Dictionary<string, bool> _directoryAccessDictionary;

    /// <summary>
    ///     The file name
    /// </summary>
    private readonly string _fileName;

    private readonly object _fileOperationLock = new();

    private readonly ImageProvider _imageProvider;

    /// <summary>
    ///     The file stream
    /// </summary>
    private FileStream _fileStream;

    //private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileManager" /> class.
    /// </summary>
    public FileManager(ImageProvider imageProvider)
    {
        _imageProvider = imageProvider;

        _fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseImgDataFilename);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileManager" /> class.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="imageProvider"></param>
    public FileManager(string fileName, ImageProvider imageProvider)
    {
        _fileName = fileName;
        _imageProvider = imageProvider;
        _directoryAccessDictionary = new Dictionary<string, bool>();
    }

    public void Dispose()
    {
        Dispose(true);
    }


    //public Image ReadImage(ThumbnailEntryModel thumbnail)
    //{
    //    if (thumbnail.Length == 0)
    //    {
    //        Log.Warning("ReadImageFromDatabase for thumbnail '{FullPath}' was not possible because of 0 lengath", thumbnail.FullPath);
    //        return null;
    //    }


    //    if (_fileStream == null)
    //        _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    //    _fileStream.Lock(thumbnail.FilePosition, thumbnail.Length);
    //    _fileStream.Position = thumbnail.FilePosition;

    //    var sr = new BinaryReader(_fileStream);

    //    Image img = _imageProvider.LoadSystemImage(thumbnail.FullPath);

    //    //_fileStream.Unlock(thumbnail.FilePosition, thumbnail.Length);

    //    return img;
    //}


    /// <summary>
    ///     Recreates the database.
    /// </summary>
    /// <param name="thumbnailEntries">The thumbnail entries.</param>
    public async Task RecreateDatabaseAsync(List<ThumbnailEntryModel> thumbnailEntries)
    {
        //if (_fileStream == null)
        //    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //else
        //    SaveToDisk();

        await Task.Factory.StartNew(() => { RecreateDatabase(thumbnailEntries); });
    }

    private void RecreateDatabase(List<ThumbnailEntryModel> thumbnailEntries)
    {
        string tempFileName = GeneralConverters.GetDirectoryNameFromPath(_fileName) + TemporaryDatabaseFilename;

        FileStream temporaryDatabaseFile = null;
        try
        {
            if (File.Exists(tempFileName))
                File.Delete(tempFileName);

            // Verify
            var deleteQueue = new Queue<ThumbnailEntryModel>();
            foreach (ThumbnailEntryModel thumbnailEntry in thumbnailEntries)
                if (thumbnailEntry.Length <= 0 || !File.Exists(thumbnailEntry.OriginalImageModel.CompletePath))
                    deleteQueue.Enqueue(thumbnailEntry);

            while (deleteQueue.Count > 0)
                thumbnailEntries.Remove(deleteQueue.Dequeue());

            temporaryDatabaseFile = File.OpenWrite(tempFileName);
            foreach (ThumbnailEntryModel entry in thumbnailEntries)
            {
                var buffer = new byte[entry.Length];
                lock (_fileOperationLock)
                {
                    _fileStream.Position = entry.FilePosition;
                    int bytesRead = _fileStream.Read(buffer, 0, entry.Length);
                }

                entry.FilePosition = temporaryDatabaseFile.Position;
                temporaryDatabaseFile.Write(buffer, 0, entry.Length);
            }


            temporaryDatabaseFile.Flush(true);
            temporaryDatabaseFile.Close();
            temporaryDatabaseFile = null;
            File.Delete(_fileName);
            File.Move(tempFileName, _fileName);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Error in RecreateDatabase()");
        }
        finally
        {
            temporaryDatabaseFile?.Close();
            lock (_fileOperationLock)
            {
                _fileStream?.Close();
                _fileStream = null;
            }
        }
    }


    /// <summary>
    ///     Verifies that the file does excist and that the physical file has not been written to after the thumbnail was
    ///     created.
    ///     Assumes access to the directory
    /// </summary>
    /// <param name="thumbnailEntryModel">The thumbnail entryModel.</param>
    /// <returns>
    ///     True if the thumbnail is up to date and the original file exists
    /// </returns>
    public static bool IsUpToDate(ThumbnailEntryModel thumbnailEntryModel)
    {
        var fileInfo = new FileInfo(thumbnailEntryModel.OriginalImageModel.CompletePath);
        return fileInfo.Exists && fileInfo.LastWriteTime == thumbnailEntryModel.OriginalImageModel.LastModified;
    }

    /// <summary>
    ///     Clears the directory access cache.
    /// </summary>
    public void ClearDirectoryAccessCache()
    {
        _directoryAccessDictionary.Clear();
    }

    /// <summary>
    ///     Determines whether [has access to directory] [the specified directory].
    /// </summary>
    /// <param name="directory">The directory.</param>
    /// <returns>
    ///     <c>true</c> if [has access to directory] [the specified directory]; otherwise, <c>false</c>.
    /// </returns>
    public bool HasAccessToDirectory(string directory)
    {
        if (_directoryAccessDictionary.ContainsKey(directory))
            return _directoryAccessDictionary[directory];

        string volume = GeneralConverters.GetVolumeLabelFromPath(directory);
        var drives = DriveInfo.GetDrives().ToList();

        if (drives.Any(d => d.IsReady && d.Name.Equals(volume, StringComparison.CurrentCultureIgnoreCase)))
            try
            {
                var directoryInfo = new DirectoryInfo(directory);
                directoryInfo.EnumerateFiles();
                _directoryAccessDictionary.Add(directory, true);
                return true;
            }
            catch (Exception)
            {
                // ignored
            }

        _directoryAccessDictionary.Add(directory, false);
        return false;
    }


    /// <summary>
    ///     Gets the size of the database.
    /// </summary>
    /// <returns></returns>
    public long GetDbSize()
    {
        return 0;
    }

    /// <summary>
    ///     Creates the raw image.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <returns></returns>
    public RawImage CreateRawImage(Image image)
    {
        RawImage rawImage = CreateRawImageFromImage(image);

        return rawImage;
    }

    // Non direct call removed

    //public Image CreateThumbnail(string fullPath, Size size)
    //{
    //    try
    //    {
    //        // Open filePath image
    //        // Resize to {size}
    //        // return Image

    //        Image img = _imageProvider.CreateThumbnail(fullPath, size);
    //        return img;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error(ex, $"Create Thumbnail exception for file {fullPath}");
    //    }


    //    return null;
    //}

    public Image LoadFromByteArray(byte[] readBytes)
    {
        Image image = _imageProvider.LoadFromByteArray(readBytes);
        return image;
    }


    public RawImage CreateRawImageFromImage(Image image)
    {
        if (image == null)
            return null;

        RawImage rawImage;
        using (var ms = new MemoryStream())
        {
            image.Save(ms, ImageFormat.Jpeg);
            ms.Flush();
            rawImage = new RawImage(ms.ToArray());
        }

        return rawImage;
    }

    public byte[] GetImageByteArray(string fileName, MagickFormat encodeImageFormat)
    {
        try
        {
            using (var ms = new MemoryStream())
            {
                _imageProvider.LoadImageFile(fileName).Write(ms, encodeImageFormat);
                byte[] imgBytes = ms.ToArray();

                return imgBytes;
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception, $"GetImageByteArray Failed using filename: {fileName} and image format: {encodeImageFormat}");
            return null;
        }
    }


    private void Dispose(bool finalize)
    {
        GC.SuppressFinalize(this);
    }
}