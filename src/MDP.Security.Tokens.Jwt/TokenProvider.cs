using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace MDP.Security.Tokens.Jwt
{
    public class TokenProvider
    {
        // Fields
        private readonly JwtSecurityTokenHandler _securityTokenHandler = new JwtSecurityTokenHandler();

        private readonly SigningCredentials _signingCredentials = null;

        private readonly string _issuer = null;

        private readonly int _expirationMinutes = 30;


        // Constructors
        public TokenProvider(string algorithm, string signKey, string issuer = null, int expirationMinutes = 30)
        {
            #region Contracts

            if (string.IsNullOrEmpty(algorithm) == true) throw new ArgumentException($"{nameof(algorithm)}=null");
            if (string.IsNullOrEmpty(signKey) == true) throw new ArgumentException($"{nameof(signKey)}=null");

            #endregion

            // Default
            _issuer = issuer;
            _expirationMinutes = expirationMinutes;

            // SecurityKey
            var securityKey = this.CreareSecurityKey(algorithm, signKey);
            if (securityKey == null) throw new InvalidOperationException($"{nameof(securityKey)}=null");

            // SigningCredentials
            _signingCredentials = new SigningCredentials(securityKey, algorithm);
        }


        // Methods
        public string CreateToken(ClaimsIdentity identity)
        {
            #region Contracts

            if (identity == null) throw new ArgumentException($"{nameof(identity)}=null");

            #endregion

            // Require
            if (string.IsNullOrEmpty(identity.AuthenticationType) == true) throw new InvalidOperationException($"{nameof(identity.AuthenticationType)}=null");

            // ClaimList
            var claimList = new List<Claim>(identity.Claims);
            {
                // AuthenticationType
                claimList.RemoveAll(claim => claim.Type == AuthenticationClaimTypes.AuthenticationType);
                claimList.Add(new Claim(AuthenticationClaimTypes.AuthenticationType, identity.AuthenticationType));

                // Issuer
                if (string.IsNullOrEmpty(_issuer) == false)
                {
                    claimList.RemoveAll(claim => claim.Type == JwtRegisteredClaimNames.Iss);
                    claimList.Add(new Claim(JwtRegisteredClaimNames.Iss, _issuer));
                }
            }

            // SecurityTokenDescriptor
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                // Claim
                Subject = new ClaimsIdentity(claimList),

                // Lifetime
                IssuedAt = DateTime.Now, // 建立時間
                NotBefore = DateTime.Now, // 在此之前不可用時間
                Expires = DateTime.Now.AddMinutes(_expirationMinutes), // 逾期時間

                // Signing
                SigningCredentials = _signingCredentials
            };

            // SecurityToken
            var securityToken = _securityTokenHandler.CreateEncodedJwt(securityTokenDescriptor);
            if (string.IsNullOrEmpty(securityToken) == true) throw new InvalidOperationException($"{nameof(securityToken)}=null");

            // Return
            return securityToken;
        }

        private SecurityKey CreareSecurityKey(string algorithm, string signKey)
        {
            #region Contracts

            if (string.IsNullOrEmpty(algorithm) == true) throw new ArgumentException($"{nameof(algorithm)}=null");
            if (string.IsNullOrEmpty(signKey) == true) throw new ArgumentException($"{nameof(signKey)}=null");

            #endregion

            // HMAC+SHA
            if (algorithm.StartsWith("HS", StringComparison.OrdinalIgnoreCase) == true)
            {
                // SignKeyBytes
                var signKeyBytes = Convert.FromBase64String(signKey);
                if (signKeyBytes == null) throw new InvalidOperationException($"{nameof(signKeyBytes)}=null");

                // SecurityKey
                var securityKey = new SymmetricSecurityKey(signKeyBytes);

                // Return
                return securityKey;
            }

            // RSA+SHA
            if (algorithm.StartsWith("RS", StringComparison.OrdinalIgnoreCase) == true)
            {
                // SignKeyString
                var signKeyString = signKey;
                if (string.IsNullOrEmpty(signKeyString) == true) throw new InvalidOperationException($"{nameof(signKeyString)}=null");

                // RsaKey
                var rsaKey = RSA.Create();
                rsaKey.ImportFromPem(signKeyString);

                // SecurityKey
                var securityKey = new RsaSecurityKey(rsaKey);

                // Return
                return securityKey;
            }

            // ECDSA+SHA
            if (algorithm.StartsWith("ES", StringComparison.OrdinalIgnoreCase) == true)
            {
                // SignKeyString
                var signKeyString = signKey;
                if (string.IsNullOrEmpty(signKeyString) == true) throw new InvalidOperationException($"{nameof(signKeyString)}=null");

                // EcdsaKey
                var ecdsaKey = ECDsa.Create();
                ecdsaKey.ImportFromPem(signKeyString);

                // SecurityKey
                var securityKey = new ECDsaSecurityKey(ecdsaKey);

                // Return
                return securityKey;
            }

            // Other
            return null;
        }
    }
}
