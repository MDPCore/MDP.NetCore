using Autofac;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public class AttributeRegisterContext : RegisterContext
    {
        // Methods
        public void RegisterModule(ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // RegisterService
            this.RegisterService(containerBuilder, configuration);
        }

        protected override RegisterFactory? CreateRegisterFactory(Type moduleType)
        {
            #region Contracts

            if (moduleType == null) throw new ArgumentException($"{nameof(moduleType)}=null");

            #endregion

            // RegisterAttribute
            var registerAttribute = moduleType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(RegisterAttribute<>)).OfType<RegisterAttribute>().FirstOrDefault();
            if (registerAttribute == null) return null;

            // RegisterFactory
            var registerFactory = new AttributeRegisterFactory(
                registerAttribute.ServiceType,
                moduleType,
                registerAttribute.ServiceNamespace,
                registerAttribute.ServiceSingleton
            );

            // Return
            return registerFactory;
        }
    }
}
