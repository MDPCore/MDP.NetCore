using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.WebApp.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            MDP.WebApp.Host.CreateHostBuilder(args).Build().Run();
        }
    }
}
