using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Logging
{
    public class LoggerContext : IDisposable
    {
        // Fields
        private readonly IEnumerable<LoggerProvider> _loggerProviderList = null;


        // Constructors
        public LoggerContext(IEnumerable<LoggerProvider> loggerProviderList)
        {
            #region Contracts

            if (loggerProviderList == null) throw new ArgumentException();

            #endregion

            // Default
            _loggerProviderList = loggerProviderList;

            // LoggerFactory
            this.LoggerFactory = new LoggerFactory(loggerProviderList);
        }

        public void Start()
        {
            // LoggerProviderList
            foreach (var loggerProvider in _loggerProviderList)
            {
                loggerProvider.Start();
            }
        }

        public void Dispose()
        {
            // LoggerProviderList
            foreach (var loggerProvider in _loggerProviderList)
            {
                loggerProvider.Dispose();
            }
        }


        // Properties
        public LoggerFactory LoggerFactory { get; private set; }


        // Methods
        public Logger<TCategory> Create<TCategory>()
        {
            // Return
            return this.LoggerFactory.Create<TCategory>();
        }

        public Logger<TCategory> Create<TCategory>(TCategory category)
        {
            // Return
            return this.Create<TCategory>();
        }
    }
}
