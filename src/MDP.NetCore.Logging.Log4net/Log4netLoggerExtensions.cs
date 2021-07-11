using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Hosting;

namespace MDP.NetCore.Logging.Log4net
{
    public static class Log4netLoggerExtensions
    {
        // Methods
        public static IHostBuilder AddLog4netLogger(this IHostBuilder hostBuilder, Action<Log4netLoggerOptions> configureOptions = null)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Logger
                services.AddLog4netLogger(configureOptions);
            });

            // Return
            return hostBuilder;
        }

        public static IServiceCollection AddLog4netLogger(this IServiceCollection services, Action<Log4netLoggerOptions> configureOptions = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Logger
            services.AddLogging(builder =>
            {
                // Configuration
                builder.AddConfiguration();

                // Provider
                builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, Log4netLoggerProvider>());
            });

            // Options            
            LoggerProviderOptions.RegisterProviderOptions<Log4netLoggerOptions, Log4netLoggerProvider>
            (
                services
            );
            if (configureOptions != null) services.Configure(configureOptions);

            // Return
            return services;
        }
    }
}
