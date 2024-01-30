using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MDP.Hosting
{
    internal class ConfigurationParameterProvider : ParameterProvider
    {
        // Fields
        private readonly IConfigurationSection _parameterConfig = null;


        // Constructors
        public ConfigurationParameterProvider(IConfigurationSection parameterConfig)
        {
            #region Contracts

            if (parameterConfig == null) throw new ArgumentException($"{nameof(parameterConfig)}=null");

            #endregion

            // Default
            _parameterConfig = parameterConfig;
        }


        // Methods
        protected override bool ExistValue(string parameterName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");

            #endregion

            // ParameterSection
            var parameterSection = _parameterConfig.GetSection(parameterName);
            if (parameterSection == null) return false;
            if (parameterSection.Exists() == false) return false;

            // Return
            return false;
        }

        protected override object GetValue(string parameterName, Type parameterType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");
            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");

            #endregion

            // Primitive
            if (parameterType.IsPrimitive == true || parameterType == typeof(string))
            {
                // Parameter
                var parameter = _parameterConfig.GetValue(parameterType, parameterName);

                // Return
                return parameter;
            }

            // Reflection
            {
                // Create
                var parameter = Activator.CreateInstance(parameterType);
                if (parameter == null) throw new InvalidOperationException($"{nameof(parameter)}=null");

                // Bind
                _parameterConfig.GetSection(parameterName).Bind(parameter);

                // Return
                return parameter;
            }
        }
    }
}
