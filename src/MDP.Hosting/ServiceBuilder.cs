using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

namespace MDP.Hosting
{
    public abstract class ServiceBuilder<THostBuilder> : ServiceBuilderCore<THostBuilder>
        where THostBuilder : class
    {
        // Methods
        internal override void RegisterService(THostBuilder hostBuilder, IConfiguration serviceConfig)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (serviceConfig == null) throw new ArgumentException($"{nameof(serviceConfig)}=null");

            #endregion

            // Configure
            this.ConfigureService(hostBuilder);
        }

        protected abstract void ConfigureService(THostBuilder hostBuilder);
    }

    public abstract class ServiceBuilder<THostBuilder, TSetting> : ServiceBuilderCore<THostBuilder>
        where THostBuilder : class
        where TSetting : class, new()
    {
        // Methods
        internal override void RegisterService(THostBuilder hostBuilder, IConfiguration serviceConfig)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (serviceConfig == null) throw new ArgumentException($"{nameof(serviceConfig)}=null");

            #endregion

            // Setting
            var serviceSetting = new TSetting();
            ConfigurationBinder.Bind(serviceConfig, serviceSetting);

            // Configure
            this.ConfigureService(hostBuilder, serviceSetting);
        }

        protected abstract void ConfigureService(THostBuilder hostBuilder, TSetting setting);
    }
}
