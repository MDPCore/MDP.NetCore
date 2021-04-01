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
            return container.RegisterInterface<IConfiguration<TService>, TService>(configuration =>
            {
                // ImplementerName
                var implementerName = configuration.GetImplementerName();
                if (string.IsNullOrEmpty(implementerName) == true) throw new InvalidOperationException($"{nameof(implementerName)}=null");

                // Return
                return implementerName;
            });
        }
    }
}