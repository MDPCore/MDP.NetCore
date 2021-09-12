using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.ExternalCookies
{
    public class ExternalAuthorizeAttribute : AuthorizeAttribute
    {
        // Constructors
        public ExternalAuthorizeAttribute() : base() 
        {
            // Default
            this.AuthenticationSchemes = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
        }

        public ExternalAuthorizeAttribute(string policy) : base(policy)
        {
            // Default
            this.AuthenticationSchemes = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
        }
    }
}
