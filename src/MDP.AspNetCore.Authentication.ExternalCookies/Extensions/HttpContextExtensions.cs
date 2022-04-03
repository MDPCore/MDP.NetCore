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
        public static Task<AuthenticateResult> ExternalAuthenticateAsync(this HttpContext httpContext)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));

            #endregion

            // AuthenticateAsync
            return httpContext.AuthenticateAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
        }

        public static Task<string> ExternalGetTokenAsync(this HttpContext httpContext, string tokenName)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));
            if (string.IsNullOrEmpty(tokenName) == true) throw new ArgumentException($"{nameof(httpContext)}=null");

            #endregion

            // AuthenticateAsync
            return httpContext.GetTokenAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme, tokenName);
        }

        public static Task ExternalSignOutAsync(this HttpContext httpContext)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));

            #endregion

            // SignOutAsync
            return httpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
