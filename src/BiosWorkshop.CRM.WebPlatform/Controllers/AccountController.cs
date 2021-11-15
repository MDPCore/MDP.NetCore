using BiosWorkshop.CRM.Members;
using MDP.AspNetCore.Authentication.ExternalCookies;
using MDP.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BiosWorkshop.CRM.WebPlatform
{
    // Class
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
    }

    // Login
    public partial class AccountController : Controller
    {
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
            return View();
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

    // Register
    public partial class AccountController : Controller
    {
        // Methods
        [ExternalAuthorize]
        [HttpGet]
        public ActionResult<RegisterResultModel> Register(string returnUrl = null)
        {
            // Require
            returnUrl = returnUrl ?? this.Url.Content("~/");

            // ClaimsIdentity
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}=null");

            // ResultModel
            var resultModel = new RegisterResultModel()
            {
                User= new MemberUser()
                {
                    Name = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value,
                    Mail = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value,
                },
            };

            // View
            return View(resultModel);
        }

        [ExternalAuthorize]
        [HttpPost]
        public ActionResult<RegisterResultModel> Register(RegisterActionModel actionModel, string returnUrl = null)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // Require
            returnUrl = returnUrl ?? this.Url.Content("~/");

            // ResultModel
            var resultModel = new RegisterResultModel()
            {
                User = actionModel.User
            };

            // View
            return View(resultModel);
        }


        // Class
        public class RegisterActionModel
        {
            // Properties
            public MemberUser User { get; set; }
        }

        public class RegisterResultModel
        {
            // Properties
            public MemberUser User { get; set; }
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
