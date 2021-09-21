using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MDP.Identity.Password
{
    public class PasswordDefaults
    {
        // Constants
        public static readonly string LoginType = "Password";

        public static Int64 ExpireMinutes { get; set; } = Int64.MaxValue;
    }
}