using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Password
{
    public class PasswordService<TUser> : LoginService<TUser>
        where TUser : BaseUser
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

            // Password
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // LoginType
            var loginType = PasswordDefaults.LoginType;
            if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = password;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // Base
            return this.LoginBase(userId, loginType, loginValue);
        }

        public TUser Login(string userPropertyName, string userPropertyValue, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userPropertyName) == true) throw new ArgumentException(nameof(userPropertyName));
            if (string.IsNullOrEmpty(userPropertyValue) == true) throw new ArgumentException(nameof(userPropertyValue));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // Password
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // LoginType
            var loginType = PasswordDefaults.LoginType;
            if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = password;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // Base
            return this.LoginBase(userPropertyName, userPropertyValue, loginType, loginValue);
        }


        public void AllowLogin(string userId, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // Password
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // LoginType
            var loginType = PasswordDefaults.LoginType;
            if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = password;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // Base
            this.AllowLoginBase(userId, loginType, loginValue, PasswordDefaults.ExpireMinutes);
        }

        public void AllowLogin(string userPropertyName, string userPropertyValue, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userPropertyName) == true) throw new ArgumentException(nameof(userPropertyName));
            if (string.IsNullOrEmpty(userPropertyValue) == true) throw new ArgumentException(nameof(userPropertyValue));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // Password
            password = _passwordHasher.HashPassword(password);
            if (string.IsNullOrEmpty(password) == true) throw new InvalidOperationException($"{nameof(password)}=null");

            // LoginType
            var loginType = PasswordDefaults.LoginType;
            if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = password;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // Base
            this.AllowLoginBase(userPropertyName, userPropertyValue, loginType, loginValue, PasswordDefaults.ExpireMinutes);
        }
    }
}
