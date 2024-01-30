using MDP.Configuration;
using MDP.Hosting;
using MDP.Logging;
using MDP.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

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
                // RegisterModule
                configurationBuilder.RegisterModule(hostContext.HostingEnvironment.EnvironmentName);
            });

            // ContainerBuilder
            hostBuilder.ConfigureServices((hostContext, serviceCollection) =>
            {
                // Logger
                serviceCollection.TryAddSingleton(typeof(ILogger<>), typeof(LoggerAdapter<>));

                // Tracer
                serviceCollection.TryAddSingleton(typeof(ITracer<>), typeof(TracerAdapter<>));

                // RegisterModule
                serviceCollection.RegisterModule(hostContext.Configuration);
            });

            // Return
            return hostBuilder;
        }
    }
}