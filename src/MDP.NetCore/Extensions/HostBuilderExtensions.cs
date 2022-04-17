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
    public static class HostBuilderExtensions
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
            hostBuilder.AddModule();
            hostBuilder.AddOptions();
            hostBuilder.AddHttpClient();

            // Expand
            if (configureAction != null)
            {
                configureAction(hostBuilder);
            }

            // Return
            return hostBuilder;
        }


        // Service
        private static void AddModule(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));
            
            #endregion

            // AppConfiguration
            hostBuilder.ConfigureAppConfiguration((configurationBuilder) =>
            {
                configurationBuilder.RegisterModule();
            });

            // ContainerBuilder
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostContext, containerBuilder) =>
            {
                containerBuilder.RegisterModule(hostContext.Configuration);
            });
        }

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

        public static void AddProgramService<TProgram>(this IHostBuilder hostBuilder)
            where TProgram : class
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
    }
}