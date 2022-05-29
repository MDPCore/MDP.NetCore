using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Facebook.Hosting
{
    public class FacebookAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<FacebookAuthenticationSetting>
    {
        // Constructors
        public FacebookAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "Facebook";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, FacebookAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddDefaultAuthentication
            hostBuilder.Services.AddFacebookAuthentication(authenticationSetting);
        }
    }
}
