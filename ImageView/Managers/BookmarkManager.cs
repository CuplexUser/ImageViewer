using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Events;
using ImageViewer.Models;
using ImageViewer.Utility;
using Serilog;

namespace ImageViewer.Managers
{
    public class BookmarkManager : ManagerBase
    {
        private BookmarkContainer _bookmarkContainer;

        public BookmarkManager()
        {
            _bookmarkContainer = CreateBookmarkContainer();
            RootFolder = _bookmarkContainer.RootFolder;
        }

        public BookmarkFolder RootFolder { get; private set; }

        public bool IsModified { get; private set; }

        public bool LoadedFromFile { get; private set; }

        public event BookmarkUpdatedEventHandler OnBookmarksUpdate;

        private void BookmarkUpdated(BookmarkUpdatedEventArgs e)
        {
            IsModified = true;
            OnBookmarksUpdate?.Invoke(this, e);
        }

        private BookmarkContainer CreateBookmarkContainer()
        {
            return new BookmarkContainer
            {
                ContainerId = Guid.NewGuid().ToString(),
                LastUpdate = DateTime.Now,
                RootFolder = new BookmarkFolder
                {
                    Name = "Root",
                    Id = Guid.NewGuid().ToString(),
                    Bookmarks = new List<Bookmark>(),
                    SortOrder = 0,
                    BookmarkFolders = new List<BookmarkFolder>()
                }
            };
        }

        public bool SaveToFile(string filename, string password)
        {
            try
            {
                //Make a copy of the original file if the original file is above 1 kb
                if (File.Exists(filename) && SystemIOHelper.GetFileSize(filename) > 1024)
                {
                    string copyFilename = GeneralConverters.GetDirectoryNameFromPath(filename, true) + "BookmarksCopy.dat";
                    if (File.Exists(copyFilename))
                    {
                        File.Delete(copyFilename);
                    }
                    File.Copy(filename, copyFilename);
                }

                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                bool successful = storageManager.SerializeObjectToFile(_bookmarkContainer, filename, null);

                if (successful)
                {
                    IsModified = false;
                }

                return successful;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BookmarkManager.SaveToFile(string filename, string password) : " + ex.Message, ex);
                return false;
            }
        }

        public bool LoadFromFile(string filename, string password)
        {
            try
            {
                LoadedFromFile = true;

                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                var bookmarkContainer = storageManager.DeserializeObjectFromFile<BookmarkContainer>(filename, null);

                if (bookmarkContainer?.RootFolder == null || string.IsNullOrEmpty(bookmarkContainer.ContainerId))
                {
                    return false;
                }

                int changesMade = PrepareContainer(bookmarkContainer);
                _bookmarkContainer = bookmarkContainer;
                RootFolder = _bookmarkContainer.RootFolder;
                if (changesMade > 0)
                {
                    Log.Information("Loaded Bookmaeksfile which had {changesMade} number of issues like faulty links. Resaving fixed file", changesMade);
                    SaveToFile(filename, password);
                }

                IsModified = true;

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BookmarkManager.LoadFromFile(string filename, string password) : " + ex.Message, ex);
            }

            return false;
        }

        public bool LoadFromFileAndAppendBookmarks(string filename, string password)
        {
            try
            {
                LoadedFromFile = true;

                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                var bookmarkContainer = storageManager.DeserializeObjectFromFile<BookmarkContainer>(filename, null);

                //storageManager.DeserializeObjectFromFile throws exception if the password is Incorrect and deserialization fails
                //just in case bookmarkContainer is null
                if (bookmarkContainer?.RootFolder == null || string.IsNullOrEmpty(bookmarkContainer.ContainerId))
                {
                    throw new Exception("LoadFromFileAndAppendBookmarks failed, bookmarkContainer was null");
                }

                PrepareContainer(bookmarkContainer);

                if (_bookmarkContainer == null)
                {
                    _bookmarkContainer = bookmarkContainer;
                }
                else
                {
                    RecursiveAdd(_bookmarkContainer.RootFolder, bookmarkContainer.RootFolder);
                }

                RootFolder = _bookmarkContainer.RootFolder;
                IsModified = true;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BookmarkManager.LoadFromFile(string filename, string password) : " + ex.Message);
            }

            return false;
        }

        private int PrepareContainer(BookmarkContainer bookmarkContainer)
        {
            int changes = 0;
            var rootFolder = bookmarkContainer.RootFolder;
            rootFolder.SortOrder = 0;

            if (string.IsNullOrEmpty(bookmarkContainer.ContainerId))
            {
                bookmarkContainer.ContainerId = Guid.NewGuid().ToString();
                changes++;
            }

            if (string.IsNullOrEmpty(rootFolder.Id))
            {
                rootFolder.Id = Guid.NewGuid().ToString();
                changes++;
            }

            if (rootFolder.BookmarkFolders == null)
            {
                rootFolder.BookmarkFolders = new List<BookmarkFolder>();
            }

            return RecursiveValidationOnContainer(rootFolder, null) + changes;
        }

        private int RecursiveValidationOnContainer(BookmarkFolder folder, string parentId)
        {
            int changes = 0;
            if (folder.Bookmarks == null)
            {
                folder.Bookmarks = new List<Bookmark>();
            }

            if (folder.ParentFolderId != parentId)
            {
                folder.ParentFolderId = parentId;
                changes++;
            }

            string folderId = folder.Id;
            foreach (var bookmark in folder.Bookmarks)
            {
                if (bookmark.ParentFolderId != folderId)
                {
                    bookmark.ParentFolderId = folderId;
                    changes++;
                }
            }

            if (folder.BookmarkFolders == null)
            {
                folder.BookmarkFolders = new List<BookmarkFolder>();
            }

            foreach (var bookmarkFolder in folder.BookmarkFolders)
            {
                if (bookmarkFolder.ParentFolderId != folder.Id)
                {
                    bookmarkFolder.ParentFolderId = folder.Id;
                    changes++;
                }

                changes += RecursiveValidationOnContainer(bookmarkFolder, folder.Id);
            }

            return changes;
        }


        private void RecursiveAdd(BookmarkFolder source, BookmarkFolder appendFrom)
        {
            foreach (var bookmark in appendFrom.Bookmarks)
            {
                if (!source.Bookmarks.Any(x => x.CompletePath == bookmark.CompletePath && x.Size == bookmark.Size))
                {
                    bookmark.ParentFolderId = source.Id;
                    source.Bookmarks.Add(bookmark);
                }
            }

            foreach (var folder in appendFrom.BookmarkFolders)
            {
                if (source.BookmarkFolders.All(x => x.Name != folder.Name))
                {
                    folder.ParentFolderId = source.Id;
                    source.BookmarkFolders.Add(folder);
                }

                if (folder.BookmarkFolders.Any())
                {
                    foreach (var subFolder in folder.BookmarkFolders)
                    {
                        var sFolder = source.BookmarkFolders.FirstOrDefault(x => x.Name == folder.Name);
                        if (sFolder != null)
                        {
                            RecursiveAdd(sFolder, subFolder);
                        }
                    }
                }
            }
        }

        private void ReindexSortOrder(bool reIndexFolders, bool reIndexBookmarks)
        {
            if (reIndexFolders)
            {
                var folderList = _bookmarkContainer.RootFolder.BookmarkFolders.OrderBy(f => f.SortOrder).ToList();
                for (int i = 0; i < folderList.Count; i++)
                {
                    folderList[i].SortOrder = i + 1;
                }

                _bookmarkContainer.RootFolder.BookmarkFolders.Sort((f1, f2) => f1.SortOrder.CompareTo(f2.SortOrder));
            }

            if (reIndexBookmarks)
            {
                var bookmarkList = _bookmarkContainer.RootFolder.Bookmarks.OrderBy(f => f.SortOrder).ToList();
                for (int i = 0; i < bookmarkList.Count; i++)
                {
                    bookmarkList[i].SortOrder = i + 1;
                }

                _bookmarkContainer.RootFolder.Bookmarks.Sort((b1, b2) => b1.SortOrder.CompareTo(b2.SortOrder));
            }
        }

        public BookmarkFolder AddBookmarkFolder(string parentId, string folderName)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentId);
            if (parentFolder == null)
                return null;

            var folder = new BookmarkFolder
            {
                Name = folderName,
                Id = Guid.NewGuid().ToString(),
                ParentFolderId = parentFolder.Id,
                SortOrder = _bookmarkContainer.RootFolder.BookmarkFolders.Count,
                BookmarkFolders = new List<BookmarkFolder>(),
                Bookmarks = new List<Bookmark>()
            };

            parentFolder.BookmarkFolders.Add(folder);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof(BookmarkFolder)));
            return folder;
        }

        public BookmarkFolder InsertBookmarkFolder(string parentId, string folderName, int index)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentId);
            if (parentFolder == null)
                return null;

            if (index < 0 || index > parentFolder.BookmarkFolders.Count)
                return null;

            var folder = new BookmarkFolder
            {
                Name = folderName,
                ParentFolderId = parentFolder.Id,
                SortOrder = index,
                Id = Guid.NewGuid().ToString(),
                BookmarkFolders = new List<BookmarkFolder>(),
                Bookmarks = new List<Bookmark>()
            };

            var postItems =
                parentFolder.BookmarkFolders.Where(b => b.SortOrder >= index).OrderBy(b => b.SortOrder).ToList();
            foreach (BookmarkFolder item in postItems)
            {
                item.SortOrder = item.SortOrder + 1;
            }

            parentFolder.BookmarkFolders.Add(folder);
            parentFolder.BookmarkFolders.Sort((f1, f2) => f1.SortOrder.CompareTo(f2.SortOrder));
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof(BookmarkFolder)));
            return folder;
        }

        public Bookmark AddBookmark(string parentFolderId, string boookmarkName, ImageReferenceElement imgRef)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return null;

            var bookmark = new Bookmark
            {
                BoookmarkName = boookmarkName,
                CompletePath = imgRef.CompletePath,
                CreationTime = imgRef.CreationTime,
                Directory = imgRef.Directory,
                FileName = imgRef.FileName,
                LastAccessTime = imgRef.LastAccessTime,
                LastWriteTime = imgRef.LastWriteTime,
                Size = imgRef.Size,
                SortOrder = parentFolder.Bookmarks.Count,
                ParentFolderId = parentFolder.Id
            };

            parentFolder.Bookmarks.Add(bookmark);
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof(Bookmark)));
            return bookmark;
        }

        public Bookmark InsertBookmark(string parentFolderId, string boookmarkName, ImageReferenceElement imgRef, int index)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return null;

            if (index < 0 || index > parentFolder.Bookmarks.Count)
                return null;

            var bookmark = new Bookmark
            {
                BoookmarkName = boookmarkName,
                CompletePath = imgRef.CompletePath,
                CreationTime = imgRef.CreationTime,
                Directory = imgRef.Directory,
                FileName = imgRef.FileName,
                LastAccessTime = imgRef.LastAccessTime,
                LastWriteTime = imgRef.LastWriteTime,
                Size = imgRef.Size,
                SortOrder = index,
                ParentFolderId = parentFolder.Id
            };

            var postItems = parentFolder.Bookmarks.Where(b => b.SortOrder >= index).OrderBy(b => b.SortOrder).ToList();
            foreach (Bookmark item in postItems)
            {
                item.SortOrder = item.SortOrder + 1;
            }

            parentFolder.Bookmarks.Add(bookmark);
            parentFolder.Bookmarks.Sort((b1, b2) => b1.SortOrder.CompareTo(b2.SortOrder));
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof(Bookmark)));
            return bookmark;
        }

        public bool MoveBookmark(Bookmark bookmark, string destinationFolderId)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmark.ParentFolderId);
            BookmarkFolder destinationFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, destinationFolderId);

            if ((parentFolder == null) | (destinationFolder == null) || parentFolder == destinationFolder)
                return false;

            parentFolder.Bookmarks.Remove(bookmark);
            destinationFolder.Bookmarks.Add(bookmark);
            bookmark.ParentFolderId = destinationFolder.Id;

            return true;
        }

        public bool DeleteBookmark(Bookmark bookmark)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmark.ParentFolderId);

            if (parentFolder == null)
                return false;

            bool success = parentFolder.Bookmarks.Remove(bookmark);
            if (success)
            {
                ReindexSortOrder(false, true);
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmark, typeof(Bookmark)));
            }
            return success;
        }

        public bool DeleteBookmarkByFilename(string parentFolderId, string fileName)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
            if (parentFolder == null)
                return false;

            Bookmark bookmarkToDelete = null;
            foreach (Bookmark bookmark in parentFolder.Bookmarks)
            {
                if (bookmark.FileName == fileName)
                {
                    bookmarkToDelete = bookmark;
                    break;
                }
            }

            if (bookmarkToDelete != null)
            {
                parentFolder.Bookmarks.Remove(bookmarkToDelete);
                ReindexSortOrder(false, true);
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmark, typeof(Bookmark)));
                return true;
            }

            return false;
        }


        public bool DeleteBookmarkFolder(BookmarkFolder folder)
        {
            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, folder.ParentFolderId);
            if (parentFolder == null)
                return false;

            bool success = parentFolder.BookmarkFolders.Remove(folder);
            if (success)
            {
                ReindexSortOrder(false, true);
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmarkFolder, typeof(Bookmark)));
            }
            return success;
        }

        public bool DeleteBookmarkFolderById(string folderId)
        {
            BookmarkFolder bookmarkFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, folderId);

            if (bookmarkFolder?.ParentFolderId == null)
                return false;

            BookmarkFolder parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmarkFolder.ParentFolderId);
            bool result = parentFolder.BookmarkFolders.Remove(bookmarkFolder);
            ReindexSortOrder(true, false);
            if (result)
                BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmarkFolder, typeof(Bookmark)));

            return result;
        }

        private BookmarkFolder GetBookmarkFolderById(BookmarkFolder rootFolder, string id)
        {
            BookmarkFolder subFolder = null;
            if (rootFolder.Id == id)
                return rootFolder;

            foreach (BookmarkFolder bookmarkFolder in rootFolder.BookmarkFolders)
            {
                if (bookmarkFolder.Id == id)
                {
                    return bookmarkFolder;
                }
                if (bookmarkFolder.BookmarkFolders != null && bookmarkFolder.BookmarkFolders.Count > 0)
                    subFolder = GetBookmarkFolderById(bookmarkFolder, id);

                if (subFolder != null)
                    return subFolder;
            }

            return null;
        }

        public int RemoveDuplicates()
        {
            return RemoveDuplicates(_bookmarkContainer.RootFolder);
        }

        // Remove duplicate complete filename bookmark, prioritize tree depth
        private int RemoveDuplicates(BookmarkFolder rootFolder)
        {
            var allBookmarks = GetAllBookmarksIncludingSubfolders(rootFolder).OrderBy(x => x.CompletePath);
            var removeQueue = new Queue<Bookmark>();

            foreach (var duplicateGroup in allBookmarks.GroupBy(x => x.CompletePath))
            {
                if (duplicateGroup.Count() > 1)
                {
                    var duplicateList = duplicateGroup.OrderBy(x => x.ParentFolderId).ToList();

                    var itemToKeep = duplicateList.FirstOrDefault(x => x.ParentFolderId != rootFolder.Id) ?? duplicateList.First();
                    duplicateList.Remove(itemToKeep);

                    foreach (var bookmark in duplicateList)
                    {
                        removeQueue.Enqueue(bookmark);
                    }
                }
            }

            int removedItems = removeQueue.Count;
            while (removeQueue.Count > 0)
            {
                DeleteBookmark(removeQueue.Dequeue());
            }

            return removedItems;
        }

        private IEnumerable<Bookmark> GetAllBookmarksIncludingSubfolders(BookmarkFolder rootFolder)
        {
            var bookmarks = new List<Bookmark>(rootFolder.Bookmarks);

            foreach (var folder in rootFolder.BookmarkFolders)
            {
                bookmarks.AddRange(GetAllBookmarksIncludingSubfolders(folder));
            }

            return bookmarks;
        }

        public void BookmarkDatasourceUpdated()
        {
            BookmarkUpdated(new BookmarkUpdatedEventArgs(BookmarkActions.LoadedNewDataSource, typeof(Bookmark)));
        }

        public void VerifyIntegrityOfBookmarFolder(BookmarkFolder bookmarkfolder)
        {
            try
            {
                var deleteQueue = new Queue<Bookmark>();
                foreach (var bookmark in bookmarkfolder.Bookmarks)
                {
                    if (bookmark.ParentFolderId != bookmarkfolder.Id)
                    {
                        bookmark.ParentFolderId = bookmarkfolder.Id;
                    }

                    if (string.IsNullOrEmpty(bookmark.FileName) || bookmark.Size == 0 || string.IsNullOrEmpty(bookmark.CompletePath))
                    {
                        deleteQueue.Enqueue(bookmark);
                    }
                }

                while (deleteQueue.Count > 0)
                {
                    var bookmark = deleteQueue.Dequeue();
                    DeleteBookmark(bookmark);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "VerifyIntegrityOfBookmarFolder exception");
            }

        }

        public IEnumerable<Bookmark> GetAllBookmarksRecursive(BookmarkFolder rootFolder)
        {
            var bookmarks = rootFolder.Bookmarks;

            foreach (var bookmarkFolder in rootFolder.BookmarkFolders)
            {
                bookmarks.AddRange(GetAllBookmarksRecursive(bookmarkFolder));
            }

            return bookmarks;
        }

        public void UpdateSortOrder(BookmarkFolder selectedBookmarkFolder, string sortBy, SortOrder sortOrder)
        {
            var bookmarks = sortOrder == SortOrder.Ascending ? selectedBookmarkFolder.Bookmarks.OrderBy(o => o.GetType().GetProperty(sortBy)?.GetValue(o, null)).ToList() :
                selectedBookmarkFolder.Bookmarks.OrderByDescending(o => o.GetType().GetProperty(sortBy)?.GetValue(o, null)).ToList();

            for (int i = 0; i < bookmarks.Count; i++)
            {
                bookmarks[i].SortOrder = i;
            }
        }

        private IEnumerable<Bookmark> GetAllBookmarksWithIncorrectPath(BookmarkFolder rootFolder)
        {
            var bookmarkList = new List<Bookmark>();

            foreach (var bookmark in rootFolder.Bookmarks)
            {
                if (!File.Exists(bookmark.CompletePath))
                {
                    bookmarkList.Add(bookmark);
                }
            }

            foreach (var bookmarkFolder in rootFolder.BookmarkFolders)
            {
                bookmarkList.AddRange(GetAllBookmarksWithIncorrectPath(bookmarkFolder));
            }

            return bookmarkList;
        }

        public Task<int> FixBrokenLinksFromBaseDir(string selectedPath)
        {
            var task = Task<int>.Factory.StartNew(() =>
            {
                int filePathsCorrected = 0;
                var brokenLinksList = GetAllBookmarksWithIncorrectPath(RootFolder);

                foreach (var bookmark in brokenLinksList)
                {
                    var fileMatches = Directory.EnumerateFiles(selectedPath, bookmark.FileName, SearchOption.AllDirectories).ToList();

                    if (fileMatches.Any())
                    {
                        foreach (string fileMatch in fileMatches)
                        {
                            FileInfo fileInfo = new FileInfo(fileMatch);
                            if (fileInfo.Length == bookmark.Size)
                            {
                                string dir = GeneralConverters.GetDirectoryNameFromPath(fileMatch);
                                string fileName = GeneralConverters.GetFileNameFromPath(fileMatch);

                                bookmark.Directory = dir;
                                bookmark.FileName = fileName;
                                bookmark.CompletePath = fileMatch;
                                bookmark.CreationTime = fileInfo.CreationTime;
                                bookmark.LastAccessTime = fileInfo.LastAccessTime;
                                bookmark.LastWriteTime = fileInfo.LastWriteTime;
                                filePathsCorrected++;
                                break;
                            }
                        }
                    }
                }

                return filePathsCorrected;
            });

            return task;
        }

        public void ClearBookmarks()
        {
            _bookmarkContainer = CreateBookmarkContainer();
            IsModified = true;
        }
    }
}