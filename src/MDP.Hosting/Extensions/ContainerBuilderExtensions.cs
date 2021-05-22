using Autofac;
using Autofac.Builder;
using CLK.Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Runtime.InteropServices;

namespace MDP.Hosting
{
    public static partial class ContainerBuilderExtensions
    {
        // Constants
        public const string ImplementerNameKey = "Type";


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
                var implementerName = configuration.GetValue<string>(ImplementerNameKey);
                if (string.IsNullOrEmpty(implementerName) == true) throw new InvalidOperationException($"{nameof(implementerName)}=null");

                // Return
                return implementerName;
            });
        }

        public static IRegistrationBuilder<TImplementer, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterImplementer<TService, TImplementer>(this ContainerBuilder container)
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
                        // ParameterValueString
                        var parameterValueString = configuration.GetValue<string>(parameterInfo.Name);
                        if (string.IsNullOrEmpty(parameterValueString) == true) return false;

                        // Return
                        return true;
                    });
                },

                // ValueProvider
                (parameterInfo, componentContext) =>
                {
                    return componentContext.Build<IConfiguration<TImplementer>, object>(configuration =>
                    {
                        // ParameterValueString
                        var parameterValueString = configuration.GetValue<string>(parameterInfo.Name);
                        if (string.IsNullOrEmpty(parameterValueString) == true) return null;
                        
                        // Return
                        return Convert.ChangeType(parameterValueString, parameterInfo.ParameterType);
                    });
                }

            ).Named<TService>(typeof(TImplementer).FullName);
        }
    }
}