using Autofac;
using Autofac.Builder;
using Autofac.Core;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;

namespace MDP.Hosting
{
    public static class ContainerBuilderExtensions
    {
        // Methods   
        public static ContainerBuilder RegisterModule(this ContainerBuilder containerBuilder, IConfiguration configuration)
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // RegisterContext
            var registerContext = new AttributeRegisterContext();
            {
                // RegisterModule
                registerContext.RegisterModule(containerBuilder, configuration);
            }

            // Return
            return containerBuilder;
        }
    }
}
