using System.Diagnostics.CodeAnalysis;
using Autofac;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using ImageView.UnitTests.Properties;
using ImageView.UnitTests.TestHelper;
using ImageViewer.Models;
using ImageViewer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ImageView.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class ThumbnailTest
{
    private static readonly string[] TestImages = ["testImg.jpg", "testImg2.jpg", "testImg3.jpg"];
    private static ILifetimeScope _lifetimeScope;
    private static IContainer _container;
    private static ImageReference _genericImageRef;
    private static readonly string TestDataPath = Path.Combine(Path.GetTempPath(), "ImageViewerUT");
    private static TestContext _testContext;
    private ThumbnailService _thumbnailService;
    private ILifetimeScope _individualTestScope;

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
        _individualTestScope = _lifetimeScope.BeginLifetimeScope(_testContext);
        _thumbnailService = _individualTestScope.Resolve<ThumbnailService>();

    }

    [TestCleanup]
    public void ThumbnailTestCleanup()
    {
        _individualTestScope.Dispose();
        _individualTestScope = null;
    }

    private static void ClearTestDirectory()
    {
        try
        {
            string[] files = Directory.GetFiles(TestDataPath);
            foreach (string file in files) File.Delete(file);
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
        GlobalSettings.Settings.UnitTestInitialize("ImageViewTest");

        var thumbnailService = _thumbnailService;
        
        // Ensure that TestDataPath exists and contains test images
        Assert.IsTrue(Directory.Exists(TestDataPath), "TestDataPath does not exist");

        var fsInfo = Directory.GetFileSystemEntries(TestDataPath, "*.jpg");
        Assert.IsTrue(fsInfo.Length == TestImages.Length, "TestDataPath does not contain all test images");

        // Scan directory   
        var scanTask = thumbnailService.ThumbnailDirectoryScan(TestDataPath, new Progress<ThumbnailScanProgress>(), true, CancellationToken.None);
        Task.WaitAll(scanTask);
        
        // Verify that the scan task returned true
        Assert.IsTrue(scanTask.Result, "ThumbnailDirectoryScan() did not return true as expected");
        
        // Verify that the first image actually exists in the thumbnail cache
        bool cached = thumbnailService.CheckIfCached(Path.Join(TestDataPath, TestImages[0]));
        Assert.IsTrue(cached, "Image 1 was not cached");    

        var thumbNailImage = thumbnailService.GetThumbnailAsync(Path.Join(TestDataPath, TestImages[0])).GetAwaiter().GetResult();
        Assert.IsNotNull(thumbNailImage, "Thumbnail image 1 was null");

        thumbNailImage = thumbnailService.GetThumbnailAsync(Path.Join(TestDataPath, TestImages[1])).GetAwaiter().GetResult();
        Assert.IsNotNull(thumbNailImage, "Thumbnail image 2 was null");

        thumbNailImage = thumbnailService.GetThumbnailAsync(Path.Join(TestDataPath, TestImages[2])).GetAwaiter().GetResult();
        Assert.IsNotNull(thumbNailImage, "Thumbnail image 3 was null");
    }

    [TestMethod("SaveDatabaseTest")]
    public void ThumbnailSaveDatabase()
    {
        string fileName = Path.Combine(TestDataPath, "thumbnail.db");

        
        _thumbnailService.ClearDatabase();
        var saveResult = _thumbnailService.SaveThumbnailDatabase();
        

        string dbFilePath = _thumbnailService.GetThumbnailDbFilePath();

        // Verify that the Save Path is equal to the Load Path for the Database DB file.
        Assert.IsTrue(fileName.Equals(dbFilePath, StringComparison.CurrentCultureIgnoreCase), "Thumbnail Database save Path is incorrect");

        // Verify return value
        Assert.IsTrue(saveResult, "SaveThumbnailDatabaseAsync was not successful");

        // Verify that the db file exists
        Assert.IsTrue(File.Exists(fileName), $"Database file does not exist at {fileName}");



        bool result = _thumbnailService.LoadThumbnailDatabase();
        Assert.IsTrue(result, "Load thumbnail database failed");
    }

    [TestMethod("LoadDatabaseTest")]
    public void ThumbnailLoadDatabase()
    {
        string fileName = Path.Combine(TestDataPath, "thumbnail.db");

        // save Database first
        _thumbnailService.ClearDatabase();
        var saveResult = _thumbnailService.SaveThumbnailDatabase();
        

        string dbFilePath = _thumbnailService.GetThumbnailDbFilePath();

        // Verify that the Save Path is equal to the Load Path for the Database DB file.
        Assert.IsTrue(fileName.Equals(dbFilePath, StringComparison.CurrentCultureIgnoreCase), "Thumbnail Database save Path is incorrect");

        // Verify return value
        Assert.IsTrue(saveResult, "SaveThumbnailDatabaseAsync was not successful");

        bool result = _thumbnailService.LoadThumbnailDatabase();
        Assert.IsTrue(result, "Load thumbnail database failed");
    }

    [TestMethod("Optimize_AddRemove")]
    public void ThumbnailOptimizeDatabaseAfterFileRemoval()
    {
        var scope = _lifetimeScope.BeginLifetimeScope();
        var thumbnailService = scope.Resolve<ThumbnailService>();

        // Create thumbnails from TestImages
        foreach (string testImage in TestImages)
        {
            string filePath = Path.Join(TestDataPath, testImage);

            // Also includes thumbnail caching
            var thumbnail = thumbnailService.GetThumbnailAsync(filePath).Result;
            Assert.IsNotNull(thumbnail,"Failed to create thumbnail");
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

    [TestMethod("Optimize_Update")]
    public void ThumbnailOptimizeDatabaseAfterFileUpdated()
    {
        var thumbnailList = new List<Image>();


        foreach (string testImage in TestImages)
        {
            string filename = Path.Join(TestDataPath, testImage);
            var img = _thumbnailService.GetThumbnail(filename,1000);
            if (img != null)
                thumbnailList.Add(img);
            else
                Assert.Fail($"Returned thumbnail for image '{filename}' was null");
        }

        Assert.IsTrue(thumbnailList.Count == TestImages.Length, "thumbnailList.Count == TestImages.Length");
        bool saveResult = _thumbnailService.SaveThumbnailDatabase();

        Assert.IsTrue(saveResult, "SaveThumbnailDatabaseAsync() did not return true as expected");

        // Verify that there are testImages.Length thumbnails created
        Assert.IsTrue(_thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length, "The thumbnail cache did not contain the right amount of images");

        FileStream fs = null;
        try
        {
            //Modify the first file
            fs = File.OpenWrite(TestDataPath + TestImages[0]);
            var buffer = new byte[1];
            buffer[0] = 0xff;
            fs.Write(buffer, 0, 1);
            fs.Flush(true);
        }
        catch
        {
            Assert.Fail("Could not modify file");
        }
        finally
        {
            fs?.Close();
        }


        // Optimize DB
        Task.Factory.FromAsync(_thumbnailService.OptimizeDatabaseAsync(), delegate { });

        // Verify that one thumbnail was removed
        Assert.IsTrue(_thumbnailService.GetNumberOfCachedThumbnails() == TestImages.Length - 1, "The thumbnail service did not remove a cached item");
    }
}