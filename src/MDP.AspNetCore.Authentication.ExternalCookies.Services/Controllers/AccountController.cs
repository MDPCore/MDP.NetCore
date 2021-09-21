using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        [AllowAnonymous]
        [Route("signin-external")]
        public Task<ActionResult> SignIn(string authenticationScheme, string returnUrl = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // Return
            return this.ExternalSignInAsync(authenticationScheme, returnUrl);
        }
    }
}
