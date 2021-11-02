using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Claims
{
    public class ClaimsService<TUser> : LoginService<TUser>
        where TUser : BaseUser
    {
        // Methods
        public TUser Login(ClaimsIdentity identity)
        {
            #region Contracts

            if (identity == null) throw new ArgumentException(nameof(identity));

            #endregion

            // This
            return this.Login(identity, ClaimsDefaults.IdentifierClaimType);
        }

        public TUser Login(ClaimsIdentity identity, string identifierClaimType)
        {
            #region Contracts

            if (identity == null) throw new ArgumentException(nameof(identity));
            if (string.IsNullOrEmpty(identifierClaimType) == true) throw new ArgumentException(nameof(identifierClaimType));

            #endregion

            // Require
            if (identity.IsAuthenticated == false) throw new InvalidOperationException($"{nameof(identity.IsAuthenticated)} is failed.");

            // LoginType
            var loginType = identity.AuthenticationType;
            if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = identity.FindFirst(identifierClaimType)?.Value;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // Base
            return this.LoginBase(loginType, loginValue);
        }


        public void AllowLogin(string userId, ClaimsIdentity identity)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (identity == null) throw new ArgumentException(nameof(identity));

            #endregion

            // AllowLogin
            this.AllowLogin(userId, identity, ClaimsDefaults.IdentifierClaimType);
        }

        public void AllowLogin(string userId, ClaimsIdentity identity, string identifierClaimType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (identity == null) throw new ArgumentException(nameof(identity));
            if (string.IsNullOrEmpty(identifierClaimType) == true) throw new ArgumentException(nameof(identifierClaimType));

            #endregion

            // Require
            if (identity.IsAuthenticated == false) throw new InvalidOperationException($"{nameof(identity.IsAuthenticated)} is failed.");

            // LoginType
            var loginType = identity.AuthenticationType;
            if (string.IsNullOrEmpty(loginType) == true) throw new InvalidOperationException($"{nameof(loginType)}=null");

            // LoginValue
            var loginValue = identity.FindFirst(identifierClaimType)?.Value;
            if (string.IsNullOrEmpty(loginValue) == true) throw new InvalidOperationException($"{nameof(loginValue)}=null");

            // Base
            this.AllowLoginBase(userId, loginType, loginValue, ClaimsDefaults.ExpireMinutes);
        }
    }
}
