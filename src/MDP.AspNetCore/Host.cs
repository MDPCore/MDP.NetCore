using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace MDP.AspNetCore
{
    public static class Host
    {
        // Methods
        public static IHost Create(string[] args)
        {
            #region Contracts

            if (args == null) throw new ArgumentException($"{nameof(args)}=null");

            #endregion

            // HostBuilder
            var hostBuilder = WebApplication.CreateBuilder(args).ConfigureDefault(hostBuilder =>
            {
                // Program

            });
            if (hostBuilder == null) throw new InvalidOperationException($"{nameof(hostBuilder)}=null");

            // Host
            var host = hostBuilder.Build().ConfigureDefault();
            if (host == null) throw new InvalidOperationException($"{nameof(host)}=null");

            // Return
            return host;
        }

        public static IHost Create<TProgram>(string[] args) where TProgram : class
        {
            #region Contracts

            if (args == null) throw new ArgumentException($"{nameof(args)}=null");

            #endregion

            // HostBuilder
            var hostBuilder = WebApplication.CreateBuilder(args).ConfigureDefault(hostBuilder =>
            {
                // Program
                hostBuilder.AddProgramService<TProgram>();
            });
            if (hostBuilder == null) throw new InvalidOperationException($"{nameof(hostBuilder)}=null");

            // Host
            var host = hostBuilder.Build().ConfigureDefault();
            if (host == null) throw new InvalidOperationException($"{nameof(host)}=null");

            // Return
            return host;
        }
    }
}
