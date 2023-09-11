using System;

namespace MDP.Logging.Serilog.Lab
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
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}