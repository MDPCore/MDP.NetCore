using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.JwtBearer
{
    public class SecurityTokenValidationParameters : TokenValidationParameters
    {
        // Constructors
        public SecurityTokenValidationParameters() : base() { }

        public SecurityTokenValidationParameters(TokenValidationParameters other) : base(other) { }

        public SecurityTokenValidationParameters(SecurityTokenValidationParameters other) : base(other) { }


        // Methods
        public override ClaimsIdentity CreateClaimsIdentity(SecurityToken securityToken, string issuer)
        {
            // Base
            var claimsIdentity = base.CreateClaimsIdentity(securityToken, issuer);
            if (claimsIdentity == null) throw new InvalidOperationException($"{nameof(claimsIdentity)}=null");

            // AuthenticationTypeClaim
            var authenticationTypeClaim = claimsIdentity.FindFirst(SecurityTokenClaimTypes.AuthenticationType);
            if (authenticationTypeClaim == null) return claimsIdentity;

            // Create
            claimsIdentity = new ClaimsIdentity(claimsIdentity.Claims, authenticationTypeClaim.Value);

            // Return
            return claimsIdentity;
        }
    }
}
