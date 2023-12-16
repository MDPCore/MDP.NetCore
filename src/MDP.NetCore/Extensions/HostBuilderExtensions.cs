using MDP.Application;
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
            hostBuilder.AddMDP();
            hostBuilder.AddProgram<TProgram>();

            // Return
            return hostBuilder;
        }
    }

    public static partial class HostBuilderExtensions
    {
        // Methods        
        public static IHostBuilder AddMDP(this IHostBuilder hostBuilder)
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
                // Application
                var applicationName = string.Empty;
                if (string.IsNullOrEmpty(applicationName) == true) applicationName = hostContext.Configuration["Environment:ApplicationName"];
                if (string.IsNullOrEmpty(applicationName) == true) applicationName = hostContext.HostingEnvironment.ApplicationName;
                if (string.IsNullOrEmpty(applicationName) == true) throw new InvalidOperationException($"{nameof(applicationName)}=null");
                serviceCollection.AddSingleton(typeof(ApplicationInfo), new ApplicationInfo(applicationName));

                // RegisterModule
                serviceCollection.RegisterModule(hostContext.Configuration);

                // Logger
                serviceCollection.TryAddSingleton(typeof(ILogger<>), typeof(LoggerAdapter<>));

                // Tracer
                serviceCollection.TryAddSingleton(typeof(ITracer<>), typeof(TracerAdapter<>));
            });

            // Return
            return hostBuilder;
        }

        public static IHostBuilder AddProgram<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

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
    }
}