using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MDP.Hosting
{
    public class ConfigurationParameterProvider : MDP.Reflection.ParameterProvider
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
        public override object GetValue(System.Type parameterType, string parameterName, bool hasDefaultValue = false, object defaultValue = null)
        {
            #region Contracts

            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");
            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");

            #endregion

            // ReferenceType
            if (parameterType.IsClass == true && parameterType.IsAbstract == false)
            {
                if (parameterType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null) != null)
                {
                    // ParameterSection
                    var parameterSection = _parameterConfig.GetSection(parameterName);
                    if (parameterSection != null && parameterSection.Exists() == true)
                    {
                        // Parameter
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
            }

            // ValueType
            if (parameterType.IsValueType == true || parameterType == typeof(string))
            {
                // ParameterSection
                var parameterSection = _parameterConfig;
                if (parameterSection != null && parameterSection.Exists() == true)
                {
                    // Parameter
                    var parameter = parameterSection.GetValue(parameterType, parameterName);
                    if (parameter != null)
                    {
                        // Return
                        return parameter;
                    }
                }
            }

            // Return
            return base.GetValue(parameterType, parameterName, hasDefaultValue, defaultValue);
        }
    }
}
