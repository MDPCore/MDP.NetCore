using System;

namespace MDP.AspNetCore
{
    public static class Host
    {
        // Methods
        public static void Run(string[] args)
        {
            #region Contracts

            if (args == null) throw new ArgumentException($"{nameof(args)}=null");

            #endregion

            // HostBuilder
            var hostBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args).ConfigureMDP();
            if (hostBuilder == null) throw new InvalidOperationException($"{nameof(hostBuilder)}=null");

            // Host
            var host = hostBuilder.Build().ConfigureMDP();
            if (host == null) throw new InvalidOperationException($"{nameof(host)}=null");

            // Run
            host.Run();
        }
    }
}
