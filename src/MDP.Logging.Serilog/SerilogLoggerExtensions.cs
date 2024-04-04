using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;

namespace MDP.Logging.Serilog
{
    public static class SerilogLoggerExtensions
    {
        // Methods
        public static IServiceCollection AddSerilogLogger(this IServiceCollection services, SerilogLoggerSetting loggerSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Logger
            services.AddLogging(loggingBuilder =>
            {
                // Serilog
                loggingBuilder.AddSerilogLogger(loggerSetting);
            });

            // Return
            return services;
        }

        private static ILoggingBuilder AddSerilogLogger(this ILoggingBuilder loggingBuilder, SerilogLoggerSetting loggerSetting = null)
        {
            #region Contracts

            if (loggingBuilder == null) throw new ArgumentException($"{nameof(loggingBuilder)}=null");

            #endregion

            // LoggerSetting
            if (loggerSetting == null) loggerSetting = new SerilogLoggerSetting();

            // ConfigFileName
            var configFileName = loggerSetting.ConfigFileName;
            if (string.IsNullOrEmpty(configFileName) == true) throw new InvalidOperationException($"{nameof(configFileName)}=null");

            // ConfigFile
            var configFile = MDP.IO.File.GetFile(configFileName);
            if (configFile == null) throw new InvalidOperationException($"{configFileName} not found.");

            // Properties
            var properties = loggerSetting.Properties;
            if (properties == null) throw new InvalidOperationException($"{nameof(properties)}=null");
            if (properties.ContainsKey("ApplicationName") == false) properties["ApplicationName"] = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name!;

            // SerilogConfig
            var serilogConfig = new ConfigurationBuilder()
                .AddJsonFile(configFile.FullName)
                .Build();

            // SerilogLogger
            var loggerConfig = new LoggerConfiguration();
            {
                foreach (var propertyPair in properties)
                {
                    loggerConfig.Enrich.WithProperty(propertyPair.Key, propertyPair.Value);
                }
                loggerConfig.Enrich.WithThreadId();
                loggerConfig.ReadFrom.Configuration(serilogConfig);
            }
            Log.Logger = loggerConfig.CreateLogger();

            // Serilog
            loggingBuilder.AddProvider(new SerilogLoggerProvider());

            // Return
            return loggingBuilder;
        }
    }
}
