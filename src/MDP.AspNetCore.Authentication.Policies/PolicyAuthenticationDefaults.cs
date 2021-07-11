using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Policies
{
    public partial class PolicyAuthenticationDefaults
    {
        // Properties
        public const string AuthenticationScheme = "Policies";
    }

    public partial class PolicyAuthenticationDefaults
    {
        // Properties
        internal const string AuthenticatePolicyScheme = ".AuthenticatePolicy";

        internal const string ChallengePolicyScheme = ".ChallengePolicy";

        internal const string ForbidPolicyScheme = ".ForbidPolicy";

        internal const string SignInPolicyScheme = ".SignInPolicy";

        internal const string SignOutPolicyScheme = ".SignOutPolicy";
    }
}
