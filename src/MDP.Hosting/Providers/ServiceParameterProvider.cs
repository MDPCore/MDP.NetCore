using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using MDP.Registration;

namespace MDP.Hosting
{
    public class ServiceParameterProvider : MDP.Reflection.ParameterProvider
    {
        // Fields
        private readonly IServiceProvider _serviceProvider = null;

        private readonly MDP.Reflection.ParameterProvider _parameterProvider = null;


        // Constructors
        public ServiceParameterProvider(IServiceProvider serviceProvider, MDP.Reflection.ParameterProvider parameterProvider)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (parameterProvider == null) throw new ArgumentException($"{nameof(parameterProvider)}=null");

            #endregion

            // Default
            _serviceProvider = serviceProvider;
            _parameterProvider = parameterProvider;
        }


        // Methods
        public override object GetValue(Type parameterType, string parameterName, bool hasDefaultValue = false, object defaultValue = null)
        {
            #region Contracts

            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");
            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");

            #endregion

            // ReferenceType
            if (parameterType.IsClass == true || parameterType.IsInterface == true)
            {
                // Resolve
                var instanceName = _parameterProvider.GetValue(typeof(string), parameterName, false, null) as string;
                if (string.IsNullOrEmpty(instanceName) == true)
                {
                    // ResolveTyped
                    if (_serviceProvider.TryResolveTyped(parameterType, out var parameter) == true)
                    {
                        // Return
                        return parameter;
                    }
                }
                else
                {
                    // ResolveNamed
                    if (_serviceProvider.TryResolveNamed(parameterType, instanceName, out var parameter) == true)
                    {
                        // Return
                        return parameter;
                    }
                }
            }

            // Return
            return _parameterProvider.GetValue(parameterType, parameterName, hasDefaultValue, defaultValue);
        }
    }
}
