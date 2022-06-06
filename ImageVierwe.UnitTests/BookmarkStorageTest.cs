﻿using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using ImageView.UnitTests.TestHelper;
using ImageViewer.DataContracts;
using ImageViewer.Managers;
using ImageViewer.Repositories;
using ImageViewer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ImageViewer.Models;

namespace ImageView.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BookmarkStorageTest
    {
        private static ILifetimeScope _lifetimeScope;
        private static IContainer _container;
        private static ImageReference _genericImageRef;
        private static readonly string TestDataPath = Path.Combine(Path.GetTempPath(), "ImageViewerUT");

        [ClassInitialize]
        public static void BookmarkStorageInitialize(TestContext testContext)
        {
            if (!Directory.Exists(TestDataPath))
                Directory.CreateDirectory(TestDataPath);

            GlobalSettings.Settings.UnitTestInitialize(TestDataPath);
            ApplicationBuildConfig.SetOverrideUserDataPath(TestDataPath);

            _genericImageRef = new ImageReference
            {
                Directory = TestDataPath,
                FileName = "testImage.jpg",
                CreationTime = DateTime.Now,
                Size = 1024,
                LastAccessTime = DateTime.Now.Date,
                LastWriteTime = DateTime.Now.Date
            };
            _genericImageRef.CompletePath = Path.Combine(_genericImageRef.Directory, _genericImageRef.FileName);

            var container = ContainerFactory.CreateUnitTestContainer();
            _lifetimeScope = container.BeginLifetimeScope();
        }

        [ClassCleanup]
        public static void BookmarkStorageCleanup()
        {
            var files = Directory.GetFiles(TestDataPath, "*.dat");
            foreach (string filename in files) File.Delete(filename);

            _lifetimeScope.Dispose();
            _container.Dispose();
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MyTestInitialize()
        {
            
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {

        }


        [TestMethod]
        public void AddBookmarkAndReloadFromFile()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var bookmarkService = scope.Resolve<BookmarkService>();
                var bookmarkManager = bookmarkService.BookmarkManager;


                var rootFolder = bookmarkManager.RootFolder;
                Assert.AreEqual("Root", rootFolder.Name, "Invalid Root folder name");

                bookmarkManager.AddBookmark(rootFolder.Id, "TestImageBookmark", _genericImageRef);
                bool saveSuccessful = bookmarkService.SaveBookmarks();
                Assert.IsTrue(saveSuccessful, "Saving bookmarks data file failed!");

                bookmarkService.OpenBookmarks();

                var bookmark = bookmarkManager.RootFolder.Bookmarks.ToList().FirstOrDefault();
                bool areEqual = CompareBookmarkToImgRef(bookmark, _genericImageRef);

                Assert.IsTrue(areEqual, "The loaded bookmark was not identical to the saved bookmark");
            }
        }

        [TestMethod]
        public void AddBookmarkFolder()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var bookmarkManager = scope.Resolve<BookmarkManager>();
                var rootFolder = bookmarkManager.RootFolder;
                var folder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "TestFolder");
                Assert.IsNotNull(folder, "Failed to add Folder");

                Assert.IsNotNull(folder.Id, "Folder id was null!");
                Assert.IsNotNull(folder.ParentFolderId, "Parent Folder id was null!");
                Assert.IsNotNull(folder.BookmarkFolders, "BookmarkFolder list was null!");
                Assert.IsNotNull(folder.Bookmarks, "BookmarkList was null!");
                Assert.AreEqual(bookmarkManager.RootFolder.BookmarkFolders.FirstOrDefault(), folder, "Added folder was not equal to item in collection");
            }
        }

        [TestMethod]
        public void InsertBookmarkFolder()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var bookmarkManager = scope.Resolve<BookmarkManager>();
                var rootFolder = bookmarkManager.RootFolder;
                var folder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder1");
                Assert.IsNotNull(folder, "Failed to add Folder");
                folder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder3");
                Assert.IsNotNull(folder, "Failed to add Folder");

                folder = bookmarkManager.InsertBookmarkFolder(rootFolder.Id, "Folder2", 1);
                Assert.IsNotNull(folder, "Failed to insert Folder");

                folder = bookmarkManager.RootFolder.BookmarkFolders.SingleOrDefault(f => f.SortOrder == 1 && f.Name == "Folder2");
                Assert.IsNotNull(folder, "Could not find inserted item in collection!");

                Assert.IsNotNull(folder.BookmarkFolders, "Inserted item Folder list is null!");
                Assert.IsNotNull(folder.Bookmarks, "Inserted item Bookmark list is null!");
                Assert.IsNotNull(folder.Id, "Inserted item Id is null!");
                Assert.IsNotNull(folder.Name, "Inserted item Name is null!");
                Assert.IsNotNull(folder.ParentFolderId, "Inserted item ParentFolderId is null!");
            }
        }

        [TestMethod]
        public void InsertBookmark()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var bookmarkManager = scope.Resolve<BookmarkManager>();
                var rootFolder = bookmarkManager.RootFolder;
                var bookmark1 = bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark1", _genericImageRef);
                Assert.IsNotNull(bookmark1, "Failed to add Bookmark1");
                var bookmark3 = bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark3", _genericImageRef);
                Assert.IsNotNull(bookmark3, "Failed to add Bookmark3");
                var bookmark2 = bookmarkManager.InsertBookmark(rootFolder.Id, "Bookmark2", _genericImageRef, 1);
                Assert.IsNotNull(bookmark2, "Failed to insert Bookmark2");

                var insertedItem = bookmarkManager.RootFolder.Bookmarks.SingleOrDefault(f => f.SortOrder == 1 && f.BoookmarkName == "Bookmark2");
                Assert.IsNotNull(insertedItem, "Could not find inserted item in collection!");
                Assert.IsTrue(CompareBookmarkToImgRef(insertedItem, _genericImageRef), "The inserted bookmark was not identical to the reference bookmark");

                scope.Dispose();
            }
        }

        [TestMethod]
        public void TestDeleteBookmarkFolderById()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var bookmarkManager = scope.Resolve<BookmarkManager>();
                var rootFolder = bookmarkManager.RootFolder;
                var bookmarkFolder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "Folder1");
                string id = bookmarkFolder.Id;

                Assert.IsTrue(bookmarkManager.DeleteBookmarkFolderById(id), "Failed to delete bookmark folder!");
                Assert.IsTrue(rootFolder.BookmarkFolders.Count == 0, "Bookmark folder was not deleted!");

                bookmarkFolder = bookmarkManager.AddBookmarkFolder(rootFolder.Id, "FolderLevel1");
                var bookmarkSubFolder = bookmarkManager.AddBookmarkFolder(bookmarkFolder.Id, "FolderLevel2");
                var bookmark = bookmarkManager.AddBookmark(bookmarkSubFolder.Id, "TestBookmark", _genericImageRef);
                Assert.IsNotNull(bookmark, "Failed to add bookmark to subfolder");

                Assert.IsTrue(bookmarkManager.DeleteBookmarkFolderById(bookmarkFolder.Id), "Failed to delete bookmark folder!");

                Assert.IsTrue(rootFolder.BookmarkFolders.Count == 0, "Not all bookmark folders where deleted!");
            }
        }

        [TestMethod]
        public void TestDeleteBookmarkByFilename()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var bookmarkManager = scope.Resolve<BookmarkManager>();
                var rootFolder = bookmarkManager.RootFolder;
                var bookmark1 = bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark1", _genericImageRef);
                Assert.IsTrue(bookmarkManager.DeleteBookmarkByFilename(bookmark1.ParentFolderId, bookmark1.FileName), "Failed to delete bookmark!");
                Assert.IsTrue(rootFolder.Bookmarks.Count == 0, "Bookmarks should be empty!");
            }
        }

        [TestMethod]
        public void TestIsModified()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var bookmarkService = scope.Resolve<BookmarkService>();
                bookmarkService.BookmarkManager.ClearBookmarks();
                var bookmarkManager = bookmarkService.BookmarkManager;
                var rootFolder = bookmarkManager.RootFolder;
                

                Assert.IsFalse(bookmarkManager.IsModified, "BookmarkManager should not be modified");

                bookmarkManager.AddBookmark(rootFolder.Id, "Bookmark1", _genericImageRef);
                Assert.IsTrue(bookmarkManager.IsModified, "BookmarkManager should be modified");
                bookmarkService.SaveBookmarks();
                Assert.IsFalse(bookmarkManager.IsModified, "BookmarkManager should not be modified");
            }
        }

        private ApplicationSettingsService GetApplicationSettingsService()
        {
            var appSettings = AppSettingsRepository.GetDefaultApplicationSettings();
            appSettings.DefaultKey = "EkNxG2vH27y4xezfzyEJpHGenOtgLJ1x";
            var appSettingsRepository = Substitute.For<AppSettingsRepository>();
            var settingsService = Substitute.For<ApplicationSettingsService>(appSettingsRepository);



            //MediaTypeNames.Application.CompanyName.Returns("Cuplex");

            //ApplicationSettingsService.CompanyName.Returns("Cuplex");

            //MediaTypeNames.Application.ProductName.Returns("ImageViewer");

            settingsService.Settings.Returns(appSettings);

            return settingsService;
        }

        private bool CompareBookmarkToImgRef(Bookmark bookmark, ImageReference imageReference)
        {
            return imageReference.Size == bookmark.Size &&
                   imageReference.CompletePath == bookmark.CompletePath &&
                   imageReference.CreationTime == bookmark.CreationTime &&
                   imageReference.Directory == bookmark.Directory &&
                   imageReference.FileName == bookmark.FileName &&
                   imageReference.LastAccessTime == bookmark.LastAccessTime &&
                   imageReference.LastWriteTime == bookmark.LastWriteTime;
        }
    }
}