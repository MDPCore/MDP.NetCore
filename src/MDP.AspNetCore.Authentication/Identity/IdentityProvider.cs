using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication
{
    public interface IdentityProvider
    {
        // Properties
        string? RegisterPath { get; set; }


        // Methods
        ClaimsIdentity SignIn(ClaimsIdentity externalIdentity);
    }

    internal class DefaultIdentityProvider : IdentityProvider
    {
        // Singleton 
        private static DefaultIdentityProvider? _instance = null;
        public static DefaultIdentityProvider Current
        {
            get
            {
                // Create
                if (_instance == null)
                {
                    _instance = new DefaultIdentityProvider();
                }

                // Return
                return _instance;
            }
        }


        // Properties
        public string? RegisterPath { get; set; } = string.Empty;


        // Methods
        public ClaimsIdentity SignIn(ClaimsIdentity externalIdentity)
        {
            #region Contracts

            if (externalIdentity == null) throw new ArgumentException($"{nameof(externalIdentity)}=null");

            #endregion

            // Return
            return externalIdentity;
        }
    }
}
