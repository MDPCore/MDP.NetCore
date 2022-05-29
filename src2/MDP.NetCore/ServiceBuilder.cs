using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    public abstract class ServiceBuilder : MDP.Hosting.ServiceBuilder<Tuple<HostBuilderContext, IServiceCollection>>
    {
        // Methods
        protected override void ConfigureService(Tuple<HostBuilderContext, IServiceCollection> hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // HostBuilderContext
            var hostBuilderContext = hostBuilder.Item1;
            if (hostBuilderContext == null) throw new InvalidOperationException($"{nameof(hostBuilderContext)}=null");

            // ServiceCollection
            var serviceCollection = hostBuilder.Item2;
            if (serviceCollection == null) throw new InvalidOperationException($"{nameof(serviceCollection)}=null");

            // ConfigureService
            this.ConfigureService(hostBuilderContext, serviceCollection);
        }

        protected abstract void ConfigureService(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection);
    }

    public abstract class ServiceBuilder<TSetting> : MDP.Hosting.ServiceBuilder<Tuple<HostBuilderContext, IServiceCollection>, TSetting>
        where TSetting : class, new()
    {
        // Methods
        protected override void ConfigureService(Tuple<HostBuilderContext, IServiceCollection> hostBuilder, TSetting setting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // HostBuilderContext
            var hostBuilderContext = hostBuilder.Item1;
            if (hostBuilderContext == null) throw new InvalidOperationException($"{nameof(hostBuilderContext)}=null");

            // ServiceCollection
            var serviceCollection = hostBuilder.Item2;
            if (serviceCollection == null) throw new InvalidOperationException($"{nameof(serviceCollection)}=null");

            // ConfigureService
            this.ConfigureService(hostBuilderContext, serviceCollection, setting);
        }

        protected abstract void ConfigureService(HostBuilderContext hostBuilderContext, IServiceCollection serviceCollection, TSetting setting);
    }
}
