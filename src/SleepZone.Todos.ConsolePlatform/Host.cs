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

namespace SleepZone.Todos.ConsolePlatform
{
    public static class Host
    {
        // Methods
        public static void Run<TProgram>(string[] args) where TProgram : class
        {
            #region Contracts

            if (args == null) throw new ArgumentException(nameof(args));

            #endregion

            // Run
            MDP.NetCore.Host.CreateHostBuilder<TProgram>(args).ConfigureServices((context, services) =>
            {
                // Log4net
                services.AddLog4netLogger(options =>
                {
                    options.Properties["ApplicationName"] = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                });

                // NLog
                services.AddNLogLogger(options =>
                {
                    options.Properties["ApplicationName"] = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                });
            }).Build().Run();
        }
    }
}
