using Autofac;
using Autofac.Extensions.DependencyInjection;
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

            // Register
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((builder) =>
            {
                // Modules
                RegisterModules(builder);

                // Config
                RegisterConfigurations(builder);
            });

            // Return
            return hostBuilder;
        }

        private static void RegisterModules(Autofac.ContainerBuilder builder, string moduleAssemblyFileName = @"*.Hosting.dll")
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");
            moduleAssemblyList.ForEach(moduleAssembly => builder.RegisterAssemblyModules<MDP.Module>(moduleAssembly));

            // EntryAssembly
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
            builder.RegisterAssemblyModules<MDP.Module>(entryAssembly);
        }

        private static void RegisterConfigurations(Autofac.ContainerBuilder builder, string configurationFileName = @"*.Hosting.json")
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(configurationFileName) == true) throw new ArgumentException(nameof(configurationFileName));

            #endregion

        }
    }
}