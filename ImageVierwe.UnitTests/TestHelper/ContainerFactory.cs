using Autofac;
using System.IO;
using AutofacConfig = ImageView.UnitTests.Configuration.AutofacConfig;

namespace ImageView.UnitTests.TestHelper
{
    public static class ContainerFactory
    {
        private static readonly string TestDirectory = Path.Combine(Path.GetTempPath(), "ImageViewTestdata");
        public const string CompanyName = "UnitTestForImageViewer";
        public const string ProductName = "ImageViewer";
        public const string ThumbnailIndexFilename = "thumbs.ibd";

        public static IContainer CreateUnitTestContainer()
        {
            var builder = new ContainerBuilder();
            var unitTestAssembly = AutofacConfig.GetUnitTestAssembly();
            builder.RegisterAssemblyModules(unitTestAssembly);

            var container = builder.Build();

            return container;

        }

        public static string GetTestDirectory()
        {
            return TestDirectory;
        }
    }
}