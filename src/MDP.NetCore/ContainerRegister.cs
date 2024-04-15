using MDP.Configuration;
using MDP.Hosting;
using MDP.Logging;
using MDP.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    public class ContainerRegister
    {
        // Methods
        public static void RegisterModule(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // Logger
            serviceCollection.TryAddSingleton(typeof(ILogger<>), typeof(LoggerAdapter<>));

            // Tracer
            serviceCollection.TryAddSingleton(typeof(ITracer<>), typeof(TracerAdapter<>));

            // List
            serviceCollection.TryAddTransient(typeof(IList<>), typeof(List<>));

            // ServiceFactoryRegister
            ServiceFactoryRegister.RegisterModule(serviceCollection, configuration);

            // ServiceAttributeRegister
            ServiceAttributeRegister.RegisterModule(serviceCollection, configuration);

            // ServiceRegistrationRegister
            ServiceRegistrationRegister.RegisterModule(serviceCollection);
        }
    }
}
