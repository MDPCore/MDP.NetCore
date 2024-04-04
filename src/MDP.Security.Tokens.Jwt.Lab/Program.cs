using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MDP.Security.Tokens.Jwt.Lab
{
    public partial class Program
    {
        // Methods
        public static void Run(SecurityTokenFactory securityTokenFactory)
        {
            #region Contracts

            if (securityTokenFactory == null) throw new ArgumentException($"{nameof(securityTokenFactory)}=null");

            #endregion

            // ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(authenticationType: "TestAuth", claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Clark"),
                new Claim(ClaimTypes.Email, "Clark@example.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User")
            });

            // SecurityToken
            var securityToken = securityTokenFactory.CreateToken(claimsIdentity);
            if (string.IsNullOrEmpty(securityToken) == true) throw new InvalidOperationException($"{nameof(securityToken)}=null");
            Console.WriteLine(securityToken);
            Console.WriteLine();
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }

    public partial class Program
    {
        // Methods
        public static void CreateHmacKey()
        {
            // HmacKey
            string hmacKey = string.Empty;
            using (HMACSHA256 hmac = new HMACSHA256())
            {
                hmacKey = Convert.ToBase64String(hmac.Key);
            }

            // Display
            Console.WriteLine("HMAC Key:");
            Console.WriteLine(hmacKey);
            Console.ReadLine();
        }

        public static void CreateRsaKey()
        {
            // RsaKey
            string publicKey = string.Empty;
            string privateKey = string.Empty;
            using (RSA rsa = RSA.Create(2048))
            {
                // PublicKey
                publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                publicKey = $"-----BEGIN RSA PUBLIC KEY-----\n{publicKey}\n-----END RSA PUBLIC KEY-----";
                publicKey = publicKey.Replace("\r", "").Replace("\n", "");

                // PrivateKey
                privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                privateKey = $"-----BEGIN RSA PRIVATE KEY-----\n{privateKey}\n-----END RSA PRIVATE KEY-----";
                privateKey = privateKey.Replace("\r", "").Replace("\n", "");
            }

            // Display
            Console.WriteLine("RSA Public Key:");
            Console.WriteLine(publicKey);
            Console.WriteLine();
            Console.WriteLine("RSA Private Key:");
            Console.WriteLine(privateKey);
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
