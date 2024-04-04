using System;
using System.Reflection;
using MDP.Registration;

namespace MDP.Hosting
{
    internal abstract class ParameterProvider
    {
        // Methods
        protected abstract object GetValue(string parameterName, Type parameterType);


        // Methods
        public object CreateParameter(ParameterInfo parameterInfo, IServiceProvider serviceProvider)
        {
            #region Contracts

            if (parameterInfo == null) throw new ArgumentException($"{nameof(parameterInfo)}=null");
            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // ClassType
            if (parameterInfo.ParameterType.IsClass == true || parameterInfo.ParameterType.IsInterface == true)
            {
                // Resolve
                var parameterName = this.GetValue(parameterInfo.Name, typeof(string)) as string;
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

                // Provider
                {
                    // GetValue
                    var parameter = this.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (parameter != null)
                    {
                        // Return
                        return parameter;
                    }
                }

                // HasDefaultValue
                if (parameterInfo.HasDefaultValue == true)
                {
                    // Return
                    return parameterInfo.DefaultValue;
                }

                // CreateDefaultValue
                if (parameterInfo.ParameterType.IsClass == true && parameterInfo.ParameterType.IsAbstract == false)
                {
                    if( parameterInfo.ParameterType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null) != null)
                    {
                        // Create
                        var parameter = Activator.CreateInstance(parameterInfo.ParameterType);
                        if (parameter != null)
                        {
                            // Return
                            return parameter;
                        }
                    }
                }

                // Null
                return null;
            }

            // ValueType
            if (parameterInfo.ParameterType.IsValueType == true || parameterInfo.ParameterType == typeof(string))
            {
                // Provider
                {
                    // GetValue
                    var parameter = this.GetValue(parameterInfo.Name, parameterInfo.ParameterType);
                    if (parameter != null)
                    {
                        // Return
                        return parameter;
                    }
                }

                // HasDefaultValue
                if (parameterInfo.HasDefaultValue == true)
                {
                    // Return
                    return parameterInfo.DefaultValue;
                }

                // CreateDefaultValue
                {
                    // Create
                    var parameter = Activator.CreateInstance(parameterInfo.ParameterType);
                    if (parameter != null)
                    {
                        // Return
                        return parameter;
                    }
                }
            }

            // Unknown
            return null;
        }
    }
}
