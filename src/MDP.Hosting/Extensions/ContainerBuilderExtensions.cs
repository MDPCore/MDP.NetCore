using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Hosting
{
    public static class ContainerBuilderExtensions
    {
        // Methods   
        public static ContainerBuilder RegisterModule(this ContainerBuilder containerBuilder, IConfiguration configuration = null, string moduleAssemblyFileName = @"*.Hosting.dll")
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // EntryAssembly
            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
            if (moduleAssemblyList.Contains(entryAssembly) == false) moduleAssemblyList.Add(entryAssembly);

            // RegisterAssemblyTypes
            var moduleContainerBuilder = new ContainerBuilder();
            {
                moduleAssemblyList.ForEach(moduleAssembly =>
                {
                    moduleContainerBuilder
                        .RegisterAssemblyTypes(moduleAssembly)
                        .Where(assemblyType => typeof(MDP.Hosting.Module).IsAssignableFrom(assemblyType))
                        .As<MDP.Hosting.Module>();
                });
            }

            // RegisterModule
            using (var moduleContainer = moduleContainerBuilder.Build())
            {
                foreach (var module in moduleContainer.Resolve<IEnumerable<MDP.Hosting.Module>>())
                {
                    // Register
                    {
                        module.Configuration = configuration;
                    }
                    containerBuilder.RegisterModule(module);
                }
            }

            // Return
            return containerBuilder;
        }

        public static IList<IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle>> RegisterFactory<TService, TFactory>(this ContainerBuilder containerBuilder, IConfiguration configuration)
                where TService : class
                where TFactory : Factory<TService>
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));
            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Variables
            var registrationBuilderList = new List<IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle>>();

            // NamespaceConfigKey
            var namespaceConfigKey = typeof(TService).Namespace;
            if (string.IsNullOrEmpty(namespaceConfigKey) == true) throw new InvalidOperationException($"{nameof(namespaceConfigKey)}=null");

            // ServiceConfigKey
            var serviceConfigKey = typeof(TFactory).Name;
            if (string.IsNullOrEmpty(serviceConfigKey) == true)
            {
                throw new InvalidOperationException($"{nameof(serviceConfigKey)}=null");
            }
            if (serviceConfigKey.EndsWith("Factory") == true)
            {
                serviceConfigKey = serviceConfigKey.Substring(0, serviceConfigKey.Length - "Factory".Length);
            }
            if (string.IsNullOrEmpty(serviceConfigKey) == true) throw new InvalidOperationException($"{serviceConfigKey}=null");

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(namespaceConfigKey);
            if (namespaceConfig == null) return registrationBuilderList;

            // ServiceConfig
            foreach (var serviceConfig in namespaceConfig.GetChildren())
            {
                // RegisterService-Default
                if (serviceConfig.Key == serviceConfigKey)
                {
                    registrationBuilderList.Add(containerBuilder.Register((componentContext, parameterList) =>
                    {
                        // ServiceFactory
                        var serviceFactory = componentContext.Resolve<TFactory>();
                        if (serviceFactory == null) throw new InvalidOperationException($"{nameof(serviceFactory)}=null");

                        // Service
                        return serviceFactory.Create(componentContext, serviceConfig);
                    }));
                }

                // RegisterService-Named
                if (serviceConfig.Key.StartsWith(serviceConfigKey) == true)
                {
                    registrationBuilderList.Add(containerBuilder.Register((componentContext, parameterList) =>
                    {
                        // ServiceFactory
                        var serviceFactory = componentContext.Resolve<TFactory>();
                        if (serviceFactory == null) throw new InvalidOperationException($"{nameof(serviceFactory)}=null");

                        // Service
                        return serviceFactory.Create(componentContext, serviceConfig);
                    }).Named<TService>(serviceConfig.Key));
                }
            }

            // RegisterFactory
            containerBuilder.RegisterType<TFactory>().As<TFactory>().SingleInstance().IfNotRegistered(typeof(TFactory));

            // Return
            return registrationBuilderList;
        }
    }
}