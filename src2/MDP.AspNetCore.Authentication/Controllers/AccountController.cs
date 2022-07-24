using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication
{
    public partial class AccountController : Controller
    {
        // Fields
        private readonly IdentityProvider _identityProvider;


        // Constructors
        public AccountController(IdentityProvider? identityProvider = null)
        {
            // Default
            _identityProvider = identityProvider ?? DefaultIdentityProvider.Current;
        }


        // Properties
        private string? RegisterPath { get { return _identityProvider.RegisterPath; } }


        // Methods
        [NonAction]
        private ClaimsIdentity SignIn(ClaimsIdentity externalIdentity)
        {
            #region Contracts

            if (externalIdentity == null) throw new ArgumentException($"{nameof(externalIdentity)}=null");

            #endregion

            // Return
            return _identityProvider.SignIn(externalIdentity);
        }
    }

    public partial class AccountController : Controller
    {
        // Methods
        [AllowAnonymous]
        [Route("/Login", Name = "Login")]
        public ActionResult Login(string? scheme = null, string? returnUrl = null)
        {           
            // Require
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (this.User?.Identity?.IsAuthenticated == true) return this.Redirect(returnUrl);

            // Challenge
            if (string.IsNullOrEmpty(scheme) == false)
            {
                // Ignore
                if (scheme.Equals(PolicyAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase) == true) throw new InvalidOperationException($"{nameof(scheme)}={scheme}");
                if (scheme.Equals(CookieAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase) == true) throw new InvalidOperationException($"{nameof(scheme)}={scheme}");
                if (scheme.Equals(ExternalCookieAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase) == true) throw new InvalidOperationException($"{nameof(scheme)}={scheme}");
                if (scheme.Equals(SecurityTokenAuthenticationDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase) == true) throw new InvalidOperationException($"{nameof(scheme)}={scheme}");

                // Redirect
                return this.Challenge(new AuthenticationProperties() { RedirectUri = returnUrl }, scheme);
            }

            // View
            return View("Login");
        }

        [AllowAnonymous]
        [Route("/Logout", Name = "Logout")]
        public ActionResult Logout(string? returnUrl = null)
        {
            // Require
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (this.User?.Identity?.IsAuthenticated == false) return this.Redirect(returnUrl);

            // SignOut
            return this.SignOut(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [ExternalAuthorize]
        [Route("/SignIn", Name = "SignIn")]
        public async Task<ActionResult> SignIn(string? returnUrl = null)
        {
            // ReturnUrl
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (string.IsNullOrEmpty(returnUrl) == true) throw new InvalidOperationException($"{nameof(returnUrl)}=null");

            // ExternalAuthenticateResult
            var externalAuthenticateResult = await this.HttpContext.ExternalAuthenticateAsync();
            if (externalAuthenticateResult.Succeeded == false) throw new InvalidOperationException($"{nameof(externalAuthenticateResult.Succeeded)}=false");

            // ExternalIdentity
            var externalIdentity = this.User.Identity as ClaimsIdentity;
            if (externalIdentity == null) throw new InvalidOperationException($"{nameof(externalIdentity)}=null");
            if (externalIdentity.IsAuthenticated == false) throw new InvalidOperationException($"{nameof(externalIdentity)}=null");

            // Identity
            var identity = this.SignIn(externalIdentity);
            if (identity == null)
            {
                if (string.IsNullOrEmpty(this.RegisterPath) == true)
                {
                    // Forbid
                    return this.Forbid();
                }
                else
                {
                    // Redirect
                    return this.Redirect(this.RegisterPath);
                }
            }

            // SignIn
            await this.HttpContext.ExternalSignOutAsync();
            await this.HttpContext.SignInAsync(new ClaimsPrincipal(identity));            

            // Redirect
            return this.Redirect(returnUrl);
        }
    }
}

// ExternalParameters
//var authenticationType = externalIdentity.AuthenticationType;
//var externalId = externalIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//var externalName = externalIdentity.Name;
//var externalMail = externalIdentity.FindFirst(ClaimTypes.Email)?.Value;
//var accessToken = await this.HttpContext.ExternalGetTokenAsync("access_token");
//var refreshToken = await this.HttpContext.ExternalGetTokenAsync("refresh_token");
