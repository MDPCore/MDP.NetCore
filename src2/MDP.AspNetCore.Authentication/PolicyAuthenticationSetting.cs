#nullable enable

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication
{
    public class PolicyAuthenticationSetting
    {
        // Properties
        public string? DefaultScheme { get; set; }

        public Func<HttpContext, string>? AuthenticatePolicy { get; set; }

        public Func<HttpContext, string>? ChallengePolicy { get; set; }

        public Func<HttpContext, string>? ForbidPolicy { get; set; }

        public Func<HttpContext, string>? SignInPolicy { get; set; }

        public Func<HttpContext, string>? SignOutPolicy { get; set; }
    }
}
