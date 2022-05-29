using Autofac;
using Autofac.Extensions.DependencyInjection;
using CLK.Diagnostics;
using MDP.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Reflection;

namespace MDP.NetCore
{
    internal static class HostBuilderExtensions
    {
        // Methods
        public static IHostBuilder ConfigureDefault(this IHostBuilder hostBuilder, Action<IHostBuilder>? configureAction = null)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Service
            hostBuilder.AddTracer();
            hostBuilder.AddOptions();
            hostBuilder.AddHttpClient();

            // Hosting
            hostBuilder.AddAutofac();
            hostBuilder.AddModule();

            // Action
            configureAction?.Invoke(hostBuilder);

            // Return
            return hostBuilder;
        }


        // Service
        private static void AddTracer(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Tracer
                services.TryAddSingleton(typeof(ITracer<>), typeof(Tracer<>));
            });
        }

        private static void AddOptions(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Options
                services.AddOptions();
            });
        }

        private static void AddHttpClient(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // HttpClientFactory
                services.AddHttpClient();

                // Func<HttpClientFactory>
                services.AddTransient<Func<IHttpClientFactory>>(serviceProvider => () =>
                {
                    return serviceProvider.GetService<IHttpClientFactory>()!;
                });
            });
        }

        public static void AddProgramService<TProgram>(this IHostBuilder hostBuilder)
            where TProgram : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Program
                services.TryAddTransient<TProgram, TProgram>();

                // ProgramService
                services.TryAddEnumerable(ServiceDescriptor.Transient<IHostedService, ProgramService<TProgram>>());
            });
        }

        // Hosting
        private static void AddAutofac(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Autofac
            hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }

        private static void AddModule(this IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Configuration
            hostBuilder.ConfigureAppConfiguration((hostContext, configurationBuilder) =>
            {
                configurationBuilder.RegisterModule(hostContext.HostingEnvironment);
            });

            // Container
            hostBuilder.ConfigureContainer<Autofac.ContainerBuilder>((hostContext, containerBuilder) =>
            {
                containerBuilder.RegisterModule(hostContext.Configuration);
            });

            // Host
            hostBuilder.ConfigureServices((context, services) =>
            {
                MDP.Hosting.ServiceBuilder.RegisterModule(Tuple.Create(context, services), context.Configuration);
            });
        }
    }
}