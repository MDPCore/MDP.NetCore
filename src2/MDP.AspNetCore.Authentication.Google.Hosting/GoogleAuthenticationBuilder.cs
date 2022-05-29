using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Google.Hosting
{
    public class GoogleAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<GoogleAuthenticationSetting>
    {
        // Constructors
        public GoogleAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "Google";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, GoogleAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddDefaultAuthentication
            hostBuilder.Services.AddGoogleAuthentication(authenticationSetting);
        }
    }
}
