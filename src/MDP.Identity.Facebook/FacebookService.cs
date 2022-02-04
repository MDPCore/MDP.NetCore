using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Facebook
{
    public class FacebookService<TUser> : IdentityService<TUser>
        where TUser : class, User
    {
        // Methods
        public void SetFacebook(string userId, string facebookId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(facebookId) == true) throw new ArgumentException(nameof(facebookId));

            #endregion

            // SetFacebook
            this.UserLoginRepository.Remove(userId, FacebookDefaults.LoginType);
            this.UserLoginRepository.Add(new UserLogin()
            {
                UserId = userId,
                LoginType = FacebookDefaults.LoginType,
                LoginValue = facebookId
            });
        }

        public TUser Login(string facebookId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(facebookId) == true) throw new ArgumentException(nameof(facebookId));

            #endregion

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(FacebookDefaults.LoginType, facebookId);
            if (userLogin == null) return null;

            // User
            var user = this.UserRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // Return
            return user;
        }
    }
}
