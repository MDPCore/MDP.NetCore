using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Claims
{
    public class ClaimsService<TUser> : IdentityService<TUser>
        where TUser : UserBase
    {
        // Methods
        public TUser Login(ClaimsIdentity identity)
        {
            #region Contracts

            if (identity == null) throw new ArgumentException(nameof(identity));

            #endregion

            // Require
            if (identity.IsAuthenticated == false) throw new InvalidOperationException($"{nameof(identity.IsAuthenticated)} is failed.");

            // LoginType
            var loginType = identity.AuthenticationType;
            if (string.IsNullOrEmpty(loginType)==true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = identity.FindFirst(ClaimsDefaults.LoginValueClaimType)?.Value;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginValue(loginType, loginValue);
            if (userLogin == null) return null;
            if (userLogin.ExpiredAt <= DateTime.Now) return null;

            // User
            var user = this.UserRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // Return
            return user;
        }


        public void SetIdentity(string userId, ClaimsIdentity identity)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (identity == null) throw new ArgumentException(nameof(identity));

            #endregion

            // Require
            if (identity.IsAuthenticated == false) throw new InvalidOperationException($"{nameof(identity.IsAuthenticated)} is failed.");

            // User
            var user = this.UserRepository.FindByUserId(userId);
            if (user == null) throw new InvalidOperationException($"{nameof(user)}=null");

            // LoginType
            var loginType = identity.AuthenticationType;
            if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = identity.FindFirst(ClaimsDefaults.LoginValueClaimType)?.Value;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginValue(loginType, loginValue);
            if (userLogin == null)
            {
                // Create
                userLogin = this.CreateUserLogin(userId, loginType, loginValue);
                if (userLogin == null) throw new InvalidOperationException($"{nameof(userLogin)}=null");

                // Add
                this.UserLoginRepository.Add(userLogin);
            }
            else
            {
                // Create
                userLogin = this.CreateUserLogin(userId, loginType, loginValue);
                if (userLogin == null) throw new InvalidOperationException($"{nameof(userLogin)}=null");

                // Update
                this.UserLoginRepository.Update(userLogin);
            }
        }

        private UserLogin CreateUserLogin(string userId, string loginType, string loginValue)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));

            #endregion

            // UserLogin
            var userLogin = new UserLogin()
            {
                UserId = userId,
                LoginType = loginType,
                LoginValue = loginValue,
                ExpiredAt = (ClaimsDefaults.ExpireMinutes == Int64.MaxValue) ? DateTime.MaxValue : DateTime.Now.AddMinutes(ClaimsDefaults.ExpireMinutes)
            };

            // Return
            return userLogin;
        }
    }
}
