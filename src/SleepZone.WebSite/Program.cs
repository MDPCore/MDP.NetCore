using MDP.NetCore;
using MDP.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MDP.WebApp;

namespace SleepZone.WebSite
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            MDP.WebApp.Host.CreateHostBuilder<Startup>(args).Build().Run();
        }
    }
}
