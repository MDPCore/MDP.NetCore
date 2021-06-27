using Autofac;
using Autofac.Extensions.DependencyInjection;
using MDP.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            // Service
            hostBuilder.AddOptions();
            hostBuilder.AddHttpClient();

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
        private static void AddOptions(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Add
                services.AddOptions();
            });
        }

        private static void AddHttpClient(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Add
                services.AddHttpClient();
            });
        }

        public static void AddProgramService<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Program
                services.TryAddTransient<TProgram, TProgram>();

                // ProgramService
                services.TryAddTransient<IHostedService, ProgramService<TProgram>>();
            });
        }

        // Logger
        public static void AddConsoleLogger(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Logger
                services.AddLogging(loggingBuilder =>
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
            hostBuilder.ConfigureServices((context, services) =>
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
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostContext, container) =>
            {
                // Configuration
                container.RegisterGeneric(typeof(Configuration<>)).As(typeof(Configuration<>)).SingleInstance();
                container.RegisterGeneric(typeof(ConfigurationParameterDictionary<>)).As(typeof(ConfigurationParameterDictionary<>)).SingleInstance();

                // ModuleAssembly
                var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
                if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");
                
                // EntryAssembly
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
                if (moduleAssemblyList.Contains(entryAssembly) == false) moduleAssemblyList.Add(entryAssembly);

                // ModuleBuilder
                var moduleBuilder = new ContainerBuilder();
                {
                    // Service
                    moduleBuilder.RegisterInstance<IConfiguration>(hostContext.Configuration);
                    moduleBuilder.RegisterInstance<IHostEnvironment>(hostContext.HostingEnvironment);

                    // Assembly
                    moduleAssemblyList.ForEach(moduleAssembly =>
                    {
                        moduleBuilder
                            .RegisterAssemblyTypes(moduleAssembly)
                            .Where(assemblyType => typeof(MDP.Hosting.Module).IsAssignableFrom(assemblyType))
                            .As<MDP.Hosting.Module>();
                    });
                }

                // ModuleContainer
                using (var moduleContainer = moduleBuilder.Build())
                {
                    foreach (var module in moduleContainer.Resolve<IEnumerable<MDP.Hosting.Module>>())
                    {
                        container.RegisterModule(module);
                    }
                }
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
                
                // EntryConfigFile
                var entryConfigFileName = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "json");
                if (string.IsNullOrEmpty(entryConfigFileName) == true) throw new ArgumentException(nameof(entryConfigFileName));
                var entryConfigFile = new FileInfo(entryConfigFileName);
                if (entryConfigFile.Exists == true) moduleConfigFileList.RemoveAll(moduleConfigFile => moduleConfigFile.FullName == entryConfigFile.FullName);
                if (entryConfigFile.Exists == true) moduleConfigFileList.Add(entryConfigFile);

                // Register
                foreach (var moduleConfigFile in moduleConfigFileList)
                {
                    configuration.AddJsonFile(moduleConfigFile.FullName);
                }
            });
        }
    }
}