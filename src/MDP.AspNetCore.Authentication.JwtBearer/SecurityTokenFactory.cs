using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
    public class SecurityTokenFactory
    {
        // Fields
        private readonly JwtSecurityTokenHandler _tokenHandler = null;

        private readonly SecurityTokenFactoryOptions _options = null;


        // Constructors
        public SecurityTokenFactory(IOptions<SecurityTokenFactoryOptions> options)
        {
            #region Contracts

            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Default
            _tokenHandler = new JwtSecurityTokenHandler();

            // Options
            _options = options.Value;
            if (_options == null) throw new InvalidOperationException($"{nameof(_options)}=null"); 
        }


        // Methods
        public string CreateEncodedJwt(ClaimsIdentity identity)
        {
            #region Contracts

            if (identity == null) throw new ArgumentException(nameof(identity));

            #endregion

            // ClaimList
            var claimList = new List<Claim>(identity.Claims);
            {
                // AuthenticationType
                claimList.RemoveAll(claim => claim.Type == SecurityTokenClaimTypes.AuthenticationType);
                claimList.Add(new Claim(SecurityTokenClaimTypes.AuthenticationType, identity.AuthenticationType));

                // Name
                claimList.RemoveAll(claim => claim.Type == ClaimTypes.Name);
                claimList.Add(new Claim(ClaimTypes.Name, identity.Name));
            }
           
            // CreateEncodedJwt
            return this.CreateEncodedJwt(claimList);
        }

        public string CreateEncodedJwt(IEnumerable<Claim> claims)
        {
            #region Contracts

            if (claims == null) throw new ArgumentException(nameof(claims));

            #endregion

            // ClaimList
            var claimList = new List<Claim>(claims);
            {
                // Issuer
                claimList.RemoveAll(claim => claim.Type == JwtRegisteredClaimNames.Iss);
                claimList.Add(new Claim(JwtRegisteredClaimNames.Iss, _options.Issuer));
            }

            // TokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claim
                Subject = new ClaimsIdentity(claimList),

                // Lifetime
                IssuedAt = DateTime.Now, // 建立時間
                NotBefore = DateTime.Now, // 在此之前不可用時間
                Expires = DateTime.Now.AddMinutes(_options.ExpireMinutes), // 逾期時間

                // Signing
                SigningCredentials = new SigningCredentials
                (
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SignKey)),
                    algorithm: _options.Algorithm
                ),
            };

            // TokenString
            var tokenString = _tokenHandler.CreateEncodedJwt(tokenDescriptor);
            if (string.IsNullOrEmpty(tokenString) == true) throw new InvalidOperationException($"{nameof(tokenString)}=null");

            // Return
            return tokenString;
        }
    }
}
