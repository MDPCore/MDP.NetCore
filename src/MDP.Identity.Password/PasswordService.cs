using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Password
{
    public class PasswordService<TUser> : IdentityService<TUser>
        where TUser : class, User
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
        public void SetPassword(string userId, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // HashPassword
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // SetPassword
            this.UserLoginRepository.Remove(userId, PasswordDefaults.LoginType);
            this.UserLoginRepository.Add(new UserLogin()
            {
                UserId = userId,
                LoginType = PasswordDefaults.LoginType,
                LoginValue = password
            });
        }

        public TUser Login(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByUserId(userId, PasswordDefaults.LoginType);
            if (userLogin == null) return null;

            // User
            var user = this.UserRepository.FindByUserId(userId);
            if (user == null) return null;

            // Return
            return user;
        }

        public bool Authenticate(string userId, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // HashPassword
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(PasswordDefaults.LoginType, password, userId);
            if (userLogin == null) return false;

            // Return
            return true;
        }
    }
}
