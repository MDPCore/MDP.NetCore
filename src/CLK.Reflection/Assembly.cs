using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Reflection
{
    public static class Assembly
    {
        // Methods
        public static List<System.Reflection.Assembly> GetAllAssembly(string fileName, string searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException();

            #endregion

            // Result
            var assemblyList = new List<System.Reflection.Assembly>();

            // AssemblyFileList
            var assemblyFileList = CLK.IO.File.GetAllFile(fileName, searchPath);
            if (assemblyFileList == null) throw new InvalidOperationException($"{nameof(assemblyFileList)}=null");

            // AssemblyList 
            foreach (var moduleAssemblyFile in assemblyFileList)
            {
                // Assembly
                var assembly = System.Reflection.Assembly.LoadFrom(moduleAssemblyFile.FullName);
                if (assembly == null) throw new InvalidOperationException($"{nameof(assembly)}=null");

                // Add
                assemblyList.Add(assembly);
            }

            // Return
            return assemblyList;
        }
    }
}
