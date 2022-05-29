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

namespace MDP.AspNetCore.Authentication
{
    public static class ExternalCookieAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddExternalCookie(this AuthenticationBuilder builder, Action<CookieAuthenticationOptions>? configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException($"{nameof(builder)}=null");

            #endregion

            // AddExternalCookie
            return builder.AddExternalCookie(ExternalCookieAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddExternalCookie(this AuthenticationBuilder builder, string authenticationScheme, Action<CookieAuthenticationOptions>? configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException($"{nameof(builder)}=null");
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationOptions
            if (configureOptions == null)
            {
                builder.AddCookie(authenticationScheme);
            }
            else
            {
                builder.AddCookie(authenticationScheme, null, configureOptions);
            }

            // Return
            return builder;
        }
    }
}
