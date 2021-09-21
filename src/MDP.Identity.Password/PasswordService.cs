using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Password
{
    public class PasswordService<TUser> : IdentityService<TUser>
        where TUser : UserBase
    {
        // Fields
        private readonly PasswordHasher _passwordHasher = null;


        // Constructors
        public PasswordService(PasswordHasher passwordHasher)
        {
            #region Contracts

            if (passwordHasher == null) throw new ArgumentException(nameof(passwordHasher));

            #endregion

            // Default
            _passwordHasher = passwordHasher;
        }


        // Methods
        public TUser Login(string userId, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // User
            var user = this.UserRepository.FindByUserId(userId);
            if (user == null) return null;

            // Login
            return this.Login(user, password);
        }

        public TUser Login(string propertyName, string propertyValue, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(propertyName) == true) throw new ArgumentException(nameof(propertyName));
            if (string.IsNullOrEmpty(propertyValue) == true) throw new ArgumentException(nameof(propertyValue));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // User
            var user = this.UserRepository.FindByProperty(propertyName, propertyValue);
            if (user == null) return null;

            // Login
            return this.Login(user, password);
        }

        private TUser Login(TUser user, string password)
        {
            #region Contracts

            if (user==null) throw new ArgumentException(nameof(user));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // Password
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(user.UserId, PasswordDefaults.LoginType);
            if (userLogin == null) return null;
            if (userLogin.LoginValue != password) return null;
            if (userLogin.ExpiredAt <= DateTime.Now) return null;

            // Return
            return user;
        }


        public void SetPassword(string userId, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // User
            var user = this.UserRepository.FindByUserId(userId);
            if (user == null) throw new InvalidOperationException($"{nameof(user)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(user.UserId, PasswordDefaults.LoginType);
            if (userLogin == null)
            {
                // Create
                userLogin = this.CreateUserLogin(userId, password);
                if (userLogin == null) throw new InvalidOperationException($"{nameof(userLogin)}=null");

                // Add
                this.UserLoginRepository.Add(userLogin);
            }
            else
            {
                // Create
                userLogin = this.CreateUserLogin(userId, password);
                if (userLogin == null) throw new InvalidOperationException($"{nameof(userLogin)}=null");

                // Update
                this.UserLoginRepository.Update(userLogin);
            }
        }

        private UserLogin CreateUserLogin(string userId, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // Password
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // UserLogin
            var userLogin = new UserLogin()
            {
                UserId = userId,
                LoginType = PasswordDefaults.LoginType,
                LoginValue = password,
                ExpiredAt = (PasswordDefaults.ExpireMinutes == Int64.MaxValue) ? DateTime.MaxValue : DateTime.Now.AddMinutes(PasswordDefaults.ExpireMinutes)
            };

            // Return
            return userLogin;
        }
    }
}
