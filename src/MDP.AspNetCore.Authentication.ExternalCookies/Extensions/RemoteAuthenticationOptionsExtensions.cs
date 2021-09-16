using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.ExternalCookies
{
    public static class RemoteAuthenticationOptionsExtensions
    {
        // Methods
        public static void ExternalSignIn(this RemoteAuthenticationOptions remoteAuthenticationOptions)
        {
            #region Contracts

            if (remoteAuthenticationOptions == null) throw new ArgumentException(nameof(remoteAuthenticationOptions));
         
            #endregion

            // ExternalSignIn
            remoteAuthenticationOptions.ExternalSignIn(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
        }

        public static void ExternalSignIn(this RemoteAuthenticationOptions remoteAuthenticationOptions, string authenticationScheme)
        {
            #region Contracts

            if (remoteAuthenticationOptions == null) throw new ArgumentException(nameof(remoteAuthenticationOptions));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));
         
            #endregion

            // SignInScheme
            remoteAuthenticationOptions.SignInScheme = authenticationScheme;

            // OnTicketReceived
            remoteAuthenticationOptions.Events.OnTicketReceived = context =>
            {
                // ExternalCookieAuthenticationOptionsMonitor
                var externalCookieAuthenticationOptionsMonitor = context.HttpContext.RequestServices.GetRequiredService<IOptionsMonitor<ExternalCookieAuthenticationOptions>>();
                if (externalCookieAuthenticationOptionsMonitor == null) throw new InvalidOperationException($"{nameof(externalCookieAuthenticationOptionsMonitor)}=null");

                // ExternalCookieAuthenticationOptions
                var externalCookieAuthenticationOptions = externalCookieAuthenticationOptionsMonitor.Get(authenticationScheme);
                if (externalCookieAuthenticationOptions == null) throw new InvalidOperationException($"{nameof(externalCookieAuthenticationOptions)}=null");
                
                // RedirectUri
                var redirectUri = externalCookieAuthenticationOptions.CallbackPath.Add(QueryString.Create(new Dictionary<string, string>()
                {
                    { "returnUrl", context.ReturnUri }
                }));
                if (string.IsNullOrEmpty(redirectUri) == true) throw new InvalidOperationException($"{nameof(redirectUri)}=null");

                // TicketReceivedContext
                context.ReturnUri = redirectUri;

                // Return
                return Task.CompletedTask;
            };
        }
    }
}
