using Autofac;
using Autofac.Extensions.DependencyInjection;
using MDP.Hosting;
using MDP.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using CLK.Tracing;

namespace MDP.NetCore
{
    internal static class HostBuilderExtensions
    {
        // Methods
        public static IHostBuilder ConfigureDefault<TProgram>(this IHostBuilder hostBuilder) where TProgram : class
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
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Tracer
                services.TryAddSingleton(typeof(ITracer<>), typeof(Tracer<>));

                // ProgramService
                services.TryAddTransient<TProgram, TProgram>();
                services.Add(ServiceDescriptor.Transient<IHostedService, ProgramService<TProgram>>());
            });
            hostBuilder.RegisterModule();

            // Return
            return hostBuilder;
        }

        private static IHostBuilder RegisterModule(this IHostBuilder hostBuilder) 
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Return
            return hostBuilder;
        }
    }
}