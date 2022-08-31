using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

namespace MDP.Hosting
{
    public abstract class ServiceFactory<TService, TInstance> : ServiceFactoryCore<TService, TInstance>
        where TService : class
        where TInstance : TService
    {
        // Methods
        internal override IRegistrationBuilder<TInstance, SimpleActivatorData, SingleRegistrationStyle> RegisterService(ContainerBuilder container, IConfiguration serviceConfig)
        {
            #region Contracts

            if (container == null) throw new ArgumentException($"{nameof(container)}=null");
            if (serviceConfig == null) throw new ArgumentException($"{nameof(serviceConfig)}=null");

            #endregion

            // Register
            var registration = container.Register((componentContext, parameterList) =>
            {
                // Create
                var service = this.CreateService(componentContext);
                if (service == null) throw new InvalidOperationException($"{nameof(service)}=null");

                // Return
                return service;
            });
            if (registration == null) throw new InvalidOperationException($"{nameof(registration)}=null");

            // Return
            return registration;
        }

        protected abstract TInstance CreateService(IComponentContext componentContext);
    }

    public abstract class ServiceFactory<TService, TInstance, TSetting> : ServiceFactoryCore<TService, TInstance>
        where TService : class
        where TInstance : TService
        where TSetting : class, new()
    {
        // Methods
        internal override IRegistrationBuilder<TInstance, SimpleActivatorData, SingleRegistrationStyle> RegisterService(ContainerBuilder container, IConfiguration serviceConfig)
        {
            #region Contracts

            if (container == null) throw new ArgumentException($"{nameof(container)}=null");
            if (serviceConfig == null) throw new ArgumentException($"{nameof(serviceConfig)}=null");

            #endregion

            // Register
            var registration = container.Register((componentContext, parameterList) =>
            {
                // Setting
                var serviceSetting = new TSetting();
                ConfigurationBinder.Bind(serviceConfig, serviceSetting);

                // Create
                var service = this.CreateService(componentContext, serviceSetting);
                if (service == null) throw new InvalidOperationException($"{nameof(service)}=null");

                // Return
                return service;
            });
            if (registration == null) throw new InvalidOperationException($"{nameof(registration)}=null");

            // Return
            return registration;
        }

        protected abstract TInstance CreateService(IComponentContext componentContext, TSetting setting);
    }
}
