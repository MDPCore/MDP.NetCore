using MDP.AspNetCore;
using MDP.NetCore;
using MDP.NetCore.Logging.Log4net;
using MDP.NetCore.Logging.NLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.WebPlatform
{
    public static class Host
    {
        // Methods
        public static void Run(string[] args)
        {
            #region Contracts

            if (args == null) throw new ArgumentException(nameof(args));

            #endregion

            // Run
            MDP.AspNetCore.Host.CreateHostBuilder<Startup>(args).Build().Run();
        }
    }
}
