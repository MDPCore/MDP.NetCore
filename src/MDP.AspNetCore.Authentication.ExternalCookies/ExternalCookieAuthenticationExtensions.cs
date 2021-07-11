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

namespace MDP.AspNetCore.Authentication.ExternalCookies
{
    public static class ExternalCookieAuthenticationExtensions
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
}
