using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.External
{
    public static partial class ExternalCookieAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddExternalCookie(this AuthenticationBuilder builder, Action<ExternalCookieAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // Return
            return builder.AddExternalCookie(ExternalCookieAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddExternalCookie(this AuthenticationBuilder builder, string authenticationScheme, Action<ExternalCookieAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationOptions
            if (configureOptions != null) builder.Services.Configure(authenticationScheme, configureOptions);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ExternalCookieAuthenticationOptions>, ExternalCookieAuthenticationPostConfigureOptions>());

            // CookieScheme
            builder.Services.AddOptions<CookieAuthenticationOptions>(authenticationScheme).Configure<IOptionsMonitor<ExternalCookieAuthenticationOptions>>((cookieOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // CookieOptions
                cookieOptions.ForwardChallenge = authenticationOptions.DefaultScheme;
                cookieOptions.ForwardForbid = authenticationOptions.DefaultScheme;
            });
            builder.AddCookie(authenticationScheme, null, null);

            // Return
            return builder;
        }
    }

    public static partial class ExternalCookieAuthenticationExtensions
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
