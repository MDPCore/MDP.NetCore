using Autofac;
using Autofac.Builder;
using CLK.Autofac;
using Microsoft.Extensions.Configuration;
using System;

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
            return container.RegisterInterface<IConfiguration, TService>(configuration =>
            {
                // ServiceType
                var serviceType = configuration.GetServiceType<TService>();
                if (string.IsNullOrEmpty(serviceType) == true) throw new InvalidOperationException($"{nameof(serviceType)}=null");

                // Return
                return serviceType;
            });
        }
    }
}