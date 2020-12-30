using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging.Log4net
{
    public class Log4netLoggerProvider : LoggerProvider
    {
        // Constructors
        public Log4netLoggerProvider(string configFileName = null)
        {
            // Initialize
            if (string.IsNullOrEmpty(configFileName) == true)
            {
                this.Initialize();
            }
            else
            {
                this.Initialize(configFileName);
            }
        }

        public void Start()
        {
            // Nothing

        }

        public void Dispose()
        {
            // Nothing

        }


        // Methods
        public Logger<TCategory> Create<TCategory>()
        {
            // Return
            return new Log4netLogger<TCategory>();
        }


        private void Initialize()
        {
            // Setting
            log4net.MDC.Set("tab", "\t");

            // Repository
            var repository = log4net.LogManager.GetRepository();
            if (repository == null) throw new InvalidOperationException("repository=null");

            // PatternLayout
            var patternLayout = new log4net.Layout.PatternLayout()
            {
                ConversionPattern = @"%date{yyyy-MM-dd HH:mm:ss fff} %-5level [%thread] %logger.%property{method}() - %message%newline"
            };
            patternLayout.ActivateOptions();

            // ConsoleAppender
            var consoleAppender = new log4net.Appender.ConsoleAppender()
            {
                Layout = patternLayout
            };
            consoleAppender.ActivateOptions();

            // FileAppender
            var fileAppender = new log4net.Appender.RollingFileAppender()
            {
                Layout = patternLayout,
                RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date,
                File = string.Format("log/"),
                DatePattern = "yyyy-MM-dd'.log'",
                StaticLogFileName = false,
                AppendToFile = true
            };
            fileAppender.ActivateOptions();

            // Configure
            log4net.Config.BasicConfigurator.Configure(repository,
                consoleAppender,
                fileAppender
            );
        }

        private void Initialize(string configFileName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(configFileName) == true) throw new ArgumentException();

            #endregion

            // Setting
            log4net.MDC.Set("tab", "\t");

            // ConfigFileList
            var configFileList = MDP.IO.File.GetAllFile(configFileName);
            if (configFileList == null) throw new InvalidOperationException("configFileList=null");
            if (configFileList.Count <= 0) throw new InvalidOperationException("configFileList<=0");

            // Repository
            var repository = LogManager.GetRepository();
            if (repository == null) throw new InvalidOperationException("repository=null");

            // Configure
            foreach (var configFile in configFileList)
            {
                XmlConfigurator.Configure(repository, configFile);
            }
        }
    }
}
