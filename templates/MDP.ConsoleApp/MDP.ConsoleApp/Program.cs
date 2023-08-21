using System;

namespace MDP.ConsoleApp
{
    public class Program
    {
        // Methods
        public static void Run(WorkContext workContext)
        {
            // Display
            Console.WriteLine(workContext.GetValue());
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
