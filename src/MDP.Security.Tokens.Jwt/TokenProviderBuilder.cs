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
    public class TokenProviderBuilder
    {
        // Constructors
        public TokenProviderBuilder(string name, string algorithm, string signKey, string issuer = null, int expirationMinutes = 30)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");
            if (string.IsNullOrEmpty(algorithm) == true) throw new ArgumentException($"{nameof(algorithm)}=null");
            if (string.IsNullOrEmpty(signKey) == true) throw new ArgumentException($"{nameof(signKey)}=null");

            #endregion

            // Default
            this.Name = name;
            this.Algorithm = algorithm;
            this.SignKey = signKey;
            this.Issuer = issuer;
            this.ExpirationMinutes = expirationMinutes;
        }


        // Properties
        public string Name { get; } = string.Empty;

        public string Algorithm { get; } = string.Empty;

        public string SignKey { get; } = string.Empty;

        public string Issuer { get; } = null;

        public int ExpirationMinutes { get; } = 30;


        // Methods
        public TokenProvider CreateProvider()
        {
            // TokenProvider
            var tokenProvider = new TokenProvider
            (
                this.Algorithm, 
                this.SignKey, 
                this.Issuer, 
                this.ExpirationMinutes
            );

            // Return
            return tokenProvider;
        }
    }
}
