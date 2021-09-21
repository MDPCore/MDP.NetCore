using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.ExternalCookies
{
    public static class ControllerExtensions
    {
        // Methods
        public static Task<ActionResult> ExternalSignInAsync(this Controller controller)
        {
            #region Contracts

            if (controller == null) throw new ArgumentException(nameof(controller));

            #endregion

            // ExternalSignIn
            return controller.ExternalSignInAsync(ExternalCookieAuthenticationDefaults.AuthenticationScheme);
        }

        public static async Task<ActionResult> ExternalSignInAsync(this Controller controller, string authenticationScheme, string returnUrl = null)
        {
            #region Contracts

            if (controller == null) throw new ArgumentException(nameof(controller));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // Require
            returnUrl = returnUrl ?? controller.Url.Content("~/");
            if (controller.User.Identity.IsAuthenticated == true) return controller.Redirect(returnUrl);

            // OptionsMonitor
            var optionsMonitor = controller.HttpContext.RequestServices.GetService(typeof(IOptionsMonitor<ExternalCookieAuthenticationOptions>)) as IOptionsMonitor<ExternalCookieAuthenticationOptions>;
            if (optionsMonitor == null) throw new InvalidOperationException($"{nameof(optionsMonitor)}=null");

            // Options
            var options = optionsMonitor.Get(authenticationScheme);
            if (options == null) throw new InvalidOperationException($"{nameof(options)}=null");

            // AuthenticateResult
            var authenticateResult = await controller.HttpContext.AuthenticateAsync(authenticationScheme);
            if (authenticateResult.Succeeded == false) throw new InvalidOperationException($"{nameof(authenticateResult)}==null");
            if (authenticateResult.Principal == null) throw new InvalidOperationException($"{nameof(authenticateResult)}==null");

            // ClaimsIdentity
            var claimsIdentity = authenticateResult.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}==null");
            if (claimsIdentity.IsAuthenticated == false) throw new InvalidOperationException($"{nameof(claimsIdentity)}==null");

            // RegisterPath
            if (options.RegisterPath != null)
            {
                // RedirectUri
                var redirectUri = options.RegisterPath.Add(QueryString.Create(new Dictionary<string, string>()
                {
                    { "returnUrl", returnUrl }
                }));
                if (string.IsNullOrEmpty(redirectUri) == true) throw new InvalidOperationException($"{nameof(redirectUri)}=null");

                // Redirect
                return controller.Redirect(redirectUri);
            }
            else
            {
                // SignIn
                await controller.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                await controller.HttpContext.ExternalSignOutAsync();

                // Redirect
                return controller.Redirect(returnUrl);
            }
        }
    }
}
