using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Hosting
{
    public class DefaultAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<DefaultAuthenticationSetting>
    {
        // Constructors
        public DefaultAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, DefaultAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddDefaultAuthentication
            hostBuilder.Services.AddDefaultAuthentication(authenticationSetting);
        }
    }
}
