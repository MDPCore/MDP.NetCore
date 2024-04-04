using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

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
        protected override object GetValue(string parameterName, Type parameterType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");
            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");

            #endregion

            // ParameterSection
            var parameterSection = _parameterConfig.GetSection(parameterName);
            if (parameterSection == null) return null;
            if (parameterSection.Exists() == false) return null;

            // ClassType
            if (parameterType.IsClass == true && parameterType.IsAbstract == false)
            {
                if (parameterType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null) != null)
                {
                    // Create
                    var parameter = Activator.CreateInstance(parameterType);
                    if (parameter != null)
                    {
                        // Bind
                        parameterSection.Bind(parameter);

                        // Return
                        return parameter;
                    }
                }
            }

            // ValueType
            if (parameterType.IsValueType == true || parameterType == typeof(string))
            {
                // GetValue
                var parameter = _parameterConfig.GetValue(parameterType, parameterName);
                if (parameter != null)
                {
                    // Return
                    return parameter;
                }
            }

            // Unknown
            return null;
        }
    }
}
