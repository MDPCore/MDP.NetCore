using Autofac;
using MDP.Hosting;
using MDP.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    internal class AspNetCoreRegisterContext : RegisterContext, IDisposable
    {
        // Constructors
        public AspNetCoreRegisterContext()
        {

        }

        public void Dispose()
        {

        }


        // Methods
        public void RegisterModule(WebApplicationBuilder webApplicationBuilder)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
           
            #endregion

            // FactoryTypeList
            var factoryTypeList = this.FindAllModuleType();
            if (factoryTypeList == null) throw new InvalidOperationException($"{nameof(factoryTypeList)}=null");

            // FactoryType
            foreach (var factoryType in factoryTypeList)
            {
                // FactoryAttribute
                var factoryAttribute = factoryType.GetCustomAttributes(false).Where(attr => attr.GetType().IsGenericType && attr.GetType().GetGenericTypeDefinition() == typeof(FactoryAttribute<,>)).OfType<FactoryAttribute>().FirstOrDefault();
                if (factoryAttribute == null) continue;
                if (factoryAttribute.BuilderType != typeof(WebApplicationBuilder)) continue;

                // ServiceName
                if (string.IsNullOrEmpty(factoryAttribute.ServiceName) == true)
                {
                    // FactoryConfig
                    var factoryConfig = this.FindServiceConfig(webApplicationBuilder.Configuration, factoryAttribute.ServiceNamespace);
                    if (factoryConfig == null) continue;

                    // ConfigureService
                    this.ConfigureService(webApplicationBuilder, factoryType, factoryConfig, factoryAttribute);
                }
                else
                {
                    // FactoryConfigList
                    var factoryConfigList = this.FindAllServiceConfig(webApplicationBuilder.Configuration, factoryAttribute.ServiceNamespace);
                    if (factoryConfigList == null) throw new InvalidOperationException($"{nameof(factoryConfigList)}=null");

                    // FactoryConfig
                    foreach (var factoryConfig in factoryConfigList)
                    {
                        if (factoryConfig.Key.StartsWith(factoryAttribute.ServiceName, StringComparison.OrdinalIgnoreCase) == true)
                        {
                            // ConfigureService
                            this.ConfigureService(webApplicationBuilder, factoryType, factoryConfig, factoryAttribute);
                        }
                    }
                }
            }
        }

        private void ConfigureService(WebApplicationBuilder webApplicationBuilder, Type factoryType, IConfigurationSection factoryConfig, FactoryAttribute factoryAttribute)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (factoryType == null) throw new ArgumentException($"{nameof(factoryType)}=null");
            if (factoryConfig == null) throw new ArgumentException($"{nameof(factoryConfig)}=null");
            if (factoryAttribute == null) throw new ArgumentException($"{nameof(factoryAttribute)}=null");

            #endregion

            // Require
            if (factoryType.IsAbstract == true) return;
            
            // FactoryMethod
            var ractoryMethod = factoryType.GetMethod("ConfigureService");
            if (ractoryMethod == null) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");

            // ParameterList
            var parameterList = ractoryMethod.GetParameters();
            if (parameterList == null) throw new InvalidOperationException($"{nameof(parameterList)}=null");
            if (parameterList.Length != 2) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");
            if (parameterList[0].ParameterType != typeof(WebApplicationBuilder)) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");
            if (parameterList[1].ParameterType != factoryAttribute.SettingType) throw new InvalidOperationException($"Factory.RegisterService(WebApplicationBuilder, TSetting) not found.");

            // FactorySetting
            var factorySetting = Activator.CreateInstance(factoryAttribute.SettingType);
            if (factorySetting == null) throw new InvalidOperationException($"{nameof(factorySetting)}=null");
            ConfigurationBinder.Bind(factoryConfig, factorySetting);

            // Factory
            var factory = Activator.CreateInstance(factoryType);
            if (factory == null) throw new InvalidOperationException($"{nameof(factory)}=null");

            // ConfigureService
            ractoryMethod.Invoke(factory, new object[] { webApplicationBuilder, factorySetting });
        }
    }
}
