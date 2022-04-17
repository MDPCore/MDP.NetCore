using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Google
{
    public abstract class GoogleIdentityService: IdentityService
    {
        // Methods
        public abstract ClaimsIdentity Login(IIdentity identity);

        public abstract string GetTokne(string userId, List<string> scopeList = null);

        public abstract void RegisterLogin(string userId, string googleId);

        public abstract void RegisterTokne(string userId, string accessToken, string refreshToken = null);
    }

    public abstract class GoogleIdentityService<TUser> : GoogleIdentityService
        where TUser : class
    {
        // Methods
        public override ClaimsIdentity Login(IIdentity externalIdentity)
        {
            #region Contracts

            if (externalIdentity == null) throw new ArgumentException(nameof(externalIdentity));

            #endregion

            // Require
            if (externalIdentity is ClaimsIdentity == false) return null;

            // Login
            return this.Login(externalIdentity as ClaimsIdentity);
        }

        public ClaimsIdentity Login(ClaimsIdentity externalIdentity)
        {
            #region Contracts

            if (externalIdentity == null) throw new ArgumentException(nameof(externalIdentity));

            #endregion

            // Require
            if (externalIdentity.IsAuthenticated == false) return null;
            if (externalIdentity.AuthenticationType != GoogleDefaults.LoginType) return null;

            // User
            var authenticationType = externalIdentity.AuthenticationType;
            var externalId = externalIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var externalName = externalIdentity.Name;
            var externalMail = externalIdentity.FindFirst(ClaimTypes.Email)?.Value;
            //var accessToken = await this.HttpContext.ExternalGetTokenAsync("access_token");
            //var refreshToken = await this.HttpContext.ExternalGetTokenAsync("refresh_token");

            // UserRepository
            var userRepository = this.GetRepository<UserRepository<TUser>>();
            if (userRepository == null) throw new InvalidOperationException($"{nameof(userRepository)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginValue(GoogleDefaults.LoginType, externalId);
            if (userLogin == null) return null;

            // User
            var user = userRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // UserToken

            // Identity
            var identity = this.CreateIdentity(user);
            if (identity == null) throw new InvalidOperationException($"{nameof(identity)}=null");

            // Return
            return identity;
        }

        public ClaimsIdentity Login(string googleId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(googleId) == true) throw new ArgumentException(nameof(googleId));

            #endregion

            // UserRepository
            var userRepository = this.GetRepository<UserRepository<TUser>>();
            if (userRepository == null) throw new InvalidOperationException($"{nameof(userRepository)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginValue(GoogleDefaults.LoginType, googleId);
            if (userLogin == null) return null;
                        
            // User
            var user = userRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // Identity
            var identity = this.CreateIdentity(user);
            if (identity == null) throw new InvalidOperationException($"{nameof(identity)}=null");

            // Return
            return identity;
        }

        public override string GetTokne(string userId, List<string> scopeList = null)
        {
            throw new NotImplementedException();
        }        

        public override void RegisterLogin(string userId, string googleId)
        {
            throw new NotImplementedException();
        }

        public override void RegisterTokne(string userId, string accessToken, string refreshToken = null)
        {
            throw new NotImplementedException();
        }

        protected abstract ClaimsIdentity CreateIdentity(TUser user);
    }
}
