using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.GitHub
{
    [MDP.Registration.Factory<WebApplicationBuilder, GitHubAuthenticationSetting>("Authentication", "GitHub")]
    public class GitHubAuthenticationFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, GitHubAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddGitHubAuthentication
            webApplicationBuilder.Services.AddGitHubAuthentication(authenticationSetting);
        }
    }
}
