using Microsoft.Extensions.Configuration;
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
    public class SecurityTokenValidationOptions
    {
        // Properties
        public string Issuer { get; set; } = null;

        public string SignKey { get; set; } = null;
    }
}
