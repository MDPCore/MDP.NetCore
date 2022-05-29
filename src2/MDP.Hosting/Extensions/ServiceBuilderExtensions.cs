using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public static class ServiceBuilder
    {
        // Methods   
        public static void RegisterModule<THostBuilder>(THostBuilder hostBuilder, IConfiguration configuration, string moduleAssemblyFileName = @"*.dll") where THostBuilder : class
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (configuration == null) throw new ArgumentException($"{nameof(configuration)}=null");
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException($"{nameof(moduleAssemblyFileName)}=null");

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // EntryAssembly
            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
            if (moduleAssemblyList.Contains(entryAssembly) == false) moduleAssemblyList.Add(entryAssembly);

            // RegisterAssemblyTypes
            var moduleContainerBuilder = new ContainerBuilder();
            {
                // ServiceBuilder
                moduleAssemblyList.ForEach(moduleAssembly =>
                {
                    moduleContainerBuilder
                        .RegisterAssemblyTypes(moduleAssembly)
                        .Where(assemblyType => typeof(ServiceBuilderCore<THostBuilder>).IsAssignableFrom(assemblyType))
                        .As<ServiceBuilderCore<THostBuilder>>();
                });
            }

            // RegisterModule
            using (var moduleContainer = moduleContainerBuilder.Build())
            {
                // ServiceBuilder
                foreach (var serviceBuilder in moduleContainer.Resolve<IEnumerable<ServiceBuilderCore<THostBuilder>>>())
                {
                    // Initialize
                    serviceBuilder.Initialize(configuration);

                    // Configure
                    serviceBuilder.ConfigureContainer(hostBuilder);
                }
            }
        }
    }
}
