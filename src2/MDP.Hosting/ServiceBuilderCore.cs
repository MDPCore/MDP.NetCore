using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

namespace MDP.Hosting
{
    public abstract class ServiceBuilderCore<THostBuilder> where THostBuilder : class
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

        protected string? ServiceNamespace { get; set; } = String.Empty;

        protected string? ServiceName { get; set; } = String.Empty;


        // Methods
        internal void ConfigureContainer(THostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
           
            #endregion

            // NamespaceConfigKey            
            var namespaceConfigKey = this.ServiceNamespace;
            if (string.IsNullOrEmpty(namespaceConfigKey) == true) throw new InvalidOperationException($"{nameof(namespaceConfigKey)}=null");

            // NamespaceConfig
            var namespaceConfig = this.Configuration.GetSection(namespaceConfigKey);           
            if (namespaceConfig == null) return;
            if (namespaceConfig.Exists() == false) return;
            if (string.IsNullOrEmpty(this.ServiceName) == true)
            {
                // Register
                this.RegisterService(hostBuilder, namespaceConfig);

                // Return
                return;
            }

            // ServiceConfigList
            var serviceConfigList = namespaceConfig.GetChildren();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // ServiceConfigKey
            var serviceConfigKey = this.ServiceName;
            if (string.IsNullOrEmpty(serviceConfigKey) == true) throw new InvalidOperationException($"{nameof(serviceConfigKey)}=null");

            // ServiceConfig
            foreach (var serviceConfig in serviceConfigList)
            {
                // DefaultServiceConfig
                if (serviceConfig.Key == serviceConfigKey)
                {
                    // Register
                    this.RegisterService(hostBuilder, serviceConfig);

                    // Continue
                    continue;
                }

                // NamedServiceConfig
                if (serviceConfig.Key.StartsWith(serviceConfigKey) == true)
                {
                    // Register
                    this.RegisterService(hostBuilder, serviceConfig);

                    // Continue
                    continue;
                }
            }
        }

        internal abstract void RegisterService(THostBuilder hostBuilder, IConfiguration serviceConfig);
    }
}