using System;
using System.Diagnostics;
using System.Text;

namespace MDP.IO.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Execute
            var result = MDP.Diagnostics.Process.Execute("ipconfig");

            // Display
            Console.WriteLine(result);
        }
    }
}
