﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using MDP.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    public static partial class HostBuilderExtensions
    {
        // Methods
        public static IHostBuilder ConfigureNetCore(this IHostBuilder hostBuilder, Action<IHostBuilder> configureAction = null)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Autofac
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // Configuration
            hostBuilder.ConfigureAppConfiguration((configuration) =>
            {
                // Module
                configuration.AddModuleConfiguration();
            });

            // Container
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((container) =>
            {
                // Service
                container.AddService();

                // Module
                container.AddModuleService();
            });

            // Expand
            if (configureAction != null)
            {
                configureAction(hostBuilder);
            }

            // Return
            return hostBuilder;
        }

        private static void AddService(this ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // Configuration<TService>
            container.RegisterGeneric(typeof(Configuration<>)).As(typeof(Configuration<>)).SingleInstance();
            container.RegisterGeneric(typeof(ConfigurationParameterDictionary<>)).As(typeof(ConfigurationParameterDictionary<>)).SingleInstance();
        }

        private static void AddModuleService(this ContainerBuilder container, string moduleAssemblyFileName = @"*.Hosting.dll")
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");
            moduleAssemblyList.ForEach(moduleAssembly => container.RegisterAssemblyModules<MDP.Hosting.Module>(moduleAssembly));

            // EntryAssembly
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
            container.RegisterAssemblyModules<MDP.Hosting.Module>(entryAssembly);
        }

        private static void AddModuleConfiguration(this IConfigurationBuilder configuration, string moduleConfigFileName = @"*.Hosting.json")
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(moduleConfigFileName) == true) throw new ArgumentException(nameof(moduleConfigFileName));

            #endregion

            // ModuleConfigFile
            var moduleConfigFileList = CLK.IO.File.GetAllFile(moduleConfigFileName);
            if (moduleConfigFileList == null) throw new InvalidOperationException($"{nameof(moduleConfigFileList)}=null");
            moduleConfigFileList.ForEach(moduleConfigFile => configuration.AddJsonFile(moduleConfigFile.FullName));

            // EntryConfigFile
            var entryConfigFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "json");
            if (string.IsNullOrEmpty(entryConfigFileName) == true) throw new ArgumentException(nameof(entryConfigFileName));
            if (File.Exists(entryConfigFileName) == true) configuration.AddJsonFile(entryConfigFileName);
        }
    }
}