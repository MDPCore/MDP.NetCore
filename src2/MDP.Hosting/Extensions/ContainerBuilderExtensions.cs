using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;

namespace MDP.Hosting
{
    public static class ContainerBuilderExtensions
    {
        // Methods   
        public static ContainerBuilder RegisterModule(this ContainerBuilder containerBuilder, IConfiguration configuration, string moduleAssemblyFileName = @"*.dll")
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException($"{nameof(containerBuilder)}=null");
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
                // ServiceFactory
                moduleAssemblyList.ForEach(moduleAssembly =>
                {
                    moduleContainerBuilder
                        .RegisterAssemblyTypes(moduleAssembly)
                        .Where(assemblyType => typeof(ServiceFactoryCore).IsAssignableFrom(assemblyType))
                        .As<ServiceFactoryCore>();
                });
            }

            // RegisterModule
            using (var moduleContainer = moduleContainerBuilder.Build())
            {
                // ServiceFactory
                foreach (var serviceFactory in moduleContainer.Resolve<IEnumerable<ServiceFactoryCore>>())
                {
                    // Initialize
                    serviceFactory.Initialize(configuration);

                    // Register
                    containerBuilder.RegisterModule(serviceFactory);
                }
            }

            // Return
            return containerBuilder;
        }
    }
}