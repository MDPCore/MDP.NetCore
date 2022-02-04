using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.WindowsAD
{
    public class WindowsADService<TUser> : IdentityService<TUser>
        where TUser : class, User
    {
        // Fields
        private readonly List<string> _hostList = null;

        private readonly int _port = 0;


        // Constructors
        public WindowsADService(List<string> hostList, int port)
        {
            #region Contracts

            if (hostList == null) throw new ArgumentException(nameof(hostList));

            #endregion

            // Default
            _hostList = hostList;
            _port = port;
        }


        // Methods
        public void SetWindowsAD(string userId, string username)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(username) == true) throw new ArgumentException(nameof(username));

            #endregion

            // Username
            username = username.ToLower();
            if (string.IsNullOrEmpty(username) == true) throw new InvalidOperationException($"{nameof(username)}=null");

            // SetWindowsAD
            this.UserLoginRepository.Remove(userId, WindowsADDefaults.LoginType);
            this.UserLoginRepository.Add(new UserLogin()
            {
                UserId = userId,
                LoginType = WindowsADDefaults.LoginType,
                LoginValue = username
            });
        }

        public TUser Login(string username)
        {
            #region Contracts

            if (string.IsNullOrEmpty(username) == true) throw new ArgumentException(nameof(username));

            #endregion

            // Username
            username = username.ToLower();
            if (string.IsNullOrEmpty(username) == true) throw new InvalidOperationException($"{nameof(username)}=null");
                      
            // UserLogin
            var userLogin = this.UserLoginRepository.FindByLoginType(WindowsADDefaults.LoginType, username);
            if (userLogin == null) return null;

            // User
            var user = this.UserRepository.FindByUserId(userLogin.UserId);
            if (user == null) return null;

            // Return
            return user;
        }

        public bool Authenticate(string username, string password)
        {
            #region Contracts

            if (string.IsNullOrEmpty(username) == true) throw new ArgumentException(nameof(username));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));

            #endregion

            // Variables
            Exception exception = null;

            // Authenticate
            foreach (var host in _hostList)
            {
                try
                {
                    if (this.Authenticate(username, password, host, _port) == true)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
            if (exception != null) throw exception;

            // Return
            return false;
        }

        private bool Authenticate(string username, string password, string host, int port)
        {
            #region Contracts

            if (string.IsNullOrEmpty(username) == true) throw new ArgumentException(nameof(username));
            if (string.IsNullOrEmpty(password) == true) throw new ArgumentException(nameof(password));
            if (string.IsNullOrEmpty(host) == true) throw new ArgumentException(nameof(host));

            #endregion

            // Username
            username = username.ToLower();
            if (string.IsNullOrEmpty(username) == true) throw new InvalidOperationException($"{nameof(username)}=null");

            // Connection
            using (var connection = new LdapConnection())
            {
                try
                {
                    // Authenticate
                    connection.Connect(host, port);
                    connection.Bind(username, password);

                    // Return
                    return true;
                }
                catch (LdapException ex)
                {
                    // Result
                    switch (ex.ResultCode)
                    {
                        // 帳號密碼錯誤
                        case 49: return false;

                        // Other
                        default: throw;
                    }
                }
            }
        }
    }
}
