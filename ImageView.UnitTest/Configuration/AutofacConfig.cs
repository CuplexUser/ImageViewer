using System.Reflection;
using Autofac;
using AutoMapper;
using ImageViewer.Repositories;
using NSubstitute.Extensions;
using Serilog;
using Serilog.Core;

namespace ImageViewer.UnitTests.Configuration
{
    public static class AutofacConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            var thisAssembly = Assembly.GetExecutingAssembly();
            MockLogger mockLogger= new MockLogger();
            mockLogger.SetupAllProperties();

            builder.RegisterAssemblyModules(thisAssembly);
            builder.RegisterAssemblyModules(ImageViewer.Configuration.AutofacConfig.GetMainAssembly());
            //builder.RegisterAssemblyModules(ImageViewer.Configuration.AutofacConfig.GetGUIAssembly());



            builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
                   .AssignableTo<RepositoryBase>()
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.Register(context => context.Resolve<MapperConfiguration>()
                        .CreateMapper())
                    .As<IMapper>()
                    .SingleInstance();

            //builder.Register()
            //    .AutoActivate()
            //    .AsSelf()
            //    .AsImplementedInterfaces()
            //    .SingleInstance();
            builder.Register<ILogger>((context, parameters) => context.InjectProperties<Logger>(mockLogger.GetMockedLogger())).AsSelf().SingleInstance();
            builder.Configure();

            var container = builder.Build();

            return container;

        }

        public static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        //private static MapperConfiguration Configure(IComponentContext context)
        //{

        //}

    }
}

