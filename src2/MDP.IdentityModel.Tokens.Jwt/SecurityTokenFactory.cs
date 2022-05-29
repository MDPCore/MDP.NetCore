using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.IdentityModel.Tokens.Jwt
{
    public class SecurityTokenFactory
    {
        // Fields
        private readonly SecurityTokenSetting _setting;

        private readonly JwtSecurityTokenHandler _tokenHandler;


        // Constructors
        public SecurityTokenFactory(SecurityTokenSetting setting)
        {
            #region Contracts

            if (setting == null) throw new ArgumentException(nameof(setting));

            #endregion

            // Default
            _setting = setting;
            _tokenHandler = new JwtSecurityTokenHandler();
        }


        // Methods
        public string CreateEncodedJwt(ClaimsIdentity identity)
        {
            #region Contracts

            if (identity == null) throw new ArgumentException(nameof(identity));

            #endregion

            // Require
            if (string.IsNullOrEmpty(identity.AuthenticationType) == true) throw new InvalidOperationException($"{nameof(identity.AuthenticationType)}=null");

            // ClaimList
            var claimList = new List<Claim>(identity.Claims);
            {
                // AuthenticationType
                claimList.RemoveAll(claim => claim.Type == SecurityTokenClaimTypes.AuthenticationType);
                claimList.Add(new Claim(SecurityTokenClaimTypes.AuthenticationType, identity.AuthenticationType));
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
                claimList.Add(new Claim(JwtRegisteredClaimNames.Iss, _setting.Issuer));
            }

            // TokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claim
                Subject = new ClaimsIdentity(claimList),

                // Lifetime
                IssuedAt = DateTime.Now, // 建立時間
                NotBefore = DateTime.Now, // 在此之前不可用時間
                Expires = DateTime.Now.AddMinutes(_setting.ExpireMinutes), // 逾期時間

                // Signing
                SigningCredentials = new SigningCredentials
                (
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.SignKey)),
                    algorithm: _setting.Algorithm
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
