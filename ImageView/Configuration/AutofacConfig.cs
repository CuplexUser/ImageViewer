using System.Reflection;

namespace ImageViewer.Configuration;

public static class AutofacConfig
{
    public static IContainer CreateContainer()
    {
        var builder = new ContainerBuilder();
        var thisAssembly = GetMainAssembly();


        var coreAssemblies = new Assembly[2];
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


    public static Assembly GetMainAssembly()
    {
        return typeof(ImageViewModule).Assembly;
    }
}