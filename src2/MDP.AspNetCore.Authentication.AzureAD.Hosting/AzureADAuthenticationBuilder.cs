using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.AzureAD.Hosting
{
    public class AzureADAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<AzureADAuthenticationSetting>
    {
        // Constructors
        public AzureADAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "AzureAD";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, AzureADAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddDefaultAuthentication
            hostBuilder.Services.AddAzureADAuthentication(authenticationSetting);
        }
    }
}
