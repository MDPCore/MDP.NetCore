using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.IO
{
    public static class File
    {
        // Methods
        public static FileInfo GetFile(string fileName, string searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException();

            #endregion

            // Return
            return GetAllFile(fileName, searchPath).FirstOrDefault();
        }

        public static List<FileInfo> GetAllFile(string fileName, string searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException();

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
                // SearchDirectory
                var searchDirectory = new DirectoryInfo(System.IO.Path.GetDirectoryName(Path.Combine(searchPath, searchPattern)));
                if (searchDirectory == null) throw new InvalidOperationException("searchDirectory=null");

                // SearchFileList
                var searchFileList = searchDirectory.GetFiles(System.IO.Path.GetFileName(Path.Combine(searchPath, searchPattern)), SearchOption.AllDirectories);
                if (searchFileList == null) throw new InvalidOperationException();

                // Add
                foreach (var searchFile in searchFileList)
                {
                    if (resultFileDictionary.ContainsKey(searchFile.Name.ToLower()) == false)
                    {
                        resultFileDictionary.Add(searchFile.Name.ToLower(), searchFile);
                    }
                }
            }

            // Return
            return resultFileDictionary.Values.ToList();
        }


        public static void DeleteReadOnly(string path)
        {
            #region Contracts

            if (string.IsNullOrEmpty(path) == true) throw new ArgumentException();

            #endregion

            // RootFile
            if (System.IO.File.Exists(path) == false) return;
            var rootFile = new FileInfo(path);

            // Delete
            rootFile.IsReadOnly = false;
            rootFile.Delete();
        }
    }
}