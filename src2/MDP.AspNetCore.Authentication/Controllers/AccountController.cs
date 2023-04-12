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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        // Fields
        private readonly IdentityProvider _identityProvider;


        // Constructors
        public AccountController(IdentityProvider? identityProvider = null)
        {
            // Default
            _identityProvider = identityProvider ?? DefaultIdentityProvider.Current;
        }


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
            var identity = _identityProvider.SignIn(externalIdentity);
            if (identity == null)
            {
                if (string.IsNullOrEmpty(_identityProvider.RegisterPath) == false)
                {
                    // Register
                    return this.Redirect(_identityProvider.RegisterPath);                    
                }
                else
                {
                    // Forbid
                    return this.Forbid();
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