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

                // AssemblyDictionary
                var assemblyDictionary = new Dictionary<string, System.Reflection.Assembly>(StringComparer.OrdinalIgnoreCase);

                //  LoadedAssemblyList
                var loadedAssemblyList = AppDomain.CurrentDomain.GetAssemblies();
                if (loadedAssemblyList == null) throw new InvalidOperationException($"{nameof(loadedAssemblyList)}=null");
                foreach (var loadedAssembly in loadedAssemblyList)
                {
                    if (assemblyDictionary.ContainsKey(loadedAssembly.Location) == false)
                    {
                        assemblyDictionary[loadedAssembly.Location] = loadedAssembly;
                    }
                }

                // FileAssemblyPathList
                var fileAssemblyPathList = MDP.IO.File.GetAllFilePath("*.dll");
                if (fileAssemblyPathList == null) throw new InvalidOperationException($"{nameof(fileAssemblyPathList)}=null");
                foreach (var fileAssemblyPath in fileAssemblyPathList)
                {
                    if (assemblyDictionary.ContainsKey(fileAssemblyPath) == false)
                    {
                        // Filter
                        if (IsApplicationAssembly(fileAssemblyPath) == false) continue;

                        // FileAssembly
                        var fileAssembly = System.Reflection.Assembly.LoadFrom(fileAssemblyPath);
                        if (fileAssembly == null) throw new InvalidOperationException($"{nameof(fileAssembly)}=null");

                        // Add
                        assemblyDictionary[fileAssembly.Location] = fileAssembly;
                    }
                }

                // EntryAssembly
                var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (entryAssembly != null)
                {
                    if (assemblyDictionary.ContainsKey(entryAssembly.Location) == false)
                    {
                        assemblyDictionary[entryAssembly.Location] = entryAssembly;
                    }
                }

                // AssemblyList 
                var assemblyList = assemblyDictionary.Values.Where(assembly =>
                {
                    // Filter
                    if (IsApplicationAssembly(assembly.Location)==false) return false;

                    // Return 
                    return true;
                }).ToList();

                // Attach
                _applicationAssemblyList = assemblyList;

                // Return
                return _applicationAssemblyList;
            }           
        }

        private static bool IsApplicationAssembly(string assemblyLocation)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNullOrEmpty(assemblyLocation);

            #endregion

            // AssemblyName
            var assemblyName = System.IO.Path.GetFileName(assemblyLocation);
            if (string.IsNullOrEmpty(assemblyName) == true) throw new InvalidOperationException($"{nameof(assemblyName)}=null");

            // Filter
            if (assemblyName.StartsWith("Azure") == true) return false;
            if (assemblyName.StartsWith("System") == true) return false;
            if (assemblyName.StartsWith("Microsoft") == true) return false;
            if (assemblyName.StartsWith("_Microsoft") == true) return false;

            // Filter(MAUI)
            if (assemblyName.StartsWith("Xamarin") == true) return false;
            if (assemblyName.StartsWith("Mono") == true) return false;

            // Filter(MAUI.WinRT)
            if (assemblyName.StartsWith("WinRT") == true) return false;

            // Filter(MAUI.Android)
            if (assemblyName.StartsWith("Java") == true) return false;
            if (assemblyName.StartsWith("Google") == true) return false;
            if (assemblyName.StartsWith("Windows") == true) return false;
            if (assemblyName.StartsWith("mscorlib.dll") == true) return false;
            if (assemblyName.StartsWith("netstandard.dll") == true) return false;
            if (assemblyName.StartsWith("Jsr305Binding.dll") == true) return false;

            // Return
            return true;
        }
    }
}
