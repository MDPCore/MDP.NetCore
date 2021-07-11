using MDP.NetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.ConsoleApp
{
    public static class Host
    {
        // Methods
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            #region Contracts

            if (args == null) throw new ArgumentException(nameof(args));

            #endregion

            // HostBuilder
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                   .ConfigureNetCore(hostBuilder =>
                   {
                       // Nothing
                   
                   });
        }

        public static IHostBuilder CreateHostBuilder<TProgram>(string[] args) where TProgram : class
        {
            #region Contracts

            if (args == null) throw new ArgumentException(nameof(args));

            #endregion

            // HostBuilder
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                   .ConfigureNetCore(hostBuilder =>
                   {
                       // Program
                       hostBuilder.AddProgramService<TProgram>();
                   });
        }
    }
}
