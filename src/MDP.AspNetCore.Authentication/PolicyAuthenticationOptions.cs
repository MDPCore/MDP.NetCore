#nullable enable

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication
{
    public class PolicyAuthenticationOptions
    {
        // Properties
        public string? DefaultScheme { get; set; }

        public Func<HttpContext, string>? AuthenticateSchemePolicy { get; set; }

        public Func<HttpContext, string>? ChallengeSchemePolicy { get; set; }

        public Func<HttpContext, string>? ForbidSchemePolicy { get; set; }

        public Func<HttpContext, string>? SignInSchemePolicy { get; set; }

        public Func<HttpContext, string>? SignOutSchemePolicy { get; set; }
    }
}
