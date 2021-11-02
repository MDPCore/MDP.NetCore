using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    public static class Host
    {
        // Methods
        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args)
            where TStartup : class
        {
            #region Contracts

            if (args == null) throw new ArgumentException(nameof(args));

            #endregion

            // HostBuilder
            return MDP.NetCore.Host.CreateHostBuilder(args)
                   .ConfigureAspNetCore<TStartup>(hostBuilder =>
                   {
                       // Nothing

                   });
        }
    }
}
