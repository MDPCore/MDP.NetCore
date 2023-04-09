namespace CLK.IO
{
    public static class File
    {
        // Methods
        public static FileInfo? GetFile(string fileName, string? searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion

            // Return
            return GetAllFile(fileName, searchPath).FirstOrDefault();
        }

        public static List<FileInfo> GetAllFile(string fileName, string? searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion

            // Result
            var resultFileDictionary = new Dictionary<string, FileInfo>();

            // SearchPath
            if (string.IsNullOrEmpty(searchPath) == true)
            {
                // EntryDirectoryPath
                var entryDirectoryPath = Directory.GetEntryDirectory();
                if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException("entryDirectoryPath=null");

                // Setting
                searchPath = entryDirectoryPath;
            }
            if (System.IO.Directory.Exists(searchPath) == false) throw new InvalidOperationException("searchPath is not exists");

            // SearchPatternList
            var searchPatternList = fileName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (searchPatternList == null) throw new InvalidOperationException();

            // Search
            foreach (var searchPattern in searchPatternList)
            {
                // SearchDirectoryPath
                var searchDirectoryPath = System.IO.Path.GetDirectoryName(Path.Combine(searchPath, searchPattern));
                if (string.IsNullOrEmpty(searchDirectoryPath) == true) throw new InvalidOperationException($"{nameof(searchDirectoryPath)}=null");

                // SearchDirectory
                var searchDirectory = new DirectoryInfo(searchDirectoryPath);
                if (searchDirectory == null) throw new InvalidOperationException($"{nameof(searchDirectory)}=null");
                if (searchDirectory.Exists == false) continue;

                // SearchFileList
                var searchFileList = searchDirectory.GetFiles(System.IO.Path.GetFileName(Path.Combine(searchPath, searchPattern)), SearchOption.TopDirectoryOnly);
                if (searchFileList == null) throw new InvalidOperationException();

                // Add
                foreach (var searchFile in searchFileList)
                {
                    if (resultFileDictionary.ContainsKey(searchFile.FullName.ToLower()) == false)
                    {
                        resultFileDictionary.Add(searchFile.FullName.ToLower(), searchFile);
                    }
                }
            }

            // Return
            return resultFileDictionary.Values.ToList();
        }


        public static void DeleteReadOnly(string path)
        {
            #region Contracts

            if (string.IsNullOrEmpty(path) == true) throw new ArgumentException($"{nameof(path)}=null");

            #endregion

            // Require
            if (System.IO.File.Exists(path) == false) return;

            // Delete
            var rootFile = new FileInfo(path)
            {
                IsReadOnly = false
            };
            rootFile.Delete();
        }
    }
}