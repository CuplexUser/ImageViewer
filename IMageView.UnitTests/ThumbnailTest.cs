using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using ImageView.UnitTests.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using ImageViewer.Models;
using ImageViewer.Services;


namespace ImageView.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThumbnailTest
    {
        private static readonly string TestDirectory = ContainerFactory.GetTestDirectory();
        private static readonly string[] TestImages = { "testImg.jpg", "testImg2.jpg", "testImg3.jpg" };
        private static ILifetimeScope _lifetimeScope;
        private static IContainer _container;
        private static ImageReference _genericImageRef;
        private static readonly string TestDataPath = Path.Combine(Path.GetTempPath(), "ImageViewerUT");

        [ClassInitialize]
        public static void TestClassInit(TestContext testContext)
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

            _container = ContainerFactory.CreateUnitTestContainer();
            _lifetimeScope = _container.BeginLifetimeScope();


        }

        [ClassCleanup]
        public static void TestClassCleanup()
        {
            _lifetimeScope.Dispose();
            _container.Dispose();
            ClearTestDirectory();
        }

        [TestInitialize]
        public void ThumbnailTestInitialize()
        {

        }

        [TestCleanup]
        public void ThumbnailTestCleanup()
        {

        }

        private static void ClearTestDirectory()
        {
            var files = Directory.GetFiles(TestDirectory);
            foreach (string file in files)
            {
                File.Delete(file);
            }
        }

        [TestMethod]
        public void ThumbnailScanDirectory()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                GlobalSettings.Settings.UnitTestInitialize("ImageViewTest");

                var thumbnailService = scope.Resolve<ThumbnailService>();

                bool result = thumbnailService.ThumbnailDirectoryScan(TestDirectory, new Progress<ThumbnailScanProgress>(), false).Result;

                var thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[0]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 1 was null");

                thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[1]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 2 was null");

                thumbNailImage = thumbnailService.GetThumbnail(TestDirectory + TestImages[2]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 3 was null");
            }
        }

        [TestMethod]
        public void ThumbnailLoadDatabase()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                string fileName = Path.Combine(ApplicationBuildConfig.UserDataPath, "thumbs.db");

                // Test database is already created by TestClassInit
                var thumbnailService = scope.Resolve<ThumbnailService>();

                // save Database first
                thumbnailService.ClearDatabase();
                thumbnailService.SaveThumbnailDatabase();

                // Verify that the db file exists
                Assert.IsTrue(File.Exists(fileName), $"Database file does not exist at {fileName}");

                bool result = thumbnailService.LoadThumbnailDatabase().Result;
                Assert.IsTrue(result, "Load thumbnail database failed");
                Assert.AreEqual(thumbnailService.GetNumberOfCachedThumbnails(), 3, "Database did not contain 3 items");
            }
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileRemoval()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var thumbnailService = scope.Resolve<ThumbnailService>();
                // Verify that there are testImages.Length thumbnails created
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain the right amount of images");

                //Remove the first file
                File.Delete(TestDirectory + TestImages[0]);

                // Optimize DB
                Task.WaitAll(thumbnailService.OptimizeDatabaseAsync());

                // Verify that one thumbnail was removed
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");
            }
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileUpdated()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var thumbnailService = scope.Resolve<ThumbnailService>();

                // Verify that there are testImages.Length thumbnails created
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain the right amount of images");

                //Modify the first file
                var fs = File.OpenWrite(TestDirectory + TestImages[0]);
                var buffer = new byte[1];
                buffer[0] = 0xff;
                fs.Write(buffer, 0, 1);
                fs.Flush(true);
                fs.Close();

                // Optimize DB
                Task.Factory.FromAsync(thumbnailService.OptimizeDatabaseAsync(), delegate (IAsyncResult result) { });

                // Verify that one thumbnail was removed
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");
            }
        }

        private ThumbnailService GetThumbnailService()
        {
            return _lifetimeScope.Resolve<ThumbnailService>();
        }
    }
}