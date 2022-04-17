using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.AspNetCore.Authentication
{
    public abstract class AuthenticateIdentityService : IdentityService
    {
        // Properties
        public List<string> SaveTokenNameList { get; } = new List<string>() { "refresh_token" };


        // Methods
        public abstract ClaimsIdentity Login(AuthenticateResult authenticateResult);
    }

    public class AuthenticateIdentityService<TUser> : AuthenticateIdentityService
        where TUser : class
    {
        // Methods
        public override ClaimsIdentity Login(AuthenticateResult authenticateResult)
        {
            #region Contracts

            if (authenticateResult == null) throw new ArgumentException(nameof(authenticateResult));

            #endregion

            // Require
            if (authenticateResult.Succeeded == false) return null;

            // ExternalIdentity
            var externalIdentity = authenticateResult?.Principal?.Identity as ClaimsIdentity;
            if (externalIdentity == null) return null;
            if (externalIdentity.IsAuthenticated == false) return null;

            // UserLogin
            var externalLoginType = externalIdentity.AuthenticationType;
            var externalId = externalIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userLogin = this.UserLoginRepository.FindByLoginValue(externalLoginType, externalId);
            if (userLogin == null) return null;

            // UserIdentity
            var userIdentity = this.CreateUserIdentity(userLogin);
            if (userIdentity == null) return null;

            // SaveUserToken
            this.SaveUserToken(userLogin, authenticateResult.Properties);

            // Return
            return userIdentity;
        }

        protected virtual ClaimsIdentity CreateUserIdentity(UserLogin userLogin)
        {
            #region Contracts

            if (userLogin == null) throw new ArgumentException(nameof(userLogin));

            #endregion

            // UserRepository
            var userRepository = this.GetRepository<UserRepository<TUser>>();
            if (userRepository == null) throw new InvalidOperationException($"{nameof(userRepository)}=null");

            // User
            var user = userRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // UserIdentity
            var userIdentity = new ClaimsIdentity(userLogin.LoginType)
            {
                
            };

            // Return
            return userIdentity;
        }

        protected virtual void SaveUserToken(UserLogin userLogin, AuthenticationProperties authenticationProperties)
        {
            #region Contracts

            if (userLogin == null) throw new ArgumentException(nameof(userLogin));
            if (authenticationProperties == null) throw new ArgumentException(nameof(authenticationProperties));

            #endregion

            // SaveToken
            foreach (var tokenName in this.SaveTokenNameList)
            {
                // TokenValue
                var tokenValue = authenticationProperties.GetTokenValue(tokenName);
                if (string.IsNullOrEmpty(tokenValue) == true) continue;

                // UserToken
                var userToken = new UserToken();
                userToken.UserId = userLogin.UserId;
                userToken.LoginType = userLogin.LoginType;
                userToken.TokenType = tokenName;
                userToken.TokenValue = tokenValue;

                // Save
                this.UserTokenRepository.Remove(userToken.UserId, userToken.LoginType, userToken.TokenType);
                this.UserTokenRepository.Add(userToken);
            }
        }
    }
}
