using System;
using System.Diagnostics;

namespace MDP.IO.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // FileList
            var fileList = MDP.IO.File.GetAllFile("*.dll");
            if (fileList == null) throw new InvalidOperationException($"{nameof(fileList)}=null");
            foreach (var file in fileList)
            {
                Console.WriteLine(file.FullName);
            }
            Console.WriteLine("\n\n");

            // EntryDirectoryPath
            var entryDirectoryPath = MDP.IO.Directory.GetEntryDirectoryPath();
            if (string.IsNullOrEmpty(entryDirectoryPath) == true ) throw new InvalidOperationException($"{nameof(entryDirectoryPath)}=null");
            Console.WriteLine(entryDirectoryPath);
            Console.WriteLine("\n\n");
        }
    }
}
