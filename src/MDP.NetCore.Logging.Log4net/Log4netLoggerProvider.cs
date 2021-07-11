using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore.Logging.Log4net
{
    [ProviderAlias("Log4net")]
    public class Log4netLoggerProvider : ILoggerProvider
    {
        // Fields
        private readonly ILoggerRepository _loggerRepository = null;


        // Constructors
        public Log4netLoggerProvider(IOptions<Log4netLoggerOptions> options)
        {
            #region Contracts

            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // ConfigFileName
            var configFileName = options.Value?.ConfigFileName;
            if (string.IsNullOrEmpty(configFileName) == true) throw new InvalidOperationException($"{nameof(configFileName)}=null");

            // Properties
            var properties = options.Value?.Properties;
            if (properties == null) throw new InvalidOperationException($"{nameof(properties)}=null");

            // LoggerRepository
            _loggerRepository = this.CreateLoggerRepository(configFileName, properties);
            if (_loggerRepository == null) throw new InvalidOperationException($"{nameof(_loggerRepository)}=null");
        }

        public void Dispose()
        {
            // Shutdown
            _loggerRepository.Shutdown();
        }


        // Methods
        public ILogger CreateLogger(string categoryName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(categoryName) == true) throw new ArgumentException(nameof(categoryName));

            #endregion

            // Create
            return new Log4netLogger(categoryName);
        }

        private ILoggerRepository CreateLoggerRepository(string configFileName, Dictionary<string, string> properties)
        {
            #region Contracts

            if (string.IsNullOrEmpty(configFileName) == true) throw new ArgumentException(nameof(configFileName));
            if (properties == null) throw new ArgumentException(nameof(properties));

            #endregion

            // Require
            if (File.Exists(configFileName) == false) throw new InvalidOperationException($"{configFileName} not found.");

            // GlobalContext
            foreach (var propertyPair in properties)
            {
                GlobalContext.Properties[propertyPair.Key] = propertyPair.Value;
            }

            // LoggerRepository
            var loggerRepository = LogManager.GetRepository();
            if (loggerRepository == null) throw new InvalidOperationException($"{nameof(loggerRepository)}=null");

            // Configure
            XmlConfigurator.Configure(loggerRepository, new FileInfo(configFileName));
                                   
            // Return
            return loggerRepository;
        }
    }
}
