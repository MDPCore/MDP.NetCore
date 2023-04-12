using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace MDP.NetCore.Logging.Log4net.Lab
{
    public class Program
    {
        // Methods
        public static void Run(ILogger<Program> logger)
        {
            #region Contracts

            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");

            #endregion

            // Execute
            logger.LogError("Hello, World!");
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}