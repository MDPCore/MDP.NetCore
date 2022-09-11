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

            if (setting == null) throw new ArgumentException($"{nameof(setting)}=null");

            #endregion

            // Require
            if (string.IsNullOrEmpty(setting.Issuer) == true) throw new ArgumentException($"{nameof(setting.Issuer)}=null");
            if (string.IsNullOrEmpty(setting.SignKey) == true) throw new ArgumentException($"{nameof(setting.SignKey)}=null");
            if (string.IsNullOrEmpty(setting.Algorithm) == true) throw new ArgumentException($"{nameof(setting.Algorithm)}=null");
            if (setting.ExpireMinutes <= 0) throw new ArgumentException($"{nameof(setting.ExpireMinutes)}<=0");

            // Default
            _setting = setting;
            _tokenHandler = new JwtSecurityTokenHandler();
        }


        // Methods
        public string CreateEncodedJwt(ClaimsIdentity identity, int? expireMinutes = null)
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
            return this.CreateEncodedJwt(claimList, expireMinutes);
        }

        public string CreateEncodedJwt(IEnumerable<Claim> claims, int? expireMinutes = null)
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

            // ExpireMinutes
            if (expireMinutes.HasValue == false)
            {
                expireMinutes = _setting.ExpireMinutes;
            }

            // TokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claim
                Subject = new ClaimsIdentity(claimList),

                // Lifetime
                IssuedAt = DateTime.Now, // 建立時間
                NotBefore = DateTime.Now, // 在此之前不可用時間
                Expires = DateTime.Now.AddMinutes(expireMinutes.Value), // 逾期時間

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
