using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MDP.Identity.Claims
{
    public class ClaimsDefaults
    {
        // Constants
        public static readonly string IdentifierClaimType = ClaimTypes.NameIdentifier;

        public static long ExpireMinutes { get; set; } = long.MaxValue;
    }
}