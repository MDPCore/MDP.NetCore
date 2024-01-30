using CLK.ComponentModel;
using System;
using System.Reflection;

namespace MDP.Hosting
{
    internal abstract class ParameterProvider
    {
        // Methods
        protected abstract bool ExistValue(string parameterName);

        protected abstract object GetValue(string parameterName, Type parameterType);


        // Methods
        public object CreateParameter(ParameterInfo parameterInfo, IServiceProvider serviceProvider)
        {
            #region Contracts

            if (parameterInfo == null) throw new ArgumentException($"{nameof(parameterInfo)}=null");
            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // ParameterConfig
            if (this.ExistValue(parameterInfo.Name) == false && parameterInfo.HasDefaultValue == true)
            {
                // DefaultValue
                var parameter = parameterInfo.DefaultValue;

                // Return
                return parameter;
            }

            // Primitive
            if (parameterInfo.ParameterType.IsPrimitive == true || parameterInfo.ParameterType == typeof(string))
            {
                // GetValue
                var parameter = this.GetValue(parameterInfo.Name, parameterInfo.ParameterType);

                // Return
                return parameter;
            }

            // Resolve
            if (parameterInfo.ParameterType.IsInterface == true || parameterInfo.ParameterType.IsClass == true)
            {
                // ParameterName
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
            }

            // Provider
            {
                // GetValue
                var parameter = this.GetValue(parameterInfo.Name, parameterInfo.ParameterType);

                // Return
                return parameter;
            }
        }
    }
}
