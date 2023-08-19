using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CLK.Reflection
{
    public static class Assembly
    {
        // Fields
        private readonly static object _syncLock = new object();

        private static ReadOnlyCollection<System.Reflection.Assembly> _assemblyList = null;


        // Methods
        public static IList<System.Reflection.Assembly> FindAllAssembly()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_assemblyList != null) return _assemblyList;

                // AssemblyFileList
                var assemblyFileList = CLK.IO.File.GetAllFile("*.dll");
                if (assemblyFileList == null) throw new InvalidOperationException($"{nameof(assemblyFileList)}=null");

                // AssemblyList 
                var assemblyList = new List<System.Reflection.Assembly>();
                foreach (var assemblyFile in assemblyFileList)
                {
                    // Assembly
                    var assembly = System.Reflection.Assembly.LoadFrom(assemblyFile.FullName);
                    if (assembly == null) throw new InvalidOperationException($"{nameof(assembly)}=null");

                    // Add
                    assemblyList.Add(assembly);
                }

                // EntryAssembly
                var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
                if (assemblyList.Contains(entryAssembly) == false) assemblyList.Add(entryAssembly);

                // Attach
                _assemblyList = assemblyList.AsReadOnly();

                // Return
                return _assemblyList;
            }           
        }
    }
}
