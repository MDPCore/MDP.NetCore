using MDP.AspNetCore.Authentication.ExternalCookies;
using MDP.AspNetCore.Authentication.JwtBearer;
using MDP.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepZone.Todos.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SleepZone.Todos.WebPlatform
{
    public partial class AccountController : Controller
    {
        // Fields
        private readonly MemberContext _memberContext = null;

        private readonly IdentityContext _identityContext = null;

        private readonly SecurityTokenFactory _tokenFactory = null;


        // Constructors
        public AccountController(MemberContext memberContext, IdentityContext identityContext, SecurityTokenFactory tokenFactory)
        {
            #region Contracts

            if (memberContext == null) throw new ArgumentException(nameof(memberContext));
            if (identityContext == null) throw new ArgumentException(nameof(identityContext));
            if (tokenFactory == null) throw new ArgumentException(nameof(tokenFactory));

            #endregion

            // Default
            _memberContext = memberContext;
            _identityContext = identityContext;
            _tokenFactory = tokenFactory;
        }


        // Methods
        [AllowAnonymous]
        public ActionResult Login(string externalScheme = null, string returnUrl = null)
        {
            // Require
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (this.User.Identity.IsAuthenticated == true) return this.Redirect(returnUrl);

            // Challenge
            if (string.IsNullOrEmpty(externalScheme) == false) return this.Challenge(new AuthenticationProperties() { RedirectUri = returnUrl }, externalScheme);

            // View
            return View(@"Index");
        }

        [AllowAnonymous]
        public ActionResult Logout(string returnUrl = null)
        {
            // Require
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (this.User.Identity.IsAuthenticated == false) return this.Redirect(returnUrl);

            // SignOut
            return this.SignOut(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
    }

    public partial class AccountController : Controller
    {
        // Methods
        [ExternalAuthorize]
        public async Task<ActionResult> ExternalLogin(string returnUrl = null)
        {
            // ReturnUrl
            returnUrl = returnUrl ?? this.Url.Content("~/");
            if (string.IsNullOrEmpty(returnUrl) == true) throw new InvalidOperationException($"{nameof(returnUrl)}=null");

            // AuthenticateResult
            var authenticateResult = await this.HttpContext.ExternalAuthenticateAsync();
            if (authenticateResult.Succeeded == false) throw new InvalidOperationException($"{nameof(authenticateResult)}==null");

            // ExternalIdentity
            var externalIdentity = this.User.Identity as ClaimsIdentity;
            if (externalIdentity == null) throw new InvalidOperationException($"{nameof(externalIdentity)}==null");
            if (externalIdentity.IsAuthenticated == false) throw new InvalidOperationException($"{nameof(externalIdentity)}==null");

            // User
            var authenticationType = externalIdentity.AuthenticationType;
            var externalId = externalIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var externalName = externalIdentity.Name;
            var externalMail = externalIdentity.FindFirst(ClaimTypes.Email)?.Value;
            var accessToken = await this.HttpContext.ExternalGetTokenAsync("access_token");
            var refreshToken = await this.HttpContext.ExternalGetTokenAsync("refresh_token");

            // AuthenticationType
            //if (authenticationType == "Google")
            //{
            //    // Challenge
            //    var properties = new AuthenticationProperties();
            //    {
            //        properties.RedirectUri = returnUrl;
            //        properties.SetParameter(GoogleChallengeProperties.ScopeKey, new List<string>
            //        {
            //            "openid",
            //            "profile",
            //            "email",
            //            "https://www.googleapis.com/auth/calendar"
            //        });
            //    }
            //    return this.Challenge(properties, authType);
            //}

            // SignIn
            await this.HttpContext.SignInAsync(new ClaimsPrincipal(externalIdentity));
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

            // ClaimsIdentity
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}=null");

            // UserModel
            var user = new UserModel();
            user.AuthenticationType = claimsIdentity.AuthenticationType;
            user.UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            user.UserName = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;

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

            // ClaimsIdentity
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}=null");
            
            // TokenString
            var tokenString = _tokenFactory.CreateEncodedJwt(claimsIdentity);
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
