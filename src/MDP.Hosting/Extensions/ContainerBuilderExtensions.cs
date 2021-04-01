using Autofac;
using Autofac.Builder;
using CLK.Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Runtime.InteropServices;

namespace MDP
{
    public static partial class ContainerBuilderExtensions
    {
        // Methods 
        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterInterface<TService>(this ContainerBuilder container)
            where TService : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // RegisterInterface
            return container.RegisterInterface<IConfiguration<TService>, TService>(configuration =>
            {
                // ImplementerName
                var implementerName = configuration.GetImplementerName();
                if (string.IsNullOrEmpty(implementerName) == true) throw new InvalidOperationException($"{nameof(implementerName)}=null");

                // Return
                return implementerName;
            });
        }

        public static IRegistrationBuilder<TImplementer, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterImplementerXXX<TService, TImplementer>(this ContainerBuilder container)
            where TService : class
            where TImplementer : class, TService
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // Return
            return container.RegisterType<TImplementer>().WithParameter
            (
                // ParameterSelector
                (parameterInfo, componentContext) =>
                {
                    return componentContext.Build<IConfiguration<TImplementer>, bool>(configuration =>
                    {
                        // ParameterValue
                        var parameterValue = configuration.GetValue<string>(parameterInfo.Name);
                        if (string.IsNullOrEmpty(parameterValue) == true) return false;

                        // Return
                        return true;
                    });
                },

                // ValueProvider
                (parameterInfo, componentContext) =>
                {
                    return componentContext.Build<IConfiguration<TImplementer>, object>(configuration =>
                    {                        
                        // ParameterValue
                        var parameterValue = configuration.GetValue<string>(parameterInfo.Name);
                        if (string.IsNullOrEmpty(parameterValue) == true) return null;

                        // Keyword
                        if (String.Equals(parameterInfo.Name, ServiceConfigurationExtensions.ConnectionStringNameKey, StringComparison.OrdinalIgnoreCase)) return configuration.GetConnectionString();

                        // Return
                        return Convert.ChangeType(parameterValue, parameterInfo.ParameterType);
                    });
                }

            ).Named<TService>(typeof(TImplementer).FullName);
        }
    }
}