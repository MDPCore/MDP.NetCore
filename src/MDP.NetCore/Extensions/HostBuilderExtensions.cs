using MDP.Configuration;
using MDP.Hosting;
using MDP.Logging;
using MDP.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace MDP.NetCore
{
    public static class HostBuilderExtensions
    {
        // Methods
        public static IHostBuilder ConfigureMDP<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // ConfigurationBuilder
            hostBuilder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                // ConfigurationRegister
                ConfigurationRegister.RegisterModule(configurationBuilder, new MDP.Configuration.FileConfigurationProvider(hostContext.HostingEnvironment.EnvironmentName));
            });

            // ContainerBuilder
            hostBuilder.ConfigureServices((hostContext, serviceCollection) =>
            {
                // ContainerRegister
                ContainerRegister.RegisterModule(serviceCollection, hostContext.Configuration);

                // ProgramService
                serviceCollection.TryAddTransient<TProgram, TProgram>();
                serviceCollection.TryAdd(ServiceDescriptor.Transient<IHostedService, ProgramService<TProgram>>());
            });

            // Return
            return hostBuilder;
        }
    }
}