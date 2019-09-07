using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using GeneralToolkitLib.ConfigHelper;
using ImageViewer.Library.AutofacModules;

namespace ImageViewer.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = GetMainAssembly();


            Assembly[] coreAssemblies = new Assembly[2];
            var generalToolKitAssembly = AssemblyHelper.GetAssembly();

            coreAssemblies[0] = thisAssembly;
            coreAssemblies[1] = generalToolKitAssembly;
           
            if (generalToolKitAssembly != null)
            {
                builder.RegisterAssemblyModules(generalToolKitAssembly);
            }

            builder.RegisterAssemblyModules(thisAssembly);
            var container = builder.Build();


            return container;
        }



        public static  Assembly GetMainAssembly()
        {
            return typeof(ImageViewer.Library.AutofacModules.ImageViewModule).Assembly;
        }
    }
}
