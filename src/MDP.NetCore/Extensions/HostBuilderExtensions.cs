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
using MDP.Hosting;
using MDP.Working;

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

            // Configuration
            hostBuilder.ConfigureAppConfiguration((configuration) =>
            {
                // ModuleConfiguration
                configuration.AddModuleConfiguration();
            });

            // Container
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((container) =>
            {
                // Module
                container.AddModule();
            });

            // Services
            hostBuilder.ConfigureServices((services) =>
            {
                // Quartz
                services.AddQuartz();
            });

            // Return
            return hostBuilder;
        }
    }
}