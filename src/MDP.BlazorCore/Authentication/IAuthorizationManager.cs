using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MDP.BlazorCore
{
    public interface IAuthorizationManager
    {
        // Methods
        Task ChallengeAsync(string scheme, string returnUrl = null);
    }
}
