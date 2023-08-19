using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;

namespace MDP.Hosting
{
    public static class ContainerBuilderExtensions
    {
        // Methods
        public static TContainerBuilder RegisterModule<TContainerBuilder>(this TContainerBuilder containerBuilder, IConfiguration configuration) where TContainerBuilder : class
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // FactoryRegister
            FactoryRegister.RegisterModule(containerBuilder, configuration);

            // ServiceRegister
            if (typeof(TContainerBuilder).IsAssignableTo(typeof(IServiceCollection)) == true)
            {
                // RegisterModule
                ServiceRegister.RegisterModule((IServiceCollection)containerBuilder, configuration);
            }

            // DefaultRegister
            if (typeof(TContainerBuilder).IsAssignableTo(typeof(IServiceCollection)) == true)
            {
                // List
                ((IServiceCollection)containerBuilder).TryAddTransient(typeof(IList<>), typeof(List<>));
            }

            // Return
            return containerBuilder;
        }
    }
}
