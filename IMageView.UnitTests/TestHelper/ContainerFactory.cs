using Autofac;
using System.IO;
using AutofacConfig = ImageView.UnitTests.Configuration.AutofacConfig;

namespace ImageView.UnitTests.TestHelper
{
    public static class ContainerFactory
    {
        private static readonly string TestDirectory = ConfigHelper.TestDataPath;
        public const string CompanyName = "UnitTestForImageViewer";
        public const string ProductName = "ImageViewer";
        public const string ThumbnailIndexFilename = "thumbs.ibd";

        public static IContainer CreateUnitTestContainer()
        {
            IContainer container= AutofacConfig.CreateContainer();

            return container;
        }

        public static string GetTestDirectory()
        {
            return TestDirectory;
        }
    }
}