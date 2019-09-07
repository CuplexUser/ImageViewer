using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Autofac.Core;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Storage.Registry;
using ImageViewer.Managers;
using ImageViewer.Repositories;
using ImageViewer.Services;
using ImageViewer.Storage;
using ImageViewer.UnitTests.TestHelper;
using NSubstitute;
using Module = Autofac.Module;

namespace ImageViewer.UnitTests.AutofacModules
{
    public class ImageViewTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordStorage>().SingleInstance();
            var fileManagerCompleteFilepath = Path.Combine(ContainerFactory.GetTestDirectory(), "thumbs.ibd");

            builder.RegisterType<FileManager>().InstancePerLifetimeScope().OnPreparing(args => { args.Parameters = new[] { TypedParameter.From(fileManagerCompleteFilepath) }; });
            builder.RegisterType<PasswordStorage>().SingleInstance();


            builder.RegisterType<LocalStorageRegistryAccess>()
                .As<IRegistryAccess>()
                .InstancePerLifetimeScope()
                .WithParameters(new List<Parameter>()
                {
                    new NamedParameter("companyName", "Cuplex"),
                    new NamedParameter("productName","UnitTest Runner")
                });

            builder.RegisterType<ApplicationSettingsService>()
                .As<ServiceBase>()
                .InstancePerLifetimeScope()
                .WithParameters(new List<Parameter>()
                {
                    new NamedParameter("registryAccess", new LocalStorageRegistryAccess("Cuplex","Unit Test Run "))
                });


            builder.RegisterAssemblyTypes(typeof(ManagerBase).Assembly)
                .AssignableTo<ManagerBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
                .AssignableTo<ServiceBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
                .AssignableTo<RepositoryBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}