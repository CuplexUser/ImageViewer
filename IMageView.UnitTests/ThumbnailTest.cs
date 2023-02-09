using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using ImageView.UnitTests.TestHelper;
using ImageViewer.Models;
using ImageViewer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ImageView.UnitTests.Properties;


namespace ImageView.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ThumbnailTest
    {
        private static readonly string[] TestImages = { "testImg.jpg", "testImg2.jpg", "testImg3.jpg" };
        private static ILifetimeScope _lifetimeScope;
        private static IContainer _container;
        private static ImageReference _genericImageRef;
        private static readonly string TestDataPath = Path.Combine(Path.GetTempPath(), "ImageViewerUT");
        private static TestContext _testContext;

        [ClassInitialize]
        public static void TestClassInit(TestContext testContext)
        {
            if (!Directory.Exists(TestDataPath))
                Directory.CreateDirectory(TestDataPath);

            GlobalSettings.Settings.UnitTestInitialize(TestDataPath);
            ApplicationBuildConfig.SetOverrideUserDataPath(TestDataPath);

            _testContext = testContext;
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
            CreateTestImages();
        }

        private static void CreateTestImages()
        {
            // Write test images to test-data folder
            var imageData = Resources.Hosico1;
            string testImagePath = Path.Join(TestDataPath, TestImages[0]);
            imageData.Save(testImagePath);

            imageData = Resources.Hosico_Cat_80;
            testImagePath = Path.Join(TestDataPath, TestImages[1]);
            imageData.Save(testImagePath);

            imageData = Resources.ficklampa1;
            testImagePath = Path.Join(TestDataPath, TestImages[2]);
            imageData.Save(testImagePath);

        }

        [ClassCleanup]
        public static void TestClassCleanup()
        {
            ClearTestDirectory();
            _lifetimeScope.Dispose();
            _container.Dispose();
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
            try
            {
                string[] files = Directory.GetFiles(TestDataPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                _testContext.WriteLine("Test framework Exception!");
                _testContext.WriteLine(ex.Message);
            }

        }

        [TestMethod]
        public void ThumbnailScanDirectory()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                GlobalSettings.Settings.UnitTestInitialize("ImageViewTest");

                var thumbnailService = scope.Resolve<ThumbnailService>();

                bool result = thumbnailService.ThumbnailDirectoryScan(TestDataPath, new Progress<ThumbnailScanProgress>(), false).Result;

                var thumbNailImage = thumbnailService.GetThumbnail(TestDataPath + TestImages[0]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 1 was null");

                thumbNailImage = thumbnailService.GetThumbnail(TestDataPath + TestImages[1]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 2 was null");

                thumbNailImage = thumbnailService.GetThumbnail(TestDataPath + TestImages[2]);
                Assert.IsNotNull(thumbNailImage, "Thumbnail image 3 was null");
            }
        }

        [TestMethod]
        public void ThumbnailLoadDatabase()
        {
            using var scope = _lifetimeScope.BeginLifetimeScope();
            string fileName = Path.Combine(TestDataPath, "thumbnail.db");

            // Test database is already created by TestClassInit
            var thumbnailService = scope.Resolve<ThumbnailService>();

            // save Database first
            thumbnailService.ClearDatabase();
            bool saveResult = thumbnailService.SaveThumbnailDatabase().Result;

            // Verify return value
            Assert.IsTrue(saveResult, "SaveThumbnailDatabase was not successful");

            // Verify that the db file exists
            Assert.IsTrue(File.Exists(fileName), $"Database file does not exist at {fileName}");

            bool result = thumbnailService.LoadThumbnailDatabaseAsync().Result;
            Assert.IsTrue(result, "Load thumbnail database failed");
        }

        [TestMethod]
        public void ThumbnailOptimizeDatabaseAfterFileRemoval()
        {
            using (var scope = _lifetimeScope.BeginLifetimeScope())
            {
                var thumbnailService = scope.Resolve<ThumbnailService>();
                // Create thumbnails from TestImages
                foreach (string testImage in TestImages)
                {
                    string filePath = Path.Join(TestDataPath, testImage);

                    // Also includes thumbnail caching
                    var thumbnail = thumbnailService.GetThumbnail(filePath);
                }

                // Verify that there are testImages.Length thumbnails created
                Assert.IsTrue(thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain the right amount of images");

                //Remove the first file
                File.Delete(TestDataPath + TestImages[0]);

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
                var fs = File.OpenWrite(TestDataPath + TestImages[0]);
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