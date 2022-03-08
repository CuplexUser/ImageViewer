using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Converters;
using ImageViewer.Models;
using JetBrains.Annotations;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace ImageViewer.Managers
{
    /// <summary>
    /// File Manager has all the low level responsibility in allowing asynchronous read from disk but exclusively locked writes. 
    /// </summary>
    /// <seealso cref="ImageViewer.Managers.ManagerBase" />
    /// <seealso cref="System.IDisposable" />
    [UsedImplicitly]
    public class FileManager : ManagerBase, IDisposable
    {
        /// <summary>
        /// The file name
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// The file stream
        /// </summary>
        private FileStream _fileStream;

        private readonly ImageFactory _imageFactory;

        private readonly object _fileOperationLock = new object();

        /// <summary>
        /// The temporary database filename
        /// </summary>
        private const string TemporaryDatabaseFilename = "temp.ibd";

        /// <summary>
        /// The database img data filename
        /// </summary>
        private const string DatabaseImgDataFilename = "thumbs.ibd";

        /// <summary>
        /// The image manager
        /// </summary>
        private readonly Dictionary<string, bool> _directoryAccessDictionary;

        //private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        public FileManager()
        {
            _imageFactory = new ImageFactory(MetaDataMode.All);
            _fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseImgDataFilename);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public FileManager(string fileName)
        {
            _fileName = fileName;
            _directoryAccessDictionary = new Dictionary<string, bool>();
        }

        public enum AddOrUpdateStatus
        {
            Added,
            Updated,
            Unchanged
        };

        /// <summary>
        /// Reads the image from database.
        /// </summary>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns></returns>
        public Image ReadImage(ThumbnailEntry thumbnail)
        {
            if (thumbnail.Length == 0)
            {
                Log.Warning($"ReadImageFromDatabase for thumbnail '{thumbnail.FullPath}' was not possible because of 0 lengath");
                return null;
            }


            if (_fileStream == null)
                _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            _fileStream.Lock(thumbnail.FilePosition, thumbnail.Length);
            _fileStream.Position = thumbnail.FilePosition;

            var sr = new BinaryReader(_fileStream);

            var img = LoadFromByteArray(sr.ReadBytes(thumbnail.Length));

            _fileStream.Unlock(thumbnail.FilePosition, thumbnail.Length);

            return img;
        }

        /// <summary>
        /// Writes the image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns></returns>
        public FileEntry WriteImage(RawImage img)
        {
            if (_fileStream == null)
                _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            _fileStream.Position = Math.Max(_fileStream.Length - 1, 0);
            var position = _fileStream.Position;

            try
            {
                _fileStream.Lock(0, _fileStream.Length + img.ImageData.Length);
                _fileStream.Flush();
                _fileStream.Seek(0, SeekOrigin.End);
                _fileStream.Write(img.ImageData, 0, img.ImageData.Length);
                _fileStream.Flush(true);
                _fileStream.Unlock(0, _fileStream.Length);

                var fileEntry = new FileEntry
                {
                    Position = position,
                    Length = Convert.ToInt32(_fileStream.Position - position)
                };

                return fileEntry;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "WriteImage Exception: {Message}", ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Recreates the database.
        /// </summary>
        /// <param name="thumbnailEntries">The thumbnail entries.</param>
        public async Task RecreateDatabaseAsync(List<ThumbnailEntry> thumbnailEntries)
        {
            //if (_fileStream == null)
            //    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //else
            //    SaveToDisk();

            var tempFileName = GeneralConverters.GetDirectoryNameFromPath(_fileName) + TemporaryDatabaseFilename;

            FileStream temporaryDatabaseFile = null;
            try
            {
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);

                // Verify
                var deleteQueue = new Queue<ThumbnailEntry>();
                foreach (var thumbnailEntry in thumbnailEntries)
                {
                    if (thumbnailEntry.Length <= 0 || !File.Exists(Path.Combine(thumbnailEntry.Directory, thumbnailEntry.FileName)))
                        deleteQueue.Enqueue(thumbnailEntry);
                }

                while (deleteQueue.Count > 0)
                    thumbnailEntries.Remove(deleteQueue.Dequeue());

                temporaryDatabaseFile = File.OpenWrite(tempFileName);
                foreach (var entry in thumbnailEntries)
                {
                    var buffer = new byte[entry.Length];
                    lock (_fileOperationLock)
                    {
                        _fileStream.Position = entry.FilePosition;
                        _fileStream.Read(buffer, 0, entry.Length);
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
        /// Verifies that the file does excist and that the physical file has not been written to after the thumbnail was
        /// created.
        /// Assumes access to the directory
        /// </summary>
        /// <param name="thumbnailEntry">The thumbnail entry.</param>
        /// <returns>
        /// True if the thumbnail is up to date and the original file exists
        /// </returns>
        public static bool IsUpToDate(ThumbnailEntry thumbnailEntry)
        {
            var fileInfo = new FileInfo(thumbnailEntry.Directory + thumbnailEntry.FileName);
            return fileInfo.Exists && fileInfo.LastWriteTime == thumbnailEntry.SourceImageDate;
        }

        /// <summary>
        /// Clears the directory access cache.
        /// </summary>
        public void ClearDirectoryAccessCache()
        {
            _directoryAccessDictionary.Clear();
        }

        /// <summary>
        /// Determines whether [has access to directory] [the specified directory].
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <returns>
        ///   <c>true</c> if [has access to directory] [the specified directory]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAccessToDirectory(string directory)
        {
            if (_directoryAccessDictionary.ContainsKey(directory))
                return _directoryAccessDictionary[directory];

            var volume = GeneralConverters.GetVolumeLabelFromPath(directory);
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
        /// Gets the size of the database.
        /// </summary>
        /// <returns></returns>
        public long GetDbSize()
        {
            return 0;
        }

        /// <summary>
        /// Creates the raw image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public RawImage CreateRawImage(Image image)
        {
            var rawImage = CreateRawImageFromImage(image);

            return rawImage;
        }

        public Image CreateThumbnail(string fullPath, Size size)
        {
            FileStream fs = null;

            try
            {
                fs = File.OpenRead(fullPath);

                _imageFactory.Load(fs);
                _imageFactory.Resize(size);
                return _imageFactory.Image;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Create Thumbnail exception for file {fullPath}");
            }
            finally
            {
                fs?.Close();
            }

            return null;
        }

        public Image LoadFromByteArray(byte[] readBytes)
        {
            _imageFactory.Load(readBytes);
            return _imageFactory.Image;
        }


        public RawImage CreateRawImageFromImage(Image image)
        {
            if (image == null)
                return null;

            RawImage rawImage;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                ms.Flush();
                rawImage = new RawImage(ms.ToArray());
            }

            return rawImage;
        }

        public byte[] GetImageByteArray(string fileName, ISupportedImageFormat imageFormat)
        {
            try
            {
                byte[] imgBytes;
                _imageFactory.Load(fileName);
                if (!Equals(_imageFactory.CurrentImageFormat, imageFormat))
                {
                    _imageFactory.Format(imageFormat);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    _imageFactory.Save(ms);
                    ms.Flush();
                    imgBytes = ms.ToArray();
                }

                return imgBytes;
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"GetImageByteArray Failed using filename: {fileName} and image format: {imageFormat}");
                return null;
            }
        }

        public static Image GetImageFromByteArray(byte[] imgBytes)
        {
            try
            {
                ImageFactory imgImageFactory = new ImageFactory();
                imgImageFactory.Load(imgBytes);
                return imgImageFactory.Image;
            }
            catch (Exception e)
            {
                Log.Error(e, "GetImageFromByteArray failed");
                return null;
            }
        }

        private void Dispose(bool finalize)
        {
            _imageFactory?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}