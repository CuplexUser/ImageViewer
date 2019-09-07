using System.IO;
using System.Reflection;
using Autofac;
using ImageViewer.Configuration;
using AutofacConfig = ImageViewer.UnitTests.Configuration.AutofacConfig;

namespace ImageViewer.UnitTests.TestHelper
{
    public static class ContainerFactory
    {
        private static readonly string TestDirectory = Path.Combine(Path.GetTempPath(), "ImageViewTestdata\\");
        public const string CompanyName = "UnitTestForImageViewer";
        public const string ProductName = "ImageViewer";
        public const string ThumbnailIndexFilename = "thumbs.ibd";

        public static IContainer BuildContainerForThumbnailTests()
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(thisAssembly);

            var container = builder.Build();

            return container;
        }

        public static IContainer CreateGenericContainerForApp()
        {
            var builder = new ContainerBuilder();
            var unitTestAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyModules(unitTestAssembly);
            builder.RegisterAssemblyModules(AutofacConfig.GetAssembly());



            var container = builder.Build();

            return container;

        }

        public static string GetTestDirectory()
        {
            return TestDirectory;
        }
    }
}