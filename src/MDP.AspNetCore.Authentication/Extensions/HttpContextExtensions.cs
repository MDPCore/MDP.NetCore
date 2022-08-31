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

namespace MDP.AspNetCore.Authentication
{
    public static partial class HttpContextExtensions
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

        public static Task ExternalSignInAsync(this HttpContext httpContext, ClaimsPrincipal principal)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException($"{nameof(httpContext)}=null");
            if (principal == null) throw new ArgumentException($"{nameof(principal)}=null");

            #endregion

            // SignInAsync
            return httpContext.SignInAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public static Task ExternalSignOutAsync(this HttpContext httpContext)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException($"{nameof(httpContext)}=null");

            #endregion

            // SignOutAsync
            return httpContext.SignOutAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    public static partial class HttpContextExtensions
    {
        // Methods
        internal static bool HasJwtBearer(this HttpContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // Require
            var authorization = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorization) == true) return false;
            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == false) return false;

            // Return
            return true;
        }

        internal static Task<string?> ExternalGetTokenAsync(this HttpContext httpContext, string tokenName)
        {
            #region Contracts

            if (httpContext == null) throw new ArgumentException(nameof(httpContext));
            if (string.IsNullOrEmpty(tokenName) == true) throw new ArgumentException($"{nameof(tokenName)}=null");

            #endregion

            // AuthenticateAsync
            return httpContext.GetTokenAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme, tokenName);
        }
    }
}
