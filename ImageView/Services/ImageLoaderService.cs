using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using ImageViewer.Collections;
using ImageViewer.DataContracts;
using ImageViewer.Library.EventHandlers;
using JetBrains.Annotations;
using Serilog;

namespace ImageViewer.Services
{
    public enum ProgressStatusEnum
    {
        Started,
        Stopping,
        Running,
        Canceled,
        Complete
    }

    [UsedImplicitly]
    public class ImageLoaderService : ServiceBase, IDisposable
    {
        public delegate void ProgressUpdateEventHandler(object sender, ProgressEventArgs e);

        private const string ImageSearchPattern = @"^[a-zA-Z0-9_]((.+\.jpg$)|(.+\.png$)|(.+\.jpeg$)|(.+\.gif$))";
        private readonly Regex _fileNameRegExp;
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly BookmarkService _bookmarkService;
        private readonly IMapper _mapper;
        private readonly object _threadLock;
        private readonly WindowsIdentity _winId;
        private int _filesLoaded;
        private string _imageBaseDir;
        private List<ImageReference> _imageReferenceList;
        private int _progressInterval;
        private bool _runWorkerThread;
        private int _tickCount;
        private int _totalNumberOfFiles;


        public ImageLoaderService(BookmarkService bookmarkService, IMapper mapper)
        {
            _bookmarkService = bookmarkService;
            _mapper = mapper;
            _fileNameRegExp = new Regex(ImageSearchPattern, RegexOptions.IgnoreCase);
            _threadLock = new object();
            _progressInterval = 100;
            _randomNumberGenerator = RandomNumberGenerator.Create();

            _winId = WindowsIdentity.GetCurrent();
        }

        public int ProgressInterval
        {
            get => _progressInterval;
            set
            {
                if (value >= 10 && value <= 2500)
                    _progressInterval = value;
            }
        }

        public List<ImageReference> ImageReferenceList
        {
            get
            {
                if (IsRunningImport)
                    return null;

                return _imageReferenceList;
            }
        }

        public bool IsRunningImport { get; private set; }

        public event ProgressUpdateEventHandler OnProgressUpdate;
        public event ProgressUpdateEventHandler OnImportComplete;
        public event ImageRemovedEventHandler OnImageWasDeleted;

        #region Image Import

        private void DoImageImport()
        {
            try
            {
                lock (_threadLock)
                {
                    _totalNumberOfFiles = GetNumberOfFilesMatchingRegexp(_imageBaseDir);
                    _tickCount = Environment.TickCount;
                    _imageReferenceList = GetAllImagesRecursive(_imageBaseDir);
                    IsRunningImport = false;
                }

                OnImportComplete?.Invoke(this, new ProgressEventArgs(ProgressStatusEnum.Complete, _imageReferenceList.Count, _totalNumberOfFiles));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ImageLoaderService.DoImageImport()");
                IsRunningImport = false;
            }
        }

        private async Task<bool> DoImageImportFromBookmarkedImages()
        {
            try
            {
                if (!_bookmarkService.OpenBookmarks())
                {
                    return false;
                }

                IsRunningImport = true;
                List<ImageReference> imgReferenceList = null;
                await Task.Run(() =>
                {
                    var bookmarks = _bookmarkService.BookmarkManager.GetAllBookmarksRecursive(_bookmarkService.BookmarkManager.RootFolder);

                    var query = from b in bookmarks
                        orderby b.LastWriteTime
                        select new ImageReference
                        {
                            FileName = b.FileName,
                            Directory = b.Directory,
                            CompletePath = b.CompletePath,
                            Size = b.Size,
                            CreationTime = b.CreationTime,
                            LastWriteTime = b.LastWriteTime,
                            LastAccessTime = b.LastAccessTime
                        };

                    imgReferenceList = query.ToList();
                });

                // Use thread lock when updating the image refference datasource
                lock (_threadLock)
                {
                    _totalNumberOfFiles = imgReferenceList.Count;
                    _imageReferenceList = imgReferenceList;
                }

                IsRunningImport = false;
                OnImportComplete?.Invoke(this, new ProgressEventArgs(ProgressStatusEnum.Complete, _imageReferenceList.Count, _totalNumberOfFiles));
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ImageLoaderService.DoImageImportFromBookmarkedImages()");
                IsRunningImport = false;
            }

            return false;
        }

        private int GetNumberOfFilesMatchingRegexp(string basePath)
        {
            int noFiles = 0;
            try
            {
                if (!_runWorkerThread)
                    return 0;
                var currentDirectory = new DirectoryInfo(basePath);
                if (UserHasReadAccessToDirectory(currentDirectory))
                {
                    noFiles += currentDirectory.GetFiles().Count(f => _fileNameRegExp.IsMatch(f.Name));
                    noFiles += currentDirectory.GetDirectories().Sum(directory => GetNumberOfFilesMatchingRegexp(directory.FullName));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ImageLoaderService.GetNumberOfFilesMatchingRegexp for {basePath}", basePath);
            }

            return noFiles;
        }

        private bool UserHasReadAccessToDirectory(DirectoryInfo directoryInfo)
        {
            DirectorySecurity dSecurity = directoryInfo.GetAccessControl();
            AuthorizationRuleCollection authorizationRuleCollection = dSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (FileSystemAccessRule fsAccessRules in authorizationRuleCollection)
            {
                if (_winId.UserClaims.Any(c => c.Value == fsAccessRules.IdentityReference.Value) &&
                    fsAccessRules.FileSystemRights.HasFlag(FileSystemRights.ListDirectory) &&
                    fsAccessRules.AccessControlType == AccessControlType.Allow)
                {
                    return true;
                }
            }

            return false;
        }

        private List<ImageReference> GetAllImagesRecursive(string baseDir)
        {
            var imageReferenceList = new List<ImageReference>();

            if (!_runWorkerThread)
                return imageReferenceList;

            var currentDirectory = new DirectoryInfo(baseDir);

            if (!UserHasReadAccessToDirectory(currentDirectory))
                return imageReferenceList;

            var fileInfoArray = currentDirectory.GetFiles();
            foreach (var fileInfo in fileInfoArray)
            {
                if (_fileNameRegExp.IsMatch(fileInfo.Name))
                    imageReferenceList.Add(new ImageReference
                    {
                        FileName = fileInfo.Name,
                        Directory = baseDir,
                        CompletePath = fileInfo.FullName,
                        CreationTime = fileInfo.CreationTime,
                        LastWriteTime = fileInfo.LastWriteTime,
                        LastAccessTime = fileInfo.LastWriteTime,
                        Size = fileInfo.Length
                    });
            }

            _filesLoaded += imageReferenceList.Count;
            if (OnProgressUpdate != null && Environment.TickCount > _tickCount + _progressInterval)
            {
                _tickCount = Environment.TickCount;
                OnProgressUpdate.Invoke(this, new ProgressEventArgs(ProgressStatusEnum.Running, _filesLoaded, _totalNumberOfFiles));
            }

            foreach (DirectoryInfo directory in currentDirectory.GetDirectories())
                imageReferenceList.AddRange(GetAllImagesRecursive(directory.FullName));

            return imageReferenceList;
        }

        public async Task<bool> RunBookmarkImageImport()
        {
            if (!IsRunningImport)
            {
                return await DoImageImportFromBookmarkedImages();
            }

            return false;
        }

        public async Task<bool> RunImageImport(string path)
        {
            if (!IsRunningImport)
            {
                _imageReferenceList = null;
                _imageBaseDir = path;
                IsRunningImport = true;
                _filesLoaded = 0;

                await Task.Factory.StartNew(() =>
                {
                    _runWorkerThread = true;
                    DoImageImport();
                });

                return true;
            }

            return false;
        }

        public void StopImport()
        {
            _runWorkerThread = false;
        }

        #endregion

        internal bool PermanentlyRemoveFile(ImageReference imgRefElement)
        {
            int removedItems = 0;
            try
            {
                bool exists = File.Exists(imgRefElement.CompletePath);

                Log.Information("Attempting to delete file: {CompletePath}. File Exists {exists}", imgRefElement.CompletePath, exists);
                if (exists)
                {
                    File.Delete(imgRefElement.CompletePath);
                }

                removedItems = _imageReferenceList.RemoveAll(i => i == imgRefElement);
                ImageReference imgReference = _mapper.Map<ImageReference>(imgRefElement);
                imgRefElement = null;

                // Delete from cache
                OnImageWasDeleted?.Invoke(this, new ImageRemovedEventArgs(imgReference, _imageReferenceList.Count - 1));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ImageLoaderService.PermanentlyRemoveFile {imgRefElement}", imgRefElement);
            }

            return removedItems > 0;
        }

        private List<int> GetRandomImagePositionList()
        {
            var randomImagePosList = new List<int>();
            var randomData = new byte[ImageReferenceList.Count * 4];
            _randomNumberGenerator.GetBytes(randomData);
            int randomDataPointer = 0;
            var candidates = new List<int>();

            for (int i = 0; i < ImageReferenceList.Count; i++)
                candidates.Add(i);

            while (candidates.Count > 0)
            {
                int index = Math.Abs(BitConverter.ToInt32(randomData, randomDataPointer)) % candidates.Count;
                randomDataPointer += 4;

                randomImagePosList.Add(candidates[index]);
                candidates.RemoveAt(index);
            }

            return randomImagePosList;
        }

        public ImageReferenceCollection GenerateImageReferenceCollection(bool randomizeImageCollection)
        {
            List<int> randomImagePosList;
            if (randomizeImageCollection)
                randomImagePosList = GetRandomImagePositionList();
            else
            {
                randomImagePosList = new List<int>();
                for (int i = 0; i < ImageReferenceList.Count; i++)
                    randomImagePosList.Add(i);
            }

            var imageReferenceCollection = new ImageReferenceCollection(randomImagePosList, this);
            return imageReferenceCollection;
        }

        public List<ImageReference> GenerateThumbnailList(bool randomOrder)
        {
            var imgRefList = new List<ImageReference>();
            if (randomOrder)
            {
                var randomImagePosList = GetRandomImagePositionList();
                while (randomImagePosList.Count > 0)
                {
                    int position = randomImagePosList[0];
                    randomImagePosList.RemoveAt(0);
                    imgRefList.Add(_imageReferenceList[position]);
                }
            }
            else
            {
                foreach (ImageReference element in _imageReferenceList)
                {
                    imgRefList.Add(element);
                }
            }

            return imgRefList;
        }

        public void Dispose()
        {
            _randomNumberGenerator?.Dispose();
            _winId?.Dispose();
            GC.SuppressFinalize(this);
        }

        public ImageReference ImportSingleImage(string fileName, ref ImageReferenceCollection imgReferenceCollection)
        {
            var fileInfo = new FileInfo(fileName);

            var imgRefElement = new ImageReference
            {
                FileName = fileInfo.Name,
                Directory = fileInfo.DirectoryName,
                CompletePath = fileInfo.FullName,
                CreationTime = fileInfo.CreationTime,
                LastWriteTime = fileInfo.LastWriteTime,
                LastAccessTime = fileInfo.LastWriteTime,
                Size = fileInfo.Length
            };

            lock (_threadLock)
            {
                bool createdNewImgRefList = false;
                if (_imageReferenceList == null)
                {
                    _imageReferenceList = new List<ImageReference>();
                    createdNewImgRefList = true;
                }

                _imageReferenceList.Add(imgRefElement);
                _totalNumberOfFiles = _imageReferenceList.Count;
                if (createdNewImgRefList)
                {
                    imgReferenceCollection = new ImageReferenceCollection(GetRandomImagePositionList(), this);
                }
            }

            imgReferenceCollection.SingleImageLoadedSetAsCurrent();
            return imgReferenceCollection.CurrentImage;
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(ProgressStatusEnum progressStatus, int imagesLoaded, int totalNumberOfFiles)
        {
            ProgressStatus = progressStatus;
            ImagesLoaded = imagesLoaded;

            if (totalNumberOfFiles > 0)
                CompletionRate = imagesLoaded / (double) totalNumberOfFiles;
        }

        public ProgressStatusEnum ProgressStatus { get; private set; }
        public int ImagesLoaded { get; private set; }
        public double CompletionRate { get; set; }
    }
}