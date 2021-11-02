using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace MDP.AspNetCore.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            MDP.AspNetCore.Host.CreateHostBuilder<Startup>(args).Build().Run();
        }
    }
}
