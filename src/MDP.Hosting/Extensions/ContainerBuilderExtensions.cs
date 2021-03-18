using Autofac;
using System;
using System.Reflection;

namespace MDP.Hosting
{
    public static class ContainerBuilderExtensions
    {
        // Methods
        public static void AddModule(this ContainerBuilder container, string moduleAssemblyFileName = @"*.Hosting.dll")
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");
            moduleAssemblyList.ForEach(moduleAssembly => container.RegisterAssemblyModules<MDP.Module>(moduleAssembly));

            // EntryAssembly
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
            container.RegisterAssemblyModules<MDP.Module>(entryAssembly);
        }
    }
}