using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.GitHub.Hosting
{
    public class GitHubAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<GitHubAuthenticationSetting>
    {
        // Constructors
        public GitHubAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "GitHub";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, GitHubAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddDefaultAuthentication
            hostBuilder.Services.AddGitHubAuthentication(authenticationSetting);
        }
    }
}
