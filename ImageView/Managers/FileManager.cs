using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using GeneralToolkitLib.Configuration;
using GeneralToolkitLib.Converters;
using ImageViewer.DataContracts;
using ImageViewer.Models;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Managers
{
    public sealed class FileManager : ManagerBase, IDisposable
    {
        private readonly Dictionary<string, bool> _directoryAccessDictionary;
        private readonly string _fileName;
        private FileStream _fileStream;
        private const string TemporaryDatabaseFilename = "temp.ibd";
        private const string DatabaseImgDataFilename = "thumbs.ibd";
        private readonly object _fileOperationLock = new object();
        private readonly ImageManager _imageManager;
        

        [UsedImplicitly]
        public FileManager(ImageManager imageManager)
        {
            _imageManager = imageManager;
            _fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, DatabaseImgDataFilename);
            _directoryAccessDictionary = new Dictionary<string, bool>();
        }

        public FileManager(string fileName, ImageManager imageManager)
        {
            _fileName = fileName;
            _imageManager = imageManager;
            _directoryAccessDictionary = new Dictionary<string, bool>();
        }

        public bool IsLocked { get; private set; }

        public RawImage ReadRawImageFromDatabase(ThumbnailEntryModel thumbnail)
        {
            lock (_fileOperationLock)
            {
                if (_fileStream == null)
                    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);


                _fileStream.Lock(thumbnail.FilePosition, thumbnail.Length);
                _fileStream.Position = thumbnail.FilePosition;

                var ms = new MemoryStream();
                _fileStream.CopyTo(ms, thumbnail.Length);

                ms.Flush();
                byte[] imageData = ms.ToArray();
                ms.Close();
                ms.Dispose();

                _fileStream.Unlock(thumbnail.FilePosition, thumbnail.Length);

                return new RawImage(imageData);
            }

        }

        public Image ReadImageFromDatabase(ThumbnailEntry thumbnail)
        {
            if (thumbnail.Length == 0)
            {
                
                return null;
            }
            lock (_fileOperationLock)
            {
                if (_fileStream == null)
                    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);


                _fileStream.Lock(thumbnail.FilePosition, thumbnail.Length);
                _fileStream.Position = thumbnail.FilePosition;

                var sr = new BinaryReader(_fileStream);

                Image img= _imageManager.LoadFromByteArray(sr.ReadBytes(thumbnail.Length));
                
                _fileStream.Unlock(thumbnail.FilePosition, thumbnail.Length);

                return img;
            }
        }

        public FileEntry WriteImage(RawImage img)
        {
            lock (_fileOperationLock)
            {
                if (_fileStream == null)
                    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                _fileStream.Position = Math.Max(_fileStream.Length - 1, 0);
                long position = _fileStream.Position;

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
        }

        public void RecreateDatabase(List<ThumbnailEntry> thumbnailEntries)
        {
            if (_fileStream == null)
            {
                lock (_fileOperationLock)
                {
                    _fileStream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
            }
            else
            {
                SaveToDisk();
            }

            string tempFileName = GeneralConverters.GetDirectoryNameFromPath(_fileName) + TemporaryDatabaseFilename;

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
                    {
                        deleteQueue.Enqueue(thumbnailEntry);
                    }
                }

                while (deleteQueue.Count > 0)
                {
                    thumbnailEntries.Remove(deleteQueue.Dequeue());
                }

                temporaryDatabaseFile = File.OpenWrite(tempFileName);
                foreach (ThumbnailEntry entry in thumbnailEntries)
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

                CloseStream();
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

        private void SaveToDisk()
        {
            lock (_fileOperationLock)
            {
                _fileStream?.Flush(true);
            }
        }

        public bool WriteToDisk()
        {
            bool result = false;
            if (_fileStream != null)
            {
                lock (_fileOperationLock)
                {
                    if (_fileStream.CanWrite)
                    {
                        _fileStream.Flush(true);
                        result = true;
                    }
                }

                lock (_fileOperationLock)
                {
                    _fileStream.Close();
                    _fileStream = null;
                }
            }

            return result;
        }

        private void CloseStream()
        {
            WriteToDisk();
        }

        /// <summary>
        ///     Verifies that the file does excist and that the physical file has not been written to after the thumbnail was
        ///     created.
        ///     Assumes access to the directory
        /// </summary>
        /// <param name="thumbnailEntry"></param>
        /// <returns>True if the thumbnail is up to date and the original file exists</returns>
        public static bool IsUpToDate(ThumbnailEntry thumbnailEntry)
        {
            var fileInfo = new FileInfo(thumbnailEntry.Directory + thumbnailEntry.FileName);
            return fileInfo.Exists && fileInfo.LastWriteTime == thumbnailEntry.SourceImageDate;
        }

        public void ClearDirectoryAccessCache()
        {
            _directoryAccessDictionary.Clear();
        }

        public bool HasAccessToDirectory(string directory)
        {
            if (_directoryAccessDictionary.ContainsKey(directory))
                return _directoryAccessDictionary[directory];

            string volume = GeneralConverters.GetVolumeLabelFromPath(directory);
            var drives = DriveInfo.GetDrives().ToList();

            if (drives.Any(d => d.IsReady && d.Name.Equals(volume, StringComparison.CurrentCultureIgnoreCase)))
            {
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
            }

            _directoryAccessDictionary.Add(directory, false);
            return false;
        }

        public bool LockDatabase()
        {
            if (IsLocked)
                return false;

            IsLocked = true;
            return true;
        }

        public void UnlockDatabase()
        {
            IsLocked = false;
        }

        public long GetDbSize()
        {
            if (!File.Exists(_fileName))
                return 0;

            FileInfo fileInfo = new FileInfo(_fileName);
            return fileInfo.Length;
        }

        // Deletes the current file-container and recreates an empty file-container.
        public void DeleteBinaryContainer()
        {
            lock (_fileOperationLock)
            {
                CloseStream();
                File.Delete(_fileName);

                var fsStream = File.Create(_fileName);
                fsStream.Flush(true);
                fsStream.Close();
            }

        }

        public void Dispose()
        {
            CloseStream();
            _fileStream?.Dispose();
        }

        public RawImage CreateRawImage(Image image)
        {
            RawImage rawImage= _imageManager.CreateRawImageFromImage(image);

            return rawImage;
        }
    }
}
