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
        public static AuthenticationBuilder AddExternalCookie(this AuthenticationBuilder builder, Action<CookieAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // AddCookie
            builder.AddCookie(ExternalCookieAuthenticationDefaults.AuthenticationScheme, ExternalCookieAuthenticationDefaults.AuthenticationScheme, configureOptions);

            // Return
            return builder;
        }
    }
}
