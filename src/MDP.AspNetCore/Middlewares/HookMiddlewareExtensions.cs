using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MDP.AspNetCore
{
    public static partial class HookMiddlewareExtensions
    {
        // Methods        
        private static WebApplicationBuilder AddHookMiddleware(this WebApplicationBuilder hostBuilder, string hookName, Action<WebApplication> configureMiddleware)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (string.IsNullOrEmpty(hookName) == true) throw new ArgumentException($"{nameof(hookName)}=null");
            if (configureMiddleware == null) throw new ArgumentException($"{nameof(configureMiddleware)}=null");

            #endregion

            // HookMiddleware
            hostBuilder.Services.Add(ServiceDescriptor.Transient<HookMiddleware>(serviceProvider => new HookMiddleware(hookName, configureMiddleware)));

            // Return
            return hostBuilder;
        }

        public static WebApplicationBuilder AddEnteringMiddleware(this WebApplicationBuilder hostBuilder, Action<WebApplication> configureMiddleware)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (configureMiddleware == null) throw new ArgumentException($"{nameof(configureMiddleware)}=null");

            #endregion

            // HookMiddleware
            return hostBuilder.AddHookMiddleware(HookMiddlewareDefaults.EnteringHook, configureMiddleware);
        }

        public static WebApplicationBuilder AddRoutingMiddleware(this WebApplicationBuilder hostBuilder, Action<WebApplication> configureMiddleware)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (configureMiddleware == null) throw new ArgumentException($"{nameof(configureMiddleware)}=null");

            #endregion

            // HookMiddleware
            return hostBuilder.AddHookMiddleware(HookMiddlewareDefaults.RoutingHook, configureMiddleware);
        }

        public static WebApplicationBuilder AddRoutedMiddleware(this WebApplicationBuilder hostBuilder, Action<WebApplication> configureMiddleware)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (configureMiddleware == null) throw new ArgumentException($"{nameof(configureMiddleware)}=null");

            #endregion

            // AddHookMiddleware
            return hostBuilder.AddHookMiddleware(HookMiddlewareDefaults.RoutedHook, configureMiddleware);
        }
    }

    public static partial class HookMiddlewareExtensions
    {
        // Methods 
        internal static IApplicationBuilder UseHook(this IApplicationBuilder app, string hookName)
        {
            #region Contracts

            if (app == null) throw new ArgumentException($"{nameof(app)}=null");
            if (string.IsNullOrEmpty(hookName) == true) throw new ArgumentException($"{nameof(hookName)}=null");

            #endregion

            // Host
            var host = app as WebApplication;
            if (host == null) throw new InvalidOperationException($"{nameof(host)}=null");

            // UseHook
            return host.UseHook(hookName);
        }

        internal static WebApplication UseHook(this WebApplication host, string hookName)
        {
            #region Contracts

            if (host == null) throw new ArgumentException($"{nameof(host)}=null");
            if (string.IsNullOrEmpty(hookName) == true) throw new ArgumentException($"{nameof(hookName)}=null");

            #endregion

            // HookList
            var hookList = host.Services.GetRequiredService<IEnumerable<HookMiddleware>>()?.TakeWhile(o => o.HookName == hookName);
            if (hookList == null) throw new InvalidOperationException($"{nameof(hookList)}=null");
            if (hookList.Any() == false) return host;

            // Configure
            foreach (var hook in hookList)
            {
                hook.ConfigureMiddleware(host);
            }

            // Return
            return host;
        }
    }
}
