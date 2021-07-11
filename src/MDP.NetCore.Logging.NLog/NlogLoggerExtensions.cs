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
using NLog.Extensions.Logging;
using NLogLib = NLog;
using System.IO;

namespace MDP.NetCore.Logging.NLog
{ 
    public static class NLogLoggerExtensions
    {
        // Methods
        public static IHostBuilder AddNLogLogger(this IHostBuilder hostBuilder, Action<NLogLoggerOptions> configureOptions = null)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Services
            hostBuilder.ConfigureServices((context, services) =>
            {
                // Logger
                services.AddNLogLogger(configureOptions);
            });

            // Return
            return hostBuilder;
        }

        public static IServiceCollection AddNLogLogger(this IServiceCollection services, Action<NLogLoggerOptions> configureOptions = null)
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
                builder.AddNLog(serviceProvider => CreateLogFactory(serviceProvider));
            });

            // Options
            services.TryAddSingleton<IConfigureOptions<NLogLoggerOptions>>(serviceProvider =>
            {
                // Configuration
                var configuration = serviceProvider.GetRequiredService<ILoggerProviderConfiguration<NLogLoggerProvider>>()?.Configuration;
                if (configuration == null) throw new InvalidOperationException($"{nameof(configuration)}=null");

                // Configure
                return new NamedConfigureFromConfigurationOptions<NLogLoggerOptions>(Options.DefaultName, configuration);
            });
            if (configureOptions != null) services.Configure(configureOptions);

            // Return
            return services;
        }

        private static NLogLib.LogFactory CreateLogFactory(IServiceProvider serviceProvider)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException(nameof(serviceProvider));

            #endregion

            // Options
            var options = serviceProvider.GetRequiredService<IOptions<NLogLoggerOptions>>()?.Value;
            if (options == null) throw new InvalidOperationException($"{nameof(options)}=null");

            // ConfigFileName
            var configFileName = options.ConfigFileName;
            if (string.IsNullOrEmpty(configFileName) == true) throw new InvalidOperationException($"{nameof(configFileName)}=null");
            if (File.Exists(configFileName) == false) throw new InvalidOperationException($"{configFileName} not found.");

            // Properties
            var properties = options.Properties;
            if (properties == null) throw new InvalidOperationException($"{nameof(properties)}=null");
                      
            // GlobalDiagnosticsContext
            foreach (var propertyPair in properties)
            {
                NLogLib.GlobalDiagnosticsContext.Set(propertyPair.Key, propertyPair.Value);
            }

            // LogFactory
            var logFactory = NLogLib.LogManager.LoadConfiguration(configFileName);
            if (logFactory == null) throw new InvalidOperationException($"{nameof(logFactory)}=null");

            // Return
            return logFactory;
        }
    }
}
