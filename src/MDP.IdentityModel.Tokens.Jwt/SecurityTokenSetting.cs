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
    public class SecurityTokenSetting
    {
        // Properties
        public string Issuer { get; set; } = String.Empty;

        public string SignKey { get; set; } = String.Empty;

        public string Algorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        public int ExpireMinutes { get; set; } = 30;
    }
}
