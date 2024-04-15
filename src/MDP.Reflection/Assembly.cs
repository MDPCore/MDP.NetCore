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

        private static IList<System.Reflection.Assembly> _applicationAssemblyList = null;


        // Methods
        public static IList<System.Reflection.Assembly> FindAllApplicationAssembly()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_applicationAssemblyList != null) return _applicationAssemblyList;

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
                var assemblyFilePathList = MDP.IO.File.GetAllFilePath("*.dll").Where(assemblyFilePath =>
                {
                    // Filter
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("System") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("Microsoft") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("_Microsoft") == true) return false;

                    // Filter(MAUI)
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("Xamarin") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("Mono") == true) return false;

                    // Filter(MAUI.WinRT)
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("WinRT") == true) return false;

                    // Filter(MAUI.Android)
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("Java") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("Google") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("Windows") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("mscorlib.dll") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("netstandard.dll") == true) return false;
                    if (System.IO.Path.GetFileName(assemblyFilePath)?.StartsWith("Jsr305Binding.dll") == true) return false;

                    // Return
                    return true;
                }).ToList();
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
                if (entryAssembly != null && assemblyList.Contains(entryAssembly) == false)
                {
                    // Add
                    assemblyList.Add(entryAssembly);
                }

                // Attach
                _applicationAssemblyList = assemblyList;

                // Return
                return _applicationAssemblyList;
            }           
        }
    }
}
