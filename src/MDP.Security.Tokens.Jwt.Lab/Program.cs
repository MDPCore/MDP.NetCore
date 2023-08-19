using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace MDP.Security.Tokens.Jwt.Lab
{
    public class Program
    {
        // Methods
        public static void Run(SecurityTokenFactory securityTokenFactory)
        {
            #region Contracts

            if (securityTokenFactory == null) throw new ArgumentException($"{nameof(securityTokenFactory)}=null");

            #endregion

            // Variables
            var username = "Clark";

            // ClaimsIdentity
            var claimsIdentity = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, username + "@hotmail.com"),
            }, "NameAuth");

            // TokenString
            var tokenString = securityTokenFactory.CreateEncodedJwt(claimsIdentity);
            if (string.IsNullOrEmpty(tokenString) == true) throw new InvalidOperationException($"{nameof(tokenString)}=null");

            // Execute
            Console.WriteLine(tokenString);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
