using CLK.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public class ConfigurationParameterDictionary<TService> : ParameterDictionary
         where TService : class
    {
        // Fields
        private Configuration<TService> _configuration = null;


        // Constructors
        public ConfigurationParameterDictionary(Configuration<TService> configuration)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Default
            _configuration = configuration;
        }


        // Methods
        public object GetValue(string key, Type type)
        {
            #region Contracts

            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException(nameof(key));
            if (type == null) throw new ArgumentException(nameof(type));

            #endregion

            // Return
            return _configuration.GetValue(key, type);
        }
    }
}
