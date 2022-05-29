namespace CLK.IO
{
    public static class Directory
    {
        // Methods
        public static string GetEntryDirectory()
        {
            // EntryAssembly
            System.Reflection.Assembly? entryAssembly = null;
            if (entryAssembly == null) entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly == null) entryAssembly = System.Reflection.Assembly.GetAssembly(typeof(Directory));
            if (entryAssembly == null) throw new InvalidOperationException("entryAssembly=null");

            // EntryAssemblyPath
            var entryAssemblyPath = entryAssembly.Location;
            if (string.IsNullOrEmpty(entryAssemblyPath) == true) throw new InvalidOperationException("entryAssemblyPath=null");

            // EntryDirectoryPath
            var entryDirectoryPath = System.IO.Path.GetDirectoryName(entryAssemblyPath);
            if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException("entryDirectoryPath=null");

            // Return
            return entryDirectoryPath;
        }

        public static void DeleteReadOnly(string path)
        {
            #region Contracts

            if (string.IsNullOrEmpty(path) == true) throw new ArgumentException();

            #endregion

            // RootDirectory
            if (System.IO.Directory.Exists(path) == false) return;
            var rootDirectory = new System.IO.DirectoryInfo(path);

            // File Delete
            foreach (var file in rootDirectory.GetFiles("*", System.IO.SearchOption.AllDirectories))
            {
                file.IsReadOnly = false;
                file.Delete();
            }

            // Directory Delete
            foreach (var directory in rootDirectory.GetDirectories())
            {
                directory.Delete(true);
            }
            System.IO.Directory.Delete(path);
        }
    }
}
