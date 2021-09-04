using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.GitHub
{
    public class GitHubAuthenticationPostConfigureOptions : IPostConfigureOptions<GitHubAuthenticationOptions>
    {
        // Methods
        public void PostConfigure(string name, GitHubAuthenticationOptions options)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException(nameof(name));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Require
            if (string.IsNullOrEmpty(options.ClientId) == true) throw new InvalidOperationException($"{nameof(options.ClientId)}=null");
            if (string.IsNullOrEmpty(options.ClientSecret) == true) throw new InvalidOperationException($"{nameof(options.ClientSecret)}=null");
            //if (string.IsNullOrEmpty(options.SignInScheme) == true) throw new InvalidOperationException($"{nameof(options.SignInScheme)}=null");
        }
    }
}
