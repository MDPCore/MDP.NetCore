using MDP.AspNetCore.Authentication.JwtBearer;
using MDP.AspNetCore.Authentication.External;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MDP.WebApp
{
    public partial class AccountController : Controller
    {
        // Fields
        private readonly SecurityTokenFactory _tokenFactory;


        // Constructors
        public AccountController(SecurityTokenFactory tokenFactory)
        {
            #region Contracts

            if (tokenFactory == null) throw new ArgumentException(nameof(tokenFactory));

            #endregion

            // Default
            _tokenFactory = tokenFactory;
        }


        // Methods
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = @"/")
        {
            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // Return
            return View(@"Index");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            // Require
            if (this.User.Identity.IsAuthenticated == false) return this.Redirect(@"/");

            // SignIn
            await this.HttpContext.SignOutAsync();

            // Redirect
            return this.Redirect(@"/");
        }
    }

    public partial class AccountController : Controller
    {
        // Methods
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLogin(string externalScheme, string returnUrl = @"/")
        {
            #region Contracts

            if (string.IsNullOrEmpty(externalScheme) == true) throw new ArgumentException(nameof(externalScheme));

            #endregion

            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // ExternalChallenge
            return await this.HttpContext.ExternalChallengeAsync(externalScheme, returnUrl);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalSignIn(string returnUrl = @"/")
        {
            // Require
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // ClaimsIdentity
            var claimsIdentity = await this.HttpContext.ExternalAuthenticateAsync();
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}==null");

            // SignIn
            await this.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
            await this.HttpContext.ExternalSignOutAsync();

            // Redirect
            return this.Redirect(returnUrl);
        }
    }

    // GetUser
    public partial class AccountController : Controller
    {
        // Methods
        [Authorize]
        public ActionResult<GetUserResultModel> GetUser([FromBody] GetUserActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // ClaimIdentity
            var claimIdentity = this.User.Identity as ClaimsIdentity;
            if (claimIdentity == null) throw new InvalidOperationException($"{nameof(claimIdentity)}=null");

            // UserModel
            var user = new UserModel();
            user.AuthenticationType = claimIdentity.AuthenticationType;
            user.UserId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            user.UserName = claimIdentity.FindFirst(ClaimTypes.Name)?.Value;

            // Return
            return (new GetUserResultModel()
            {
                User = user
            });
        }


        // Class
        public class GetUserActionModel
        {
            // Properties

        }

        public class GetUserResultModel
        {
            // Properties
            public UserModel User { get; set; }
        }

        public class UserModel
        {
            // Properties
            public string AuthenticationType { get; set; }

            public string UserId { get; set; }

            public string UserName { get; set; }
        }
    }

    // GetToken
    public partial class AccountController : Controller
    {
        // Methods
        [Authorize]
        public ActionResult<GetTokenResultModel> GetToken([FromBody] GetTokenActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // ClaimIdentity
            var claimIdentity = this.User.Identity as ClaimsIdentity;
            if (claimIdentity == null) throw new InvalidOperationException($"{nameof(claimIdentity)}=null");
            
            // TokenString
            var tokenString = _tokenFactory.CreateEncodedJwt(claimIdentity);
            if (string.IsNullOrEmpty(tokenString) == true) throw new InvalidOperationException($"{nameof(tokenString)}=null");

            // Return
            return (new GetTokenResultModel()
            {
                Token = tokenString
            });
        }


        // Class
        public class GetTokenActionModel
        {
            // Properties
           
        }

        public class GetTokenResultModel
        {
            // Properties
            public string Token { get; set; }
        }
    }
}
