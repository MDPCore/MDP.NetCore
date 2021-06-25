using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.JwtBearer
{
    public class SecurityTokenValidationHandler : JwtSecurityTokenHandler
    {
        // Constructors
        public SecurityTokenValidationHandler() : base()
        {

        }


        // Methods
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            #region Contracts

            if (string.IsNullOrEmpty(token) == true) throw new ArgumentException(nameof(token));
            if (validationParameters == null) throw new ArgumentException(nameof(validationParameters));

            #endregion

            // ClaimsPrincipal
            var claimsPrincipal = base.ValidateToken(token, validationParameters, out validatedToken);
            if (claimsPrincipal == null) return claimsPrincipal;

            // ClaimsIdentity
            var claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return claimsPrincipal;

            // AuthenticationTypeClaim
            var authenticationTypeClaim = claimsIdentity.FindFirst(SecurityTokenClaimTypes.AuthenticationType);
            if (authenticationTypeClaim == null) return claimsPrincipal;
            if (string.IsNullOrEmpty(authenticationTypeClaim.Value) == true) return claimsPrincipal;

            // Create
            claimsIdentity = new ClaimsIdentity(claimsIdentity.Claims, authenticationTypeClaim.Value);
            claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Return
            return claimsPrincipal;
        }
    }
}
