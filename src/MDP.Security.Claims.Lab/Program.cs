using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace MDP.Security.Claims.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // ClaimsPrincipal
            var claimsIdentity = new ClaimsIdentity(authenticationType: "TestAuth", claims: new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Clark"),
                new Claim(ClaimTypes.Email, "Clark@example.com"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User")
            });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Execute
            Console.WriteLine($"AuthenticationType= {claimsPrincipal.Identity?.AuthenticationType}");
            Console.WriteLine($"UserId= {claimsPrincipal.GetClaimValue(ClaimTypes.NameIdentifier)}");
            Console.WriteLine($"Name= {claimsPrincipal.GetClaimValue(ClaimTypes.Name)}");
            Console.WriteLine($"Mail= {claimsPrincipal.GetClaimValue(ClaimTypes.Email)}");
            Console.WriteLine($"Role= {string.Join(",", claimsPrincipal.GetAllClaimValue(ClaimTypes.Role))}");
        }
    }
}
