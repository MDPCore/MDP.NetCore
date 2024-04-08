using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MDP.IO
{
    public static partial class File
    {
        // Methods
        public static FileInfo GetFile(string fileName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion

            // Require
            if (Path.IsPathRooted(fileName)==true) return new FileInfo(fileName);

            // EntryDirectoryPath
            var entryDirectoryPath = Directory.GetEntryDirectoryPath();
            if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException("entryDirectoryPath=null");

            // FilePath
            var filePath = Path.Combine(entryDirectoryPath, fileName);
            if (string.IsNullOrEmpty(filePath) == true) throw new InvalidOperationException("filePath=null");

            // Return
            return new FileInfo(filePath);
        }

        public static List<FileInfo> GetAllFile(string searchPattern, string searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(searchPattern) == true) throw new ArgumentException($"{nameof(searchPattern)}=null");

            #endregion

            // SearchPath
            if (string.IsNullOrEmpty(searchPath) == true)
            {
                // EntryDirectoryPath
                var entryDirectoryPath = Directory.GetEntryDirectoryPath();
                if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException("entryDirectoryPath=null");

                // Setting
                searchPath = entryDirectoryPath;
            }
            if (System.IO.Directory.Exists(searchPath) == false) return new List<FileInfo>();

            // SearchPatternSectionList
            var searchPatternSectionList = searchPattern.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (searchPatternSectionList == null) throw new InvalidOperationException();

            // Search
            var resultFileDictionary = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (var searchPatternSection in searchPatternSectionList)
            {
                // SearchDirectoryPath
                var searchDirectoryPath = System.IO.Path.GetDirectoryName(Path.Combine(searchPath, searchPatternSection));
                if (string.IsNullOrEmpty(searchDirectoryPath) == true) throw new InvalidOperationException($"{nameof(searchDirectoryPath)}=null");

                // SearchDirectoryPattern
                var searchDirectoryPattern = System.IO.Path.GetFileName(Path.Combine(searchPath, searchPatternSection));
                if (string.IsNullOrEmpty(searchDirectoryPattern) == true) throw new InvalidOperationException($"{nameof(searchDirectoryPattern)}=null");

                // ResultFileList
                var resultFileList = (new DirectoryInfo(searchDirectoryPath)).GetFiles(searchDirectoryPattern, SearchOption.TopDirectoryOnly);
                if (resultFileList == null) throw new InvalidOperationException($"{nameof(resultFileList)}=null");

                // Add
                foreach (var resultFile in resultFileList)
                {
                    if (resultFileDictionary.ContainsKey(resultFile.FullName) == false)
                    {
                        resultFileDictionary.Add(resultFile.FullName, resultFile);
                    }
                }
            }

            // Return
            return resultFileDictionary.Values.ToList();
        }

        public static List<string> GetAllFilePath(string searchPattern, string searchPath = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(searchPattern) == true) throw new ArgumentException($"{nameof(searchPattern)}=null");

            #endregion

            // SearchPath
            if (string.IsNullOrEmpty(searchPath) == true)
            {
                // EntryDirectoryPath
                var entryDirectoryPath = Directory.GetEntryDirectoryPath();
                if (string.IsNullOrEmpty(entryDirectoryPath) == true) throw new InvalidOperationException("entryDirectoryPath=null");

                // Setting
                searchPath = entryDirectoryPath;
            }
            if (System.IO.Directory.Exists(searchPath) == false) return new List<string>();

            // SearchPatternSectionList
            var searchPatternSectionList = searchPattern.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (searchPatternSectionList == null) throw new InvalidOperationException();

            // Search
            var resultFilePathDictionary = new Dictionary<string,string>(StringComparer.OrdinalIgnoreCase);
            foreach (var searchPatternSection in searchPatternSectionList)
            {
                // SearchDirectoryPath
                var searchDirectoryPath = System.IO.Path.GetDirectoryName(Path.Combine(searchPath, searchPatternSection));
                if (string.IsNullOrEmpty(searchDirectoryPath) == true) throw new InvalidOperationException($"{nameof(searchDirectoryPath)}=null");

                // SearchDirectoryPattern
                var searchDirectoryPattern = System.IO.Path.GetFileName(Path.Combine(searchPath, searchPatternSection));
                if (string.IsNullOrEmpty(searchDirectoryPattern) == true) throw new InvalidOperationException($"{nameof(searchDirectoryPattern)}=null");

                // ResultFilePathList
                var resultFilePathList = System.IO.Directory.EnumerateFiles(searchDirectoryPath, searchDirectoryPattern, SearchOption.TopDirectoryOnly);
                if (resultFilePathList == null) throw new InvalidOperationException($"{nameof(resultFilePathList)}=null");

                // Add
                foreach (var resultFilePath in resultFilePathList)
                {
                    if (resultFilePathDictionary.ContainsKey(resultFilePath) == false)
                    {
                        resultFilePathDictionary.Add(resultFilePath, resultFilePath);
                    }
                }
            }

            // Return
            return resultFilePathDictionary.Values.ToList();
        }
    }

    public static partial class File
    {
        // Methods
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