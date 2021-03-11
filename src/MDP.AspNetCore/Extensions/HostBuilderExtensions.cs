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

namespace MDP.AspNetCore
{
    public static class HostBuilderExtensions
    {
        // Methods
        public static IHostBuilder ConfigureWebMDP(this IHostBuilder hostBuilder)
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