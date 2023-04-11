using Autofac;
using Autofac.Extensions.DependencyInjection;
using MDP.Hosting;
using MDP.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MDP.Tracing;
using Microsoft.Extensions.Configuration;

namespace MDP.NetCore
{
    public static class HostBuilderExtensions
    {
        // Methods        
        public static IHostBuilder ConfigureDefault(this IHostBuilder hostBuilder) 
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

            // ContainerBuilder
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostContext, containerBuilder) =>
            {
                // Module
                containerBuilder.RegisterModule(hostContext.Configuration);
            });
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());

            // HostBuilder
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                // Logger

                // Tracer
                serviceCollection.TryAddSingleton(typeof(ITracer<>), typeof(Tracer<>));

                // RegisterContext
                using (var registerContext = new NetCoreRegisterContext())
                {
                    // Module
                    registerContext.RegisterModule(serviceCollection, context.Configuration);
                }
            });

            // Return
            return hostBuilder;
        }

        public static IHostBuilder ConfigureDefault<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // HostBuilder
            hostBuilder.ConfigureServices((context, serviceCollection) =>
            {
                // ProgramService
                serviceCollection.TryAddTransient<TProgram, TProgram>();
                serviceCollection.Add(ServiceDescriptor.Transient<IHostedService, ProgramService<TProgram>>());
            })
            .ConfigureDefault();

            // Return
            return hostBuilder;
        }
    }
}