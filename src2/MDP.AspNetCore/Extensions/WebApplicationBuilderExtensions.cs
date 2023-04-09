using Microsoft.AspNetCore.Builder;

namespace MDP.AspNetCore
{
    internal static class WebApplicationBuilderExtensions
    {
        // Methods
        public static WebApplicationBuilder ConfigureDefault(this WebApplicationBuilder hostBuilder, Action<WebApplicationBuilder>? configureAction = null)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // Return
            return hostBuilder;
        }
    }
}