using Autofac;
using Autofac.Extensions.DependencyInjection;
using MDP.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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

            // Module
            hostBuilder.AddModuleConfiguration();
            hostBuilder.AddModuleService();

            // Expand
            if (configureAction != null)
            {
                configureAction(hostBuilder);
            }

            // Return
            return hostBuilder;
        }


        // Service
        public static void AddConsoleLogger(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            hostBuilder.ConfigureServices((context, collection) =>
            {
                collection.AddLogging(loggingBuilder =>
                {
                    // Add
                    loggingBuilder.AddConsole();
                });
            });
        }

        public static void RemoveConsoleLogger(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Services
            hostBuilder.ConfigureServices((hostContext, services) =>
            {
                // Remove
                services.RemoveService<ConsoleLoggerProvider>();
            });
        }

        // Module
        private static void AddModuleService(this IHostBuilder hostBuilder, string moduleAssemblyFileName = @"*.Hosting.dll")
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // Container
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((container) =>
            {
                // Configuration
                container.RegisterGeneric(typeof(Configuration<>)).As(typeof(Configuration<>)).SingleInstance();
                container.RegisterGeneric(typeof(ConfigurationParameterDictionary<>)).As(typeof(ConfigurationParameterDictionary<>)).SingleInstance();

                // ModuleAssembly
                var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
                if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");
                moduleAssemblyList.ForEach(moduleAssembly => container.RegisterAssemblyModules<MDP.Hosting.Module>(moduleAssembly));

                // EntryAssembly
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
                container.RegisterAssemblyModules<MDP.Hosting.Module>(entryAssembly);
            });
        }

        private static void AddModuleConfiguration(this IHostBuilder hostBuilder, string moduleConfigFileName = @"*.Hosting.json")
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));
            if (string.IsNullOrEmpty(moduleConfigFileName) == true) throw new ArgumentException(nameof(moduleConfigFileName));

            #endregion

            // AppConfiguration
            hostBuilder.ConfigureAppConfiguration((configuration) =>
            {
                // ModuleConfigFile
                var moduleConfigFileList = CLK.IO.File.GetAllFile(moduleConfigFileName);
                if (moduleConfigFileList == null) throw new InvalidOperationException($"{nameof(moduleConfigFileList)}=null");
                moduleConfigFileList.ForEach(moduleConfigFile => configuration.AddJsonFile(moduleConfigFile.FullName));

                // EntryConfigFile
                var entryConfigFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "json");
                if (string.IsNullOrEmpty(entryConfigFileName) == true) throw new ArgumentException(nameof(entryConfigFileName));
                if (File.Exists(entryConfigFileName) == true) configuration.AddJsonFile(entryConfigFileName);
            });
        }
    }
}