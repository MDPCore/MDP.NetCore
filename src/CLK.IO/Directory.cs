using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLK.IO
{
    public static class Directory
    {
        // Methods
        public static string GetEntryDirectory()
        {
            // Environment
            if (System.IO.Directory.Exists(AppContext.BaseDirectory) == true) return AppContext.BaseDirectory;

            // EntryAssembly
            System.Reflection.Assembly entryAssembly = null;
            if (entryAssembly == null) entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly == null) entryAssembly = System.Reflection.Assembly.GetAssembly(typeof(Directory));
            if (entryAssembly == null) throw new InvalidOperationException("entryAssembly=null");
           
            // EntryAssemblyPath
            var entryAssemblyPath = Uri.UnescapeDataString((new UriBuilder(entryAssembly.Location)).Path);
            if (string.IsNullOrEmpty(entryAssemblyPath) == true) throw new InvalidOperationException("entryAssemblyPath=null");

            // EntryDirectoryPath
            var entryDirectoryPath = System.IO.Path.GetDirectoryName(entryAssemblyPath);
            if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException("entryDirectoryPath=null");

            // Return
            return entryDirectoryPath;
        }
    }
}
