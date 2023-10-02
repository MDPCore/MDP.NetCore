using Microsoft.Extensions.Hosting;
using System;

namespace ConsoleApp1
{
    public class Program
    {
        // Methods
        public static void Run(DemoService demoService)
        {
            // Message
            var message = demoService.GetMessage();

            // Display
            Console.WriteLine(message);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}