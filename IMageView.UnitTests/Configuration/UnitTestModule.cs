﻿using Autofac;
using Autofac.Core;
using AutoMapper;
using GeneralToolkitLib.Storage.Memory;
using GeneralToolkitLib.Storage.Registry;
using ImageViewer.Managers;
using ImageViewer.Repositories;
using ImageViewer.Services;
using System.Collections.Generic;
using System.Windows.Forms;
using Module = Autofac.Module;

namespace ImageView.UnitTests.Configuration
{
    public class UnitTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordStorage>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ManagerBase).Assembly)
                .AssignableTo<ManagerBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(ServiceBase).Assembly)
                .AssignableTo<ServiceBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(RepositoryBase).Assembly)
                .AssignableTo<RepositoryBase>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();


            builder.RegisterType<RegistryAccess>()
                .As<IRegistryAccess>()
                .SingleInstance()
                .WithParameters(new List<Parameter>()
                {
                    new NamedParameter("companyName", Application.CompanyName), new NamedParameter("companyName", Application.ProductName),
                    new NamedParameter("productName", Application.ProductName), new NamedParameter("productName", Application.ProductName)
                });

            builder.Register(context => context.Resolve<MapperConfiguration>()
                    .CreateMapper())
                .As<IMapper>()
                .AutoActivate()
                .SingleInstance();

            builder.Register(Configure)
                            .AutoActivate()
                            .AsSelf()
                            .AsImplementedInterfaces()
                            .SingleInstance();

            //var assembly = Assembly.GetExecutingAssembly();
            //builder.RegisterAssemblyTypes(assembly)
            //                .AssignableTo<Form>()
            //                .AsSelf()
            //                .InstancePerDependency();
        }

        private void Handler(IActivatingEventArgs<IRegistryAccess> obj)
        {
            obj.ReplaceInstance(new RegistryAccess(Application.CompanyName, Application.ProductName));
        }

        private static MapperConfiguration Configure(IComponentContext context)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                var innerContext = context.Resolve<IComponentContext>();
                cfg.ConstructServicesUsing(innerContext.Resolve);

                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            });

            return configuration;
        }
    }
}
