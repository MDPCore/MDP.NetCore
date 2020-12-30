using System;
using System.Collections.Generic;
using System.Text;

namespace MDP.Logging
{
    public class LoggerFactory
    {
        // Fields
        private readonly object _syncRoot = new object();

        private readonly IEnumerable<LoggerProvider> _loggerProviderList = null;

        private readonly Dictionary<Type, object> _loggerDictionary = null;


        // Constructors
        internal LoggerFactory(IEnumerable<LoggerProvider> loggerProviderList)
        {
            #region Contracts

            if (loggerProviderList == null) throw new ArgumentException();

            #endregion

            // Default
            _loggerProviderList = loggerProviderList;
            _loggerDictionary = new Dictionary<Type, object>();
        }


        // Methods
        public Logger<TCategory> Create<TCategory>()
        {
            // Sync
            lock (_syncRoot)
            {
                // Create
                if (_loggerDictionary.ContainsKey(typeof(TCategory)) == false)
                {
                    // LoggerList
                    var loggerList = new List<Logger<TCategory>>();
                    foreach (var loggerProvider in _loggerProviderList)
                    {
                        // Logger
                        var logger = loggerProvider.Create<TCategory>();
                        if (logger == null) throw new InvalidOperationException("logger=null");

                        // Add
                        loggerList.Add(logger);
                    }

                    // Add
                    _loggerDictionary.Add(typeof(TCategory), new LoggerCollection<TCategory>(loggerList));
                }

                // Return
                return _loggerDictionary[typeof(TCategory)] as Logger<TCategory>;
            }
        }

        public Logger<TCategory> Create<TCategory>(TCategory category)
        {
            // Return
            return this.Create<TCategory>();
        }
    }
}
