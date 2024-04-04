using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using NLogLib = NLog;

namespace MDP.Logging.NLog
{
    public static class NLogLoggerExtensions
    {
        // Methods
        public static IServiceCollection AddNLogLogger(this IServiceCollection services, NLogLoggerSetting loggerSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Logger
            services.AddLogging(loggingBuilder =>
            {
                // NLog
                loggingBuilder.AddNLog(serviceProvider => CreateLogFactory(services, loggerSetting));
            });

            // Return
            return services;
        }

        private static NLogLib.LogFactory CreateLogFactory(IServiceCollection services, NLogLoggerSetting loggerSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException($"{nameof(services)}=null");

            #endregion

            // LoggerSetting
            if (loggerSetting == null) loggerSetting = new NLogLoggerSetting();

            // ConfigFileName
            var configFileName = loggerSetting.ConfigFileName;
            if (string.IsNullOrEmpty(configFileName) == true) throw new InvalidOperationException($"{nameof(configFileName)}=null");

            // ConfigFile
            var configFile = MDP.IO.File.GetFile(configFileName);
            if (configFile == null) throw new InvalidOperationException($"{configFileName} not found.");

            // Properties
            var properties = loggerSetting.Properties;
            if (properties == null) throw new InvalidOperationException($"{nameof(properties)}=null");

            // Properties-Default
            {
                // ApplicationName
                if (properties.ContainsKey("ApplicationName") == false) properties["ApplicationName"] = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name!;
            }

            // GlobalDiagnosticsContext
            foreach (var propertyPair in properties)
            {
                NLogLib.GlobalDiagnosticsContext.Set(propertyPair.Key, propertyPair.Value);
            }

            // LogFactory
            var logFactory = NLogLib.LogManager.LoadConfiguration(configFile.FullName);
            if (logFactory == null) throw new InvalidOperationException($"{nameof(logFactory)}=null");

            // Return
            return logFactory;
        }
    }
}
