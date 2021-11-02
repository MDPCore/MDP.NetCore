using Microsoft.Extensions.Hosting;
using System;

namespace MDP.NetCore.Lab
{
    public class Program
    {
        // Methods
        public static void Run(WorkService workService)
        {
            #region Contracts

            if (workService == null) throw new ArgumentException(nameof(workService));

            #endregion

            // Execute
            workService.Execute();
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.CreateHostBuilder<Program>(args).Build().Run();
        }
    }
}
