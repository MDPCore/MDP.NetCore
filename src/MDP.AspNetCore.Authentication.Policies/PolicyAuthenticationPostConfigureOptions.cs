using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Policies
{
    public class PolicyAuthenticationPostConfigureOptions : IPostConfigureOptions<PolicyAuthenticationOptions>
    {
        // Methods
        public void PostConfigure(string name, PolicyAuthenticationOptions options)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException(nameof(name));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Require
            if (string.IsNullOrEmpty(options.DefaultScheme) == true) throw new InvalidOperationException($"{nameof(options.DefaultScheme)}=null");
        }
    }
}
