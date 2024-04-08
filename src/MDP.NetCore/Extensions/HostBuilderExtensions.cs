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
    public static partial class HostBuilderExtensions
    {
        // Methods    
        public static IHostBuilder ConfigureMDP<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // HostBuilder
            hostBuilder.ConfigureMDP();

            // ContainerBuilder
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                // ProgramService
                serviceCollection.TryAddTransient<TProgram, TProgram>();
                serviceCollection.TryAdd(ServiceDescriptor.Transient<IHostedService, ProgramService<TProgram>>());
            });

            // Return
            return hostBuilder;
        }

        public static IHostBuilder ConfigureMDP(this IHostBuilder hostBuilder)
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
                // Logger
                serviceCollection.TryAddSingleton(typeof(ILogger<>), typeof(LoggerAdapter<>));

                // Tracer
                serviceCollection.TryAddSingleton(typeof(ITracer<>), typeof(TracerAdapter<>));

                // List
                serviceCollection.TryAddTransient(typeof(IList<>), typeof(List<>));

                // ServiceFactoryRegister
                ServiceFactoryRegister.RegisterModule(serviceCollection, hostContext.Configuration);

                // ServiceAttributeRegister
                ServiceAttributeRegister.RegisterModule(serviceCollection, hostContext.Configuration);

                // ServiceRegistrationRegister
                ServiceRegistrationRegister.RegisterModule(serviceCollection);
            });

            // Return
            return hostBuilder;
        }
    }
}