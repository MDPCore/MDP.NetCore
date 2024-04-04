using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MDP.Reflection
{
    public static class Assembly
    {
        // Fields
        private readonly static object _syncLock = new object();

        private static IList<System.Reflection.Assembly> _assemblyList = null;

        private static IList<System.Reflection.Assembly> _applicationAssemblyList = null;


        // Methods
        public static IList<System.Reflection.Assembly> FindAllAssembly()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_assemblyList != null) return _assemblyList;

                //  LoadedAssemblyList
                var loadedAssemblyList = AppDomain.CurrentDomain.GetAssemblies();
                if (loadedAssemblyList == null) throw new InvalidOperationException($"{nameof(loadedAssemblyList)}=null");

                // LoadedAssemblyDictionary
                var loadedAssemblyDictionary = new Dictionary<string, System.Reflection.Assembly>(StringComparer.OrdinalIgnoreCase);
                foreach (var assembly in loadedAssemblyList)
                {
                    loadedAssemblyDictionary[assembly.Location] = assembly;
                }

                // AssemblyFilePathList
                var assemblyFilePathList = MDP.IO.File.GetAllFilePath("*.dll");
                if (assemblyFilePathList == null) throw new InvalidOperationException($"{nameof(assemblyFilePathList)}=null");

                // AssemblyList 
                var assemblyList = new List<System.Reflection.Assembly>();
                foreach (var assemblyFilePath in assemblyFilePathList)
                {
                    if (loadedAssemblyDictionary.ContainsKey(assemblyFilePath) == true)
                    {
                        // Assembly
                        var assembly = loadedAssemblyDictionary[assemblyFilePath];
                        if (assembly == null) throw new InvalidOperationException($"{nameof(assembly)}=null");

                        // Add
                        assemblyList.Add(assembly);
                    }
                    else
                    {
                        // Assembly
                        var assembly = System.Reflection.Assembly.LoadFrom(assemblyFilePath);
                        if (assembly == null) throw new InvalidOperationException($"{nameof(assembly)}=null");

                        // Add
                        assemblyList.Add(assembly);
                    }
                }

                // EntryAssembly
                var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
                if (assemblyList.Contains(entryAssembly) == false) assemblyList.Add(entryAssembly);

                // Attach
                _assemblyList = assemblyList;

                // Return
                return _assemblyList;
            }           
        }

        public static IList<System.Reflection.Assembly> FindAllApplicationAssembly()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_applicationAssemblyList != null) return _applicationAssemblyList;

                // AssemblyList
                var assemblyList = Assembly.FindAllAssembly();
                if (assemblyList == null) throw new InvalidOperationException($"{nameof(assemblyList)}=null");

                // Filter
                assemblyList = assemblyList.Where(assembly =>
                {
                    // Require
                    if (assembly == null) return false;

                    // Filter
                    if (assembly.FullName.StartsWith("Microsoft") == true) return false;
                    if (assembly.FullName.StartsWith("System") == true) return false;

                    // Return
                    return true;
                }).ToList();

                // Attach
                _applicationAssemblyList = assemblyList;

                // Return
                return _applicationAssemblyList;
            }
        }
    }
}
