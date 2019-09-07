using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using AutoMapper;
using ImageViewer.Library.AutofacModules;
using ImageViewer.Library.AutoMapperProfile;
using ImageViewer.Repositories;
using Moq;
using NSubstitute;
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

            



            builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
                .AssignableTo<RepositoryBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(context => context.Resolve<MapperConfiguration>()
                        .CreateMapper())
                    .As<IMapper>()
                    .AutoActivate()
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

