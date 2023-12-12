using System;
using System.Reflection;

namespace MDP.Hosting
{
    internal class ParameterActivator
    {
        // Methods
        public static object CreateParameter(ParameterInfo parameterInfo, ParameterProvider parameterProvider, IServiceProvider serviceProvider)
        {
            #region Contracts

            if (parameterInfo == null) throw new ArgumentException($"{nameof(parameterInfo)}=null");
            if (parameterProvider == null) throw new ArgumentException($"{nameof(parameterProvider)}=null");
            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // ParameterConfig
            if (parameterInfo.HasDefaultValue == true && parameterProvider.Exists(parameterInfo.Name) == false)
            {
                // Return
                return parameterInfo.DefaultValue;
            }

            // Primitive
            if (parameterInfo.ParameterType.IsPrimitive == true || parameterInfo.ParameterType == typeof(string))
            {
                // Parameter
                var parameter = parameterProvider.GetValue(parameterInfo.Name, parameterInfo.ParameterType);

                // Return
                return parameter;
            }

            // Resolve
            if (parameterInfo.ParameterType.IsInterface == true || parameterInfo.ParameterType.IsClass == true)
            {
                // ParameterName
                var parameterName = parameterProvider.GetValue(parameterInfo.Name, typeof(string)) as string;
                if (string.IsNullOrEmpty(parameterName) == true)
                {
                    // ResolveTyped
                    if (serviceProvider.TryResolveTyped(parameterInfo.ParameterType, out var parameter) == true)
                    {
                        // Return
                        return parameter;
                    }
                }
                else
                {
                    // ResolveNamed
                    if (serviceProvider.TryResolveNamed(parameterInfo.ParameterType, parameterName, out var parameter) == true)
                    {
                        // Return
                        return parameter;
                    }
                }
            }

            // Reflection
            {
                // Parameter
                var parameter = parameterProvider.GetValue(parameterInfo.Name, parameterInfo.ParameterType);

                // Return
                return parameter;
            }
        }
    }
}
