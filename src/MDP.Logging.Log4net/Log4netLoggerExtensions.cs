using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace MDP.Logging.Log4net
{
    public static class Log4netLoggerExtensions
    {
        // Methods
        public static IServiceCollection AddLog4netLogger(this IServiceCollection services, Log4netLoggerSetting? loggerSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException($"{nameof(services)}=null");

            #endregion

            // Logger
            services.AddLogging(builder =>
            {
                // Log4net
                builder.Services.AddSingleton<ILoggerProvider>(CreateLoggerProvider(services, loggerSetting));
            });

            // Return
            return services;
        }

        private static Log4netLoggerProvider CreateLoggerProvider(IServiceCollection services, Log4netLoggerSetting? loggerSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException($"{nameof(services)}=null");

            #endregion

            // LoggerSetting
            if (loggerSetting == null) loggerSetting = new Log4netLoggerSetting();

            // ConfigFileName
            var configFileName = loggerSetting.ConfigFileName;
            if (string.IsNullOrEmpty(configFileName) == true) throw new InvalidOperationException($"{nameof(configFileName)}=null");

            // Properties
            var properties = loggerSetting.Properties;
            if (properties == null) throw new InvalidOperationException($"{nameof(properties)}=null");

            // Properties-Default
            {
                // ApplicationName
                if (properties.ContainsKey("ApplicationName") == false) properties["ApplicationName"] = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name!;
            }

            // Return
            return new Log4netLoggerProvider(configFileName, properties);
        }
    }
}
