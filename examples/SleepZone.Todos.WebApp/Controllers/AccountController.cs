using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using MDP.AspNetCore.Authentication;

namespace SleepZone.Todos.WebApp
{
    public partial class AccountController : Controller
    {
        // Methods
        [AllowAnonymous]
        public async Task<ActionResult> LoginByName(string username, string? returnUrl = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(username) == true) throw new ArgumentException(nameof(username));

            #endregion

            // Require
            returnUrl = returnUrl ?? this.Url.Content("~/");

            // ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, username + "@hotmail.com"),
            }, "NameAuth");

            // SignIn
            await this.HttpContext.ExternalSignInAsync(new ClaimsPrincipal(claimsIdentity));

            // Return
            return this.RedirectToRoute("SignIn", new { returnUrl = returnUrl });
        }
    }
}
