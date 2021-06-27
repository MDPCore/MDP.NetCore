using Autofac;
using Autofac.Builder;
using CLK.Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

            // Register
            return container.Register<IComponentContext, Configuration<TService>, TService>((componentContext, configuration) =>
            {
                // ImplementerName
                var implementerName = configuration.GetValue<string>(ImplementerNameKey);
                if (string.IsNullOrEmpty(implementerName) == true) throw new InvalidOperationException($"{nameof(implementerName)}=null");

                // Resolve
                return componentContext.ResolveNamed<TService>(implementerName);
            });
        }

        public static IRegistrationBuilder<TImplementer, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterImplementer<TService, TImplementer>(this ContainerBuilder container)
            where TService : class
            where TImplementer : class, TService
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // Register
            return container.RegisterType<ConfigurationParameterDictionary<TService>, TImplementer>
            (
                parameter =>
                {
                    return parameter;
                }
            ).Named<TService>(typeof(TImplementer).FullName);
        }
    }
}