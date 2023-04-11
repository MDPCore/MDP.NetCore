using Autofac;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract partial class RegisterContext
    {
        // Fields
        private readonly static object _syncLock = new object();

        private static List<Assembly>? _moduleAssemblyList = null;

        private static List<Type>? _moduleTypeList = null;


        // Methods
        protected List<Type> FindAllModuleType()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_moduleTypeList != null) return _moduleTypeList;

                // ModuleAssemblyList
                var moduleAssemblyList = this.FindAllModuleAssembly();
                if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

                // ModuleTypeList
                var moduleTypeList = new List<Type>();
                foreach (var moduleAssembly in moduleAssemblyList)
                {
                    moduleTypeList.AddRange(moduleAssembly.GetTypes());
                }

                // Attach
                _moduleTypeList = moduleTypeList;

                // Return
                return _moduleTypeList;
            }
        }

        private List<Assembly> FindAllModuleAssembly()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_moduleAssemblyList != null) return _moduleAssemblyList;

                // ModuleAssembly
                var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(@"*.dll");
                if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

                // EntryAssembly
                var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
                if (moduleAssemblyList.Contains(entryAssembly) == false) moduleAssemblyList.Add(entryAssembly);

                // Attach
                _moduleAssemblyList = moduleAssemblyList;

                // Return
                return _moduleAssemblyList;
            }
        }


        protected IConfigurationSection? FindServiceConfig(IConfiguration configuration, string serviceNamespace, string serviceName)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");
            if (string.IsNullOrEmpty(serviceName) == true) throw new ArgumentException($"{nameof(serviceName)}=null");

            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return null;
            if (namespaceConfig.Exists() == false) return null;

            // ServiceConfig
            var serviceConfig = namespaceConfig.GetSection(serviceName);
            if (serviceConfig == null) return null;
            if (serviceConfig.Exists() == false) return null;

            // Return
            return serviceConfig;
        }

        protected List<IConfigurationSection> FindAllServiceConfig(IConfiguration configuration, string serviceNamespace)
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(serviceNamespace) == true) throw new ArgumentException($"{nameof(serviceNamespace)}=null");
          
            #endregion

            // NamespaceConfig
            var namespaceConfig = configuration.GetSection(serviceNamespace);
            if (namespaceConfig == null) return new List<IConfigurationSection>();
            if (namespaceConfig.Exists() == false) return new List<IConfigurationSection>();

            // ServiceConfigList
            var serviceConfigList = namespaceConfig.GetChildren();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // Return
            return serviceConfigList.ToList();
        }
    }
}