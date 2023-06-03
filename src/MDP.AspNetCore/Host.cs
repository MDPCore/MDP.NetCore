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
            var hostBuilder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args).ConfigureDefault();
            if (hostBuilder == null) throw new InvalidOperationException($"{nameof(hostBuilder)}=null");

            // Host
            var host = hostBuilder.Build().ConfigureDefault();
            if (host == null) throw new InvalidOperationException($"{nameof(host)}=null");

            // Run
            host.Run();
        }
    }
}
