using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.ExternalCookies.Services
{
    public class AccountController : Controller
    {
        // Methods
        [ExternalAuthorize]
        [Route("signin-external")]
        public async Task<ActionResult> ExternalSignIn(string returnUrl = @"/")
        {
            // ClaimsIdentity
            var claimsIdentity = this.User.Identity;
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}==null");

            // SignIn
            await this.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            await this.HttpContext.ExternalSignOutAsync();

            // Redirect
            return this.Redirect(returnUrl);
        }
    }
}
