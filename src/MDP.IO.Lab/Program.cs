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

            // EntryDirectory
            var entryDirectory = MDP.IO.Directory.GetEntryDirectory();
            if (string.IsNullOrEmpty(entryDirectory) == true ) throw new InvalidOperationException($"{nameof(entryDirectory)}=null");
            Console.WriteLine(entryDirectory);
            Console.WriteLine("\n\n");
        }
    }
}
