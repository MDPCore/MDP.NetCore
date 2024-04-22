using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public partial class ServiceRegistrationRegister
    {
        // Methods
        public static void RegisterModule(IServiceCollection serviceCollection)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");

            #endregion

            // ServiceProvider
            var serviceProvider = serviceCollection.BuildServiceProvider();
            if (serviceProvider == null) throw new InvalidOperationException($"{nameof(serviceProvider)}=null");

            // ServiceRegistrationList
            var serviceRegistrationList = serviceProvider.GetService<IList<ServiceRegistration>>();
            if (serviceRegistrationList == null) throw new InvalidOperationException($"{nameof(serviceRegistrationList)}=null");

            // ServiceRegistration
            foreach (var serviceRegistration in serviceRegistrationList)
            {
                // Require
                if (serviceRegistration.ServiceType == null) throw new InvalidOperationException($"{nameof(serviceRegistration.ServiceType)}=null");
                if (serviceRegistration.InstanceType == null) throw new InvalidOperationException($"{nameof(serviceRegistration.InstanceType)}=null");
                if (string.IsNullOrEmpty(serviceRegistration.InstanceName) == true) throw new InvalidOperationException($"{nameof(serviceRegistration.InstanceName)}=null");
                if (serviceRegistration.Parameters == null) throw new InvalidOperationException($"{nameof(serviceRegistration.Parameters)}=null");

                // RegisterService
                ServiceRegister.RegisterService
                (
                    serviceCollection: serviceCollection,
                    serviceType: serviceRegistration.ServiceType,
                    instanceType: serviceRegistration.InstanceType,
                    instanceName: serviceRegistration.InstanceName,
                    parameterProvider: new MDP.Reflection.DictionaryParameterProvider(serviceRegistration.Parameters),
                    singleton: serviceRegistration.Singleton
                );
            }
        }
    }
}
