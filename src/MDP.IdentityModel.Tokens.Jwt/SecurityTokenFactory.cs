using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MDP.IdentityModel.Tokens.Jwt
{
    public class SecurityTokenFactory
    {
        // Fields
        private readonly SecurityTokenSetting _setting;

        private readonly JwtSecurityTokenHandler _tokenHandler;

        private readonly SigningCredentials _signingCredentials;


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

            // JwtSecurityTokenHandler
            _tokenHandler = new JwtSecurityTokenHandler();

            // SigningCredentials
            _signingCredentials = this.CreareSigningCredentials(_setting.SignKey, _setting.Algorithm);
            if (_signingCredentials == null) throw new ArgumentException($"{nameof(_signingCredentials)}=null");
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
                SigningCredentials = _signingCredentials
            };

            // TokenString
            var tokenString = _tokenHandler.CreateEncodedJwt(tokenDescriptor);
            if (string.IsNullOrEmpty(tokenString) == true) throw new InvalidOperationException($"{nameof(tokenString)}=null");

            // Return
            return tokenString;
        }


        private SigningCredentials CreareSigningCredentials(string signKey, string algorithm)
        {
            #region Contracts

            if (string.IsNullOrEmpty(signKey) == true) throw new ArgumentException($"{nameof(signKey)}=null");
            if (string.IsNullOrEmpty(algorithm) == true) throw new ArgumentException($"{nameof(algorithm)}=null");

            #endregion

            // SecurityKey
            var securityKey = this.CreateSecurityKey(signKey);
            if (securityKey == null) throw new ArgumentException($"{nameof(signKey)}=null");

            // SymmetricSecurityKey
            var signingCredentials = new SigningCredentials
            (
                key: securityKey,
                algorithm: algorithm
            );

            // Return
            return signingCredentials;
        }

        private SecurityKey CreateSecurityKey(string signKey)
        {
            #region Contracts

            if (string.IsNullOrEmpty(signKey) == true) throw new ArgumentException($"{nameof(signKey)}=null");

            #endregion

            // RSA
            if (signKey.StartsWith("RSA ", StringComparison.OrdinalIgnoreCase) == true)
            {
                // SignKey
                signKey = signKey.Substring("RSA ".Length).Trim();
                if (string.IsNullOrEmpty(signKey) == false) signKey = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(signKey));
                if (string.IsNullOrEmpty(signKey) == true) throw new InvalidOperationException($"{nameof(signKey)}=null");

                // RsaKey
                var rsa = RSA.Create();
                {
                    // Create
                    rsa.ImportFromPem(signKey);
                    var rsaKey = new RsaSecurityKey(rsa);

                    // Return
                    return rsaKey;
                }
            }

            // Symmetric
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));
        }
    }
}
