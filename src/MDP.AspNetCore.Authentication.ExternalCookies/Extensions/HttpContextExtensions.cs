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

            // SignOut
            await httpContext.SignOutAsync(authenticationScheme);
        }
    }
}
