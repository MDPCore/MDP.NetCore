using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CLK.IO
{
    public static class File
    {
        // Methods
        public static FileInfo GetFile(string fileName, string searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion

            // Return
            return GetAllFile(fileName, searchPath).FirstOrDefault();
        }

        public static List<FileInfo> GetAllFile(string fileName, string searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion
                  
            // SearchPath
            if (string.IsNullOrEmpty(searchPath) == true)
            {
                // EntryDirectoryPath
                var entryDirectoryPath = Directory.GetEntryDirectory();
                if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException("entryDirectoryPath=null");

                // Setting
                searchPath = entryDirectoryPath;
            }
            if (System.IO.Directory.Exists(searchPath) == false) return new List<FileInfo>();

            // SearchPatternList
            var searchPatternList = fileName.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (searchPatternList == null) throw new InvalidOperationException();

            // Search
            var resultFileDictionary = new Dictionary<string, FileInfo>();
            foreach (var searchPattern in searchPatternList)
            {
                // SearchDirectoryPath
                var searchDirectoryPath = System.IO.Path.GetDirectoryName(Path.Combine(searchPath, searchPattern));
                if (string.IsNullOrEmpty(searchDirectoryPath) == true) throw new InvalidOperationException($"{nameof(searchDirectoryPath)}=null");

                // SearchDirectory
                var searchDirectory = new DirectoryInfo(searchDirectoryPath);
                if (searchDirectory == null) throw new InvalidOperationException($"{nameof(searchDirectory)}=null");
                if (searchDirectory.Exists == false) continue;

                // ResultFileList
                var resultFileList = searchDirectory.GetFiles(System.IO.Path.GetFileName(Path.Combine(searchPath, searchPattern)), SearchOption.TopDirectoryOnly);
                if (resultFileList == null) throw new InvalidOperationException($"{nameof(resultFileList)}=null");

                // Add
                foreach (var resultFile in resultFileList)
                {
                    if (resultFileDictionary.ContainsKey(resultFile.FullName.ToLower()) == false)
                    {
                        resultFileDictionary.Add(resultFile.FullName.ToLower(), resultFile);
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