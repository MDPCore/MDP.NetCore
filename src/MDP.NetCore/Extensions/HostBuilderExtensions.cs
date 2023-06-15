using MDP.Configuration;
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
        public static IHostBuilder ConfigureDefault(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // HostBuilder
            hostBuilder.RegisterModule();

            // Return
            return hostBuilder;
        }

        public static IHostBuilder ConfigureDefault<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // HostBuilder
            hostBuilder.ConfigureDefault().ConfigureServices((context, serviceCollection) =>
            {
                // ProgramService
                serviceCollection.TryAddTransient<TProgram, TProgram>();
                serviceCollection.Add(ServiceDescriptor.Transient<IHostedService, ProgramService<TProgram>>());
            });

            // Return
            return hostBuilder;
        }
    }

    public static partial class HostBuilderExtensions
    {
        // Methods        
        public static IHostBuilder RegisterModule(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // ConfigurationBuilder
            hostBuilder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                // Module
                configurationBuilder.RegisterModule(hostContext.HostingEnvironment.EnvironmentName);
            });

            // HostBuilder
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                // Logger
                serviceCollection.TryAddSingleton(typeof(ILogger<>), typeof(Logger<>));

                // Tracer
                serviceCollection.TryAddSingleton(typeof(ITracer<>), typeof(Tracer<>));

                // FactoryRegisterContext
                var factoryRegisterContext = new FactoryRegisterContext<IServiceCollection>();
                {
                    // Module
                    factoryRegisterContext.RegisterModule(serviceCollection, context.Configuration);
                }

                // ServiceRegisterContext
                var serviceRegisterContext = new ServiceRegisterContext();
                {
                    // Module
                    serviceRegisterContext.RegisterModule(serviceCollection, context.Configuration);
                }
            });

            // Return
            return hostBuilder;
        }
    }    
}