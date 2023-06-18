using MDP.Configuration;
using MDP.Logging;
using MDP.Tracing;
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
        public static IHostBuilder ConfigureDefault<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // RegisterModule
            hostBuilder.RegisterModule();

            // HostBuilder
            hostBuilder.ConfigureServices((context, serviceCollection) =>
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
                // RegisterModule
                configurationBuilder.RegisterModule(hostContext.HostingEnvironment.EnvironmentName);
            });

            // HostBuilder
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                // List
                serviceCollection.TryAddTransient(typeof(IList<>), typeof(List<>));

                // Logger
                serviceCollection.TryAddSingleton(typeof(ILogger<>), typeof(Logger<>));

                // Tracer
                serviceCollection.TryAddSingleton(typeof(ITracer<>), typeof(Tracer<>));

                // FactoryRegisterContext
                var factoryRegisterContext = new FactoryRegisterContext<IServiceCollection>();
                {
                    // RegisterModule
                    factoryRegisterContext.RegisterModule(serviceCollection, context.Configuration);
                }

                // ServiceRegisterContext
                var serviceRegisterContext = new ServiceRegisterContext();
                {
                    // RegisterModule
                    serviceRegisterContext.RegisterModule(serviceCollection, context.Configuration);
                }
            });

            // Return
            return hostBuilder;
        }
    }    
}