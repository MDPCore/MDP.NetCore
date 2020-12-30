using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLogLib = NLog;

namespace MDP.Logging.NLog
{
    public class NLogLoggerProvider : LoggerProvider
    {
        // Constructors
        public NLogLoggerProvider(string configFileName = null, List<NLogTargetFactory> targetFactoryList = null)
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
            if (targetFactoryList != null) this.Initialize(targetFactoryList);
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
            return new NLogLogger<TCategory>();
        }


        private void Initialize()
        {
           
        }

        private void Initialize(string configFileName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(configFileName) == true) throw new ArgumentException();

            #endregion

            // ConfigFileList
            var configFileList = MDP.IO.File.GetAllFile(configFileName);
            if (configFileList == null) throw new InvalidOperationException("configFileList=null");
            if (configFileList.Count <= 0) throw new InvalidOperationException("configFileList<=0");
            
            // Configure
            foreach (var configFile in configFileList)
            {
                NLogLib.LogManager.Configuration = new NLogLib.Config.XmlLoggingConfiguration(configFile.FullName, true);
            }
        }

        private void Initialize(List<NLogTargetFactory> targetFactoryList)
        {
            #region Contracts

            if (targetFactoryList == null) throw new ArgumentException();

            #endregion

            // Process
            foreach (var target in NLogLib.LogManager.Configuration.AllTargets)
            {
                foreach(var targetFactory in targetFactoryList)
                {
                    targetFactory.Process(target);
                }
            }

            // Reconfig
            NLogLib.LogManager.ReconfigExistingLoggers();
        }
    }
}
