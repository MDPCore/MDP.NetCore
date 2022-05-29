using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

namespace MDP.Hosting
{
    public abstract class ServiceFactoryCore : Autofac.Module
    {
        // Fields
        private IConfiguration? _configuration = null;


        // Constructors
        internal void Initialize(IConfiguration configuration)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // Default
            _configuration = configuration;
        }


        // Properties
        internal IConfiguration Configuration { get { return _configuration!; } }


        // Methods
        protected override void Load(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException($"{nameof(container)}=null");

            #endregion

            // Base
            base.Load(container);

            // Configure
            this.ConfigureContainer(container);
        }

        internal abstract void ConfigureContainer(ContainerBuilder container);
    }

    public abstract class ServiceFactoryCore<TService, TInstance> : ServiceFactoryCore
        where TService : class
        where TInstance : TService
    {
        // Properties
        protected string? ServiceNamespace { get; set; } = String.Empty;

        protected bool ServiceSingleton { get; set; } = false;


        // Methods
        internal override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException($"{nameof(container)}=null");

            #endregion

            // NamespaceConfigKey            
            var namespaceConfigKey = this.ServiceNamespace;
            if (string.IsNullOrEmpty(namespaceConfigKey) == true) namespaceConfigKey = typeof(TService).Namespace;
            if (string.IsNullOrEmpty(namespaceConfigKey) == true) throw new InvalidOperationException($"{nameof(namespaceConfigKey)}=null");

            // NamespaceConfig
            var namespaceConfig = this.Configuration?.GetSection(namespaceConfigKey);
            if (namespaceConfig == null) return;
            if (namespaceConfig.Exists() == false) return;

            // ServiceConfigList
            var serviceConfigList = namespaceConfig.GetChildren();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // ServiceConfigKey
            var serviceConfigKey = typeof(TInstance).Name;
            if (string.IsNullOrEmpty(serviceConfigKey) == true) throw new InvalidOperationException($"{nameof(serviceConfigKey)}=null");

            // ServiceConfig
            foreach (var serviceConfig in serviceConfigList)
            {
                // DefaultServiceConfig
                if (serviceConfig.Key == serviceConfigKey)
                {
                    // Register
                    var registration = this.RegisterService(container, serviceConfig);
                    if (registration == null) throw new InvalidOperationException($"{nameof(registration)}=null");

                    // Configure
                    this.ConfigureRegistration(registration);
                }

                // NamedServiceConfig
                if (serviceConfig.Key.StartsWith(serviceConfigKey) == true)
                {
                    // Register
                    var registration = this.RegisterService(container, serviceConfig);
                    if (registration == null) throw new InvalidOperationException($"{nameof(registration)}=null");

                    // Configure
                    this.ConfigureRegistration(registration, serviceConfig.Key);
                }
            }
        }

        internal abstract IRegistrationBuilder<TInstance, SimpleActivatorData, SingleRegistrationStyle> RegisterService(ContainerBuilder container, IConfiguration serviceConfig);

        private IRegistrationBuilder<TInstance, SimpleActivatorData, SingleRegistrationStyle> ConfigureRegistration(IRegistrationBuilder<TInstance, SimpleActivatorData, SingleRegistrationStyle> registration, string? serviceName = null)
        {
            #region Contracts

            if (registration == null) throw new ArgumentException($"{nameof(registration)}=null");

            #endregion

            // As
            if (string.IsNullOrEmpty(serviceName) == true)
            {
                registration = registration.As<TService>();
            }
            else
            {
                registration = registration.Named<TService>(serviceName);
            }

            // Singleton
            if (this.ServiceSingleton == true)
            {
                registration = registration.SingleInstance();
            }

            // Return
            return registration;
        }
    }
}
