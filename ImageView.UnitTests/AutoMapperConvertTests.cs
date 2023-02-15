using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using GeneralToolkitLib.ConfigHelper;
using GeneralToolkitLib.Configuration;
using ImageView.UnitTests.TestHelper;
using ImageViewer.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using ImageViewer.Models;

namespace ImageView.UnitTests;

public class AutoMapperConvertTests
{
    private readonly ILifetimeScope _lifetimeScope;
    private static TestContext _context;
    private static readonly string TestDataPath = Path.Combine(Path.GetTempPath(), "ImageViewerUT");
    private readonly string _blobDbPath;
    private readonly IMapper _mapper;
    private List<ImageReference> _imageRefList;
    private static TestDataFactory _dataFactory;


    [ClassInitialize]
    public static void InitTestClass(TestContext context)
    {
        _context = context;
        if (!Directory.Exists(TestDataPath))
            Directory.CreateDirectory(TestDataPath);

        GlobalSettings.Settings.UnitTestInitialize(TestDataPath);
        ApplicationBuildConfig.SetOverrideUserDataPath(TestDataPath);


        _dataFactory = new TestDataFactory(context);

    }
    [ClassCleanup]
    public static void TestClassCleanup()
    {
        

    }

    public AutoMapperConvertTests()
    {
        IContainer testContainer = ContainerFactory.CreateUnitTestContainer();
        _lifetimeScope = testContainer.BeginLifetimeScope();
        _mapper = _lifetimeScope.Resolve<IMapper>();

        _blobDbPath = Path.Join(TestDataPath, "binaryBlobData.bin");
    }

    [TestInitialize]
    public void TestInit()
    {
        _imageRefList = _dataFactory.BuildTestImageList();
    }

    [TestCleanup]
    public void CleanupUnitTestResources()
    {
        if (File.Exists(_blobDbPath))
        {
            File.Delete(_blobDbPath);
        }
    }

    [TestMethod("SelfTest-Environment-Sasnity-Check")]
    public void CoreEnvironmentSetupSanityTest()
    {
        // Verify Test data generated as expected
        Assert.IsNotNull(_imageRefList, "_imageRefList != null");
        int length = _imageRefList.Count;
        var item = _imageRefList.FirstOrDefault();

        Assert.IsTrue(length==7,"");

    }

    [TestMethod("TestModel-To-DataModel-Mapping-ThumbnailMetadataDbModel")]
    public void TestModelToDataModelMapping()
    {
        var model = ThumbnailMetadataDbModel.CreateModel(_blobDbPath);


        Assert.IsNotNull(model,"model != null");
        Assert.IsNotNull(model.ThumbnailEntries,"model.ThumbnailEntries != null");
        Assert.IsNotNull(model.DatabaseId);
        Assert.IsNotNull(model.BinaryBlobFilename);
        Assert.IsTrue(model.CreatedDate!=DateTime.MinValue, "model.CreatedDate!=DateTime.MinValue");
        Assert.IsNotNull(model.ThumbnailEntries);

    }
}