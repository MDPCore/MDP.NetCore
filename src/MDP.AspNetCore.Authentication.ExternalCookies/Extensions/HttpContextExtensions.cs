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
    public static class HttpContextExtensions
    {
        // Methods
        public static Task<ActionResult> ExternalChallengeAsync(this HttpContext httpContext, string externalScheme, string returnUrl = @"/")
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));
            if (string.IsNullOrEmpty(externalScheme) == true) throw new ArgumentException(nameof(externalScheme));

            #endregion

            // ExternalChallengeAsync
            return httpContext.ExternalChallengeAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme, externalScheme, returnUrl);
        }

        public static async Task<ActionResult> ExternalChallengeAsync(this HttpContext httpContext, string authenticationScheme, string externalScheme, string returnUrl = @"/")
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));
            if (string.IsNullOrEmpty(externalScheme) == true) throw new ArgumentException(nameof(externalScheme));

            #endregion

            // AuthenticationOptionsMonitor
            var authenticationOptionsMonitor = httpContext.RequestServices.GetRequiredService<IOptionsMonitor<ExternalCookieAuthenticationOptions>>();
            if (authenticationOptionsMonitor == null) throw new InvalidOperationException($"{nameof(authenticationOptionsMonitor)}=null");

            // AuthenticationOptions
            var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
            if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

            // Require
            if (externalScheme == authenticationOptions.DefaultScheme) throw new InvalidOperationException($"{nameof(externalScheme)}={authenticationOptions.DefaultScheme}");
            if (externalScheme == authenticationScheme) throw new InvalidOperationException($"{nameof(externalScheme)}={authenticationScheme}");

            // SignOut
            await httpContext.SignOutAsync(authenticationScheme);

            // RedirectUri
            var redirectUri = authenticationOptions.CallbackPath.Add(QueryString.Create(new Dictionary<string, string>()
            {
                { "returnUrl", returnUrl }
            }));
            if (string.IsNullOrEmpty(redirectUri) == true) throw new InvalidOperationException($"{nameof(redirectUri)}=null");

            // Challenge
            return new ChallengeResult(externalScheme, new AuthenticationProperties() { RedirectUri = redirectUri });
        }


        public static Task<ClaimsIdentity> ExternalAuthenticateAsync(this HttpContext httpContext)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));

            #endregion

            // ExternalChallenge
            return httpContext.ExternalAuthenticateAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
        }

        public static async Task<ClaimsIdentity> ExternalAuthenticateAsync(this HttpContext httpContext, string authenticationScheme)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticateResult
            var authenticateResult = await httpContext.AuthenticateAsync(authenticationScheme);
            if (authenticateResult.Succeeded == false) return null;
            if (authenticateResult.Principal == null) return null;

            // AuthenticateAsync
            return authenticateResult.Principal.Identity as ClaimsIdentity;
        }


        public static Task ExternalSignOutAsync(this HttpContext httpContext)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));

            #endregion

            // ExternalChallenge
            return httpContext.ExternalSignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
        }

        public static async Task ExternalSignOutAsync(this HttpContext httpContext, string authenticationScheme)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationOptionsMonitor
            var authenticationOptionsMonitor = httpContext.RequestServices.GetRequiredService<IOptionsMonitor<ExternalCookieAuthenticationOptions>>();
            if (authenticationOptionsMonitor == null) throw new InvalidOperationException($"{nameof(authenticationOptionsMonitor)}=null");

            // AuthenticationOptions
            var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
            if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

            // SignOut
            await httpContext.SignOutAsync(authenticationScheme);
        }
    }
}
