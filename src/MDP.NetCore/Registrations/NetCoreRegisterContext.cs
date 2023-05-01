using Autofac;
using MDP.Hosting;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    internal class NetCoreRegisterContext : RegisterContext, IDisposable
    {
        // Constructors
        public NetCoreRegisterContext()
        {

        }

        public void Dispose()
        {

        }


        // Methods
        public void RegisterModule(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");

            #endregion

            // FactoryTypeList
            var factoryTypeList = CLK.Reflection.Type.FindAllType();
            if (factoryTypeList == null) throw new InvalidOperationException($"{nameof(factoryTypeList)}=null");

            // FactoryType
            foreach (var factoryType in factoryTypeList)
            {
                // FactoryAttribute
                var factoryAttribute = factoryType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(FactoryAttribute<,>)).OfType<FactoryAttribute>().FirstOrDefault();
                if (factoryAttribute == null) continue;
                if (factoryAttribute.BuilderType != typeof(IServiceCollection)) continue;

                // ServiceName
                if (string.IsNullOrEmpty(factoryAttribute.ServiceName) == true)
                {
                    // FactoryConfig
                    var factoryConfig = this.FindServiceConfig(configuration, factoryAttribute.ServiceNamespace);
                    if (factoryConfig == null) continue;

                    // ConfigureService
                    this.ConfigureService(serviceCollection, factoryType, factoryConfig, factoryAttribute);
                }
                else
                {
                    // FactoryConfigList
                    var factoryConfigList = this.FindAllServiceConfig(configuration, factoryAttribute.ServiceNamespace);
                    if (factoryConfigList == null) throw new InvalidOperationException($"{nameof(factoryConfigList)}=null");

                    // FactoryConfig
                    foreach (var factoryConfig in factoryConfigList)
                    {
                        if (factoryConfig.Key.StartsWith(factoryAttribute.ServiceName, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            // ConfigureService
                            this.ConfigureService(serviceCollection, factoryType, factoryConfig, factoryAttribute);
                        }
                    }
                }
            }
        }

        private void ConfigureService(IServiceCollection serviceCollection, Type factoryType, IConfigurationSection factoryConfig, FactoryAttribute factoryAttribute)
        {
            #region Contracts

            if (serviceCollection == null) throw new ArgumentException($"{nameof(serviceCollection)}=null");
            if (factoryType == null) throw new ArgumentException($"{nameof(factoryType)}=null");
            if (factoryConfig == null) throw new ArgumentException($"{nameof(factoryConfig)}=null");
            if (factoryAttribute == null) throw new ArgumentException($"{nameof(factoryAttribute)}=null");

            #endregion

            // Require
            if (factoryType.IsAbstract == true) return;
            
            // FactoryMethod
            var ractoryMethod = factoryType.GetMethod("ConfigureService");
            if (ractoryMethod == null) throw new InvalidOperationException($"Factory.RegisterService(IServiceCollection, TSetting) not found.");

            // ParameterList
            var parameterList = ractoryMethod.GetParameters();
            if (parameterList == null) throw new InvalidOperationException($"{nameof(parameterList)}=null");
            if (parameterList.Length != 2) throw new InvalidOperationException($"Factory.RegisterService(IServiceCollection, TSetting) not found.");
            if (parameterList[0].ParameterType != typeof(IServiceCollection)) throw new InvalidOperationException($"Factory.RegisterService(IServiceCollection, TSetting) not found.");
            if (parameterList[1].ParameterType != factoryAttribute.SettingType) throw new InvalidOperationException($"Factory.RegisterService(IServiceCollection, TSetting) not found.");

            // FactorySetting
            var factorySetting = Activator.CreateInstance(factoryAttribute.SettingType);
            if (factorySetting == null) throw new InvalidOperationException($"{nameof(factorySetting)}=null");
            ConfigurationBinder.Bind(factoryConfig, factorySetting);

            // Factory
            var factory = Activator.CreateInstance(factoryType);
            if (factory == null) throw new InvalidOperationException($"{nameof(factory)}=null");

            // ConfigureService
            ractoryMethod.Invoke(factory, new object[] { serviceCollection, factorySetting });
        }
    }
}
