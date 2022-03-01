using Autofac;
using GeneralToolkitLib.ConfigHelper;
using System.Reflection;

namespace ImageView.UnitTests.Configuration
{
    public static class AutofacConfig
    {

        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = GetUnitTestAssembly();


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

        //public static IContainer CreateContainer()
        //{
        //    var builder = new ContainerBuilder();
        //    var thisAssembly = GetMainAssembly();
        //    MockLogger mockLogger = new MockLogger();
        //    mockLogger.SetupAllProperties();

        //    builder.RegisterAssemblyModules(thisAssembly);
        //    builder.RegisterAssemblyModules(ImageViewer.Configuration.AutofacConfig.GetMainAssembly());


        //    builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
        //        .AssignableTo<RepositoryBase>()
        //        .AsSelf()
        //        .AsImplementedInterfaces()
        //        .SingleInstance();

        //    builder.Register(context => context.Resolve<MapperConfiguration>()
        //                .CreateMapper())
        //            .As<IMapper>()
        //            .AutoActivate()
        //            .SingleInstance();

        //    //builder.Register()
        //    //    .AutoActivate()
        //    //    .AsSelf()
        //    //    .AsImplementedInterfaces()
        //    //    .SingleInstance();
        //    builder.Register<ILogger>((context, parameters) => context.InjectProperties<Logger>(mockLogger.GetMockedLogger())).AsSelf().SingleInstance();
        //    builder.Configure();

        //    var container = builder.Build();

        //    return container;

        //}

        public static Assembly GetUnitTestAssembly()
        {
            return typeof(UnitTestModule).Assembly;
        }

    }
}

