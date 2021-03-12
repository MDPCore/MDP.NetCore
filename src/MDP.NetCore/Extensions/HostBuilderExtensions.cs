using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP
{
    public static class HostBuilderExtensions
    {
        // Methods
        public static IHostBuilder ConfigureMDP(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Autofac
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // Container
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((container) =>
            {
                // Module
                RegisterModule(container);
            });

            // Configuration
            hostBuilder.ConfigureAppConfiguration((hostContext, configuration) =>
            {
                // ModuleConfiguration
                RegisterModuleConfiguration(configuration);
            });

            // Return
            return hostBuilder;
        }

        private static void RegisterModule(ContainerBuilder container, string moduleAssemblyFileName = @"*.Hosting.dll")
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");
            moduleAssemblyList.ForEach(moduleAssembly => container.RegisterAssemblyModules<MDP.Module>(moduleAssembly));

            // EntryAssembly
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
            container.RegisterAssemblyModules<MDP.Module>(entryAssembly);
        }

        private static void RegisterModuleConfiguration(IConfigurationBuilder configuration, string moduleConfigFileName = @"*.Hosting.json")
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