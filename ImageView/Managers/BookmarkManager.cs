﻿using GeneralToolkitLib.Converters;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageViewer.DataContracts;
using ImageViewer.Events;
using ImageViewer.Models;
using ImageViewer.Utility;

namespace ImageViewer.Managers;

[UsedImplicitly]
public class BookmarkManager : ManagerBase
{
    private BookmarkContainer _bookmarkContainer;

    public BookmarkManager()
    {
        _bookmarkContainer = CreateBookmarkContainer();
        BookmarkManagerId = Guid.NewGuid();
        IsModified = false;
    }

    public Guid BookmarkManagerId { get; init; }

    public BookmarkFolder RootFolder => _bookmarkContainer.RootFolder;

    public bool IsModified { get; private set; }

    public bool LoadedFromFile { get; private set; }

    public event BookmarkUpdatedEventHandler OnBookmarksUpdate;

    protected void BookmarkUpdated(object sender, BookmarkUpdatedEventArgs e)
    {
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
                string copyFilename = GeneralConverters.GetDirectoryNameFromPath(filename) + "BookmarksCopy.dat";
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
            Log.Error(ex, "BookmarkManager.SaveToFile(string filename, string password) : {Message}", ex.Message);
            return false;
        }
    }

    public bool LoadFromFile(string filename, string password)
    {
        try
        {
            BookmarkContainer bookmarkContainer;
            LoadedFromFile = true;
            var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
            var storageManager = new StorageManager(settings);

            try
            {
                bookmarkContainer = storageManager.DeserializeObjectFromFile<BookmarkContainer>(filename, null);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to load bookmark file");
                bookmarkContainer = CreateBookmarkContainer();
            }

            if (bookmarkContainer?.RootFolder == null || string.IsNullOrEmpty(bookmarkContainer.ContainerId))
            {
                return false;
            }

            int changesMade = PrepareContainer(bookmarkContainer);
            _bookmarkContainer = bookmarkContainer;
            if (changesMade > 0)
            {
                Log.Information("Loaded Bookmarks file which had {changesMade} number of issues like faulty links. Re-saving fixed file", changesMade);
                SaveToFile(filename, password);
            }

            IsModified = true;

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "BookmarkManager.LoadFromFile(string filename, string password) : {Message}", ex.Message);
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

            IsModified = true;
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "BookmarkManager.LoadFromFile(string filename, string password) : {Message}", ex.Message);
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
            if (bookmark.ParentFolderId != folderId)
            {
                bookmark.ParentFolderId = folderId;
                changes++;
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
            if (!source.Bookmarks.Any(x => x.CompletePath == bookmark.CompletePath && x.Size == bookmark.Size))
            {
                bookmark.ParentFolderId = source.Id;
                source.Bookmarks.Add(bookmark);
                IsModified = true;
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
            for (int i = 0; i < folderList.Count; i++) folderList[i].SortOrder = i + 1;

            _bookmarkContainer.RootFolder.BookmarkFolders.Sort((f1, f2) => f1.SortOrder.CompareTo(f2.SortOrder));
        }

        if (reIndexBookmarks)
        {
            var bookmarkList = _bookmarkContainer.RootFolder.Bookmarks.OrderBy(f => f.SortOrder).ToList();
            for (int i = 0; i < bookmarkList.Count; i++) bookmarkList[i].SortOrder = i + 1;

            _bookmarkContainer.RootFolder.Bookmarks.Sort((b1, b2) => b1.SortOrder.CompareTo(b2.SortOrder));
        }
    }

    public BookmarkFolder AddBookmarkFolder(string parentId, string folderName)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentId);
        if (parentFolder == null)
        {
            return null;
        }

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
        OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof(BookmarkFolder)));

        return folder;
    }

    public BookmarkFolder InsertBookmarkFolder(string parentId, string folderName, int index)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentId);
        if (parentFolder == null)
        {
            return null;
        }

        if (index < 0 || index > parentFolder.BookmarkFolders.Count)
        {
            return null;
        }

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
        foreach (var item in postItems) item.SortOrder = item.SortOrder + 1;

        parentFolder.BookmarkFolders.Add(folder);
        parentFolder.BookmarkFolders.Sort((f1, f2) => f1.SortOrder.CompareTo(f2.SortOrder));
        OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmarkFolder, typeof(BookmarkFolder)));

        return folder;
    }

    public Bookmark AddBookmark(string parentFolderId, string bookmarkName, ImageReference imgRef)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
        if (parentFolder == null)
        {
            return null;
        }

        var bookmark = new Bookmark
        {
            BoookmarkName = bookmarkName,
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
        IsModified = true;

        OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof(Bookmark)));
        return bookmark;
    }

    public Bookmark InsertBookmark(string parentFolderId, string bookmarkName, ImageReference imgRef, int index)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
        if (parentFolder == null)
        {
            return null;
        }

        if (index < 0 || index > parentFolder.Bookmarks.Count)
        {
            return null;
        }

        var bookmark = new Bookmark
        {
            BoookmarkName = bookmarkName,
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
        foreach (var item in postItems) item.SortOrder = item.SortOrder + 1;

        parentFolder.Bookmarks.Add(bookmark);
        parentFolder.Bookmarks.Sort((b1, b2) => b1.SortOrder.CompareTo(b2.SortOrder));
        OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.CreatedBookmark, typeof(Bookmark)));

        return bookmark;
    }

    public bool MoveBookmark(Bookmark bookmark, string destinationFolderId)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmark.ParentFolderId);
        var destinationFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, destinationFolderId);

        if ((parentFolder == null) | (destinationFolder == null) || parentFolder == destinationFolder)
        {
            return false;
        }

        parentFolder.Bookmarks.Remove(bookmark);
        destinationFolder.Bookmarks.Add(bookmark);
        bookmark.ParentFolderId = destinationFolder.Id;

        return true;
    }

    public bool DeleteBookmark(Bookmark bookmark)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmark.ParentFolderId);

        if (parentFolder == null)
        {
            return false;
        }

        bool success = parentFolder.Bookmarks.Remove(bookmark);
        if (success)
        {
            ReindexSortOrder(false, true);
            OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmark, typeof(Bookmark)));
        }

        return success;
    }

    public bool DeleteBookmarkByFilename(string parentFolderId, string fileName)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, parentFolderId);
        var bookmarkToDelete = parentFolder?.Bookmarks.FirstOrDefault(bookmark => bookmark.FileName == fileName);

        if (bookmarkToDelete == null)
        {
            return false;
        }

        bool success = parentFolder.Bookmarks.Remove(bookmarkToDelete);
        ReindexSortOrder(false, true);
        OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmark, typeof(Bookmark)));

        return success;
    }


    public bool DeleteBookmarkFolder(BookmarkFolder folder)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, folder.ParentFolderId);
        if (parentFolder == null)
        {
            return false;
        }

        bool success = parentFolder.BookmarkFolders.Remove(folder);
        if (success)
        {
            ReindexSortOrder(false, true);
            OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmarkFolder, typeof(Bookmark)));
        }

        return success;
    }

    public bool DeleteBookmarkFolderById(string folderId)
    {
        var bookmarkFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, folderId);

        if (bookmarkFolder?.ParentFolderId == null)
        {
            return false;
        }

        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmarkFolder.ParentFolderId);
        bool result = parentFolder.BookmarkFolders.Remove(bookmarkFolder);
        ReindexSortOrder(true, false);
        if (result)
        {
            OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmarkFolder, typeof(Bookmark)));
        }

        return result;
    }

    private static BookmarkFolder GetBookmarkFolderById(BookmarkFolder rootFolder, string id)
    {
        BookmarkFolder subFolder = null;
        if (rootFolder.Id == id)
        {
            return rootFolder;
        }

        foreach (var bookmarkFolder in rootFolder.BookmarkFolders)
        {
            if (bookmarkFolder.Id == id)
            {
                return bookmarkFolder;
            }

            if (bookmarkFolder.BookmarkFolders != null && bookmarkFolder.BookmarkFolders.Count > 0)
            {
                subFolder = GetBookmarkFolderById(bookmarkFolder, id);
            }

            if (subFolder != null)
            {
                return subFolder;
            }
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
            if (duplicateGroup.Count() > 1)
            {
                var duplicateList = duplicateGroup.OrderBy(x => x.ParentFolderId).ToList();

                var itemToKeep = duplicateList.FirstOrDefault(x => x.ParentFolderId != rootFolder.Id) ?? duplicateList.First();
                duplicateList.Remove(itemToKeep);

                foreach (var bookmark in duplicateList) removeQueue.Enqueue(bookmark);
            }

        int removedItems = removeQueue.Count;

        while (removeQueue.Count > 0) DeleteBookmarkQuick(removeQueue.Dequeue());

        if (removedItems > 0)
        {
            ReindexSortOrder(false, true);
            OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.DeletedBookmark, typeof(Bookmark)));
        }

        return removedItems;
    }

    private bool DeleteBookmarkQuick(Bookmark bookmark)
    {
        var parentFolder = GetBookmarkFolderById(_bookmarkContainer.RootFolder, bookmark.ParentFolderId);

        if (parentFolder == null)
        {
            return false;
        }

        return parentFolder.Bookmarks.Remove(bookmark);
    }

    private static IEnumerable<Bookmark> GetAllBookmarksIncludingSubfolders(BookmarkFolder rootFolder)
    {
        var bookmarks = new List<Bookmark>(rootFolder.Bookmarks);

        foreach (var folder in rootFolder.BookmarkFolders) bookmarks.AddRange(GetAllBookmarksIncludingSubfolders(folder));

        return bookmarks;
    }

    public void BookmarkDatasourceUpdated()
    {
        OnBookmarksUpdate?.Invoke(this, new BookmarkUpdatedEventArgs(BookmarkActions.LoadedNewDataSource, typeof(Bookmark)));
    }

    public void VerifyIntegrityOfBookmarkFolder(BookmarkFolder folder)
    {
        try
        {
            var deleteQueue = new Queue<Bookmark>();
            foreach (var bookmark in folder.Bookmarks)
            {
                if (bookmark.ParentFolderId != folder.Id)
                {
                    bookmark.ParentFolderId = folder.Id;
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
            Log.Error(ex, "VerifyIntegrityOfBookmarkFolder exception");
        }
    }

    public IEnumerable<Bookmark> GetAllBookmarksRecursive(BookmarkFolder rootFolder)
    {
        var bookmarks = rootFolder.Bookmarks;

        foreach (var bookmarkFolder in rootFolder.BookmarkFolders) bookmarks.AddRange(GetAllBookmarksRecursive(bookmarkFolder));

        return bookmarks;
    }

    public static void UpdateSortOrder(BookmarkFolder selectedBookmarkFolder, string sortBy, SortOrder sortOrder)
    {
        var bookmarks = sortOrder == SortOrder.Ascending
            ? selectedBookmarkFolder.Bookmarks.OrderBy(o => o.GetType().GetProperty(sortBy)?.GetValue(o, null)).ToList()
            : selectedBookmarkFolder.Bookmarks.OrderByDescending(o => o.GetType().GetProperty(sortBy)?.GetValue(o, null)).ToList();

        for (int i = 0; i < bookmarks.Count; i++) bookmarks[i].SortOrder = i;
    }

    private static IEnumerable<Bookmark> GetAllBookmarksWithIncorrectPath(BookmarkFolder rootFolder)
    {
        var bookmarkList = rootFolder.Bookmarks.Where(bookmark => !File.Exists(bookmark.CompletePath)).ToList();

        foreach (var bookmarkFolder in rootFolder.BookmarkFolders) bookmarkList.AddRange(GetAllBookmarksWithIncorrectPath(bookmarkFolder));

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

                if (!fileMatches.Any())
                {
                    continue;
                }

                foreach (string fileMatch in fileMatches)
                {
                    var fileInfo = new FileInfo(fileMatch);
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

            return filePathsCorrected;
        });

        return task;
    }

    public void ClearBookmarks()
    {
        _bookmarkContainer = CreateBookmarkContainer();
        IsModified = false;
    }

    public override string ToString()
    {
        return BookmarkManagerId.ToString();
    }
}