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

namespace MDP.AspNetCore.Authentication.Liff
{
    public class AccountController : Controller
    {
        // Fields
        private readonly IOptionsMonitor<LiffOptions> _optionsMonitor;


        // Constructors
        public AccountController(IOptionsMonitor<LiffOptions> optionsMonitor)
        {
            #region Contracts

            if (optionsMonitor == null) throw new ArgumentException(nameof(optionsMonitor));

            #endregion

            // Default
            _optionsMonitor = optionsMonitor;
        }


        // Methods
        [AllowAnonymous]
        [Route("/Login-Liff", Name = "Login-Liff")]
        public ActionResult LoginLiff(string authenticationScheme = null, string returnUrl = null)
        {
            // AuthenticationScheme
            authenticationScheme = authenticationScheme ?? LiffDefaults.AuthenticationScheme;
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new InvalidOperationException($"{nameof(authenticationScheme)}=null");
                        
            // AuthenticationOptions
            var authenticationOptions = _optionsMonitor.Get(authenticationScheme);
            if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

            // ReturnUrl
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (string.IsNullOrEmpty(returnUrl) == true) throw new InvalidOperationException($"{nameof(returnUrl)}=null");

            // ViewBag
            this.ViewBag.LiffId = authenticationOptions.LiffId;
            this.ViewBag.LiffName = authenticationOptions.LiffName;
            this.ViewBag.LiffColor = authenticationOptions.LiffColor;
            this.ViewBag.ReturnUrl = returnUrl;

            // Return
            return View();
        }
    }
}