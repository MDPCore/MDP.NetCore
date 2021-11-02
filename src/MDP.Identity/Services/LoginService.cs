using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class LoginService<TUser> : IdentityService<TUser>
       where TUser : BaseUser
    {
        // Methods
        protected TUser LoginBase(string userId, string loginType, string loginValue)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));

            #endregion

            // User
            var user = this.UserRepository.FindByUserId(userId);
            if (user == null) return null;

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(userId, loginType);
            if (userLogin == null) return null;
            if (userLogin.LoginValue != loginValue) return null;
            if (userLogin.ExpiredAt <= DateTime.Now) return null;

            // Return
            return user;
        }

        protected TUser LoginBase(string userPropertyName, string userPropertyValue, string loginType, string loginValue)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userPropertyName) == true) throw new ArgumentException(nameof(userPropertyName));
            if (string.IsNullOrEmpty(userPropertyValue) == true) throw new ArgumentException(nameof(userPropertyValue));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));

            #endregion

            // UserList
            var userList = this.UserRepository.FindAllByProperty(userPropertyName, userPropertyValue);
            if (userList == null) throw new InvalidOperationException($"{nameof(userList)}=null");
            if (userList.Count > 1) throw new InvalidOperationException($"Duplicate userPropertyName and userPropertyValue for LoginBase(): {nameof(userPropertyName)}={userPropertyName}, {nameof(userPropertyValue)}={userPropertyValue}.");

            // User
            var user = userList.First();
            if (user == null) return null;

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(user.UserId, loginType);
            if (userLogin == null) return null;
            if (userLogin.LoginValue != loginValue) return null;
            if (userLogin.ExpiredAt <= DateTime.Now) return null;

            // Return
            return user;
        }

        protected TUser LoginBase(string loginType, string loginValue)
        {
            #region Contracts

            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));

            #endregion

            // UserLoginList
            var userLoginList = this.UserLoginRepository.FindAllByLoginValue(loginType, loginValue);
            if (userLoginList == null) throw new InvalidOperationException($"{nameof(userLoginList)}=null");
            if (userLoginList.Count > 1) throw new InvalidOperationException($"Duplicate loginType and loginValue for LoginBase(): {nameof(loginType)}={loginType}, {nameof(loginValue)}={loginValue}.");

            // UserLogin
            var userLogin = userLoginList.First();
            if (userLogin == null) return null;
            if (userLogin.LoginValue != loginValue) return null;
            if (userLogin.ExpiredAt <= DateTime.Now) return null;

            // User
            var user = this.UserRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // Return
            return user;
        }


        protected void AllowLoginBase(string userId, string loginType, string loginValue, long expireMinutes)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));
            if (expireMinutes <= 0) throw new ArgumentException(nameof(expireMinutes));

            #endregion

            // User
            var user = this.UserRepository.FindByUserId(userId);
            if (user == null) throw new InvalidOperationException($"{nameof(user)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(user.UserId, loginType);
            if (userLogin == null)
            {
                // Add
                this.UserLoginRepository.Add(new UserLogin()
                {
                    UserId = user.UserId,
                    LoginType = loginType,
                    LoginValue = loginValue,
                    ExpiredAt = (expireMinutes != long.MaxValue) ? DateTime.Now.AddMinutes(expireMinutes) : DateTime.MaxValue
                });
            }
            else
            {
                // Update
                this.UserLoginRepository.Add(new UserLogin()
                {
                    UserId = user.UserId,
                    LoginType = loginType,
                    LoginValue = loginValue,
                    ExpiredAt = (expireMinutes != long.MaxValue) ? DateTime.Now.AddMinutes(expireMinutes) : DateTime.MaxValue
                });
            }
        }

        protected void AllowLoginBase(string userPropertyName, string userPropertyValue, string loginType, string loginValue, long expireMinutes)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userPropertyName) == true) throw new ArgumentException(nameof(userPropertyName));
            if (string.IsNullOrEmpty(userPropertyValue) == true) throw new ArgumentException(nameof(userPropertyValue));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));
            if (expireMinutes <= 0) throw new ArgumentException(nameof(expireMinutes));

            #endregion

            // UserList
            var userList = this.UserRepository.FindAllByProperty(userPropertyName, userPropertyValue);
            if (userList == null) throw new InvalidOperationException($"{nameof(userList)}=null");
            if (userList.Count > 1) throw new InvalidOperationException($"Duplicate userPropertyName and userPropertyValue for AllowLoginBase(): {nameof(userPropertyName)}={userPropertyName}, {nameof(userPropertyValue)}={userPropertyValue}.");

            // User
            var user = userList.First();
            if (user == null) throw new InvalidOperationException($"{nameof(user)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(user.UserId, loginType);
            if (userLogin == null)
            {
                // Add
                this.UserLoginRepository.Add(new UserLogin()
                {
                    UserId = user.UserId,
                    LoginType = loginType,
                    LoginValue = loginValue,
                    ExpiredAt = (expireMinutes != long.MaxValue) ? DateTime.Now.AddMinutes(expireMinutes) : DateTime.MaxValue
                });
            }
            else
            {
                // Update
                this.UserLoginRepository.Add(new UserLogin()
                {
                    UserId = user.UserId,
                    LoginType = loginType,
                    LoginValue = loginValue,
                    ExpiredAt = (expireMinutes != long.MaxValue) ? DateTime.Now.AddMinutes(expireMinutes) : DateTime.MaxValue
                });
            }
        }
    }
}
