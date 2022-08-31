using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Liff.Hosting
{
    public class LiffAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<LiffAuthenticationSetting>
    {
        // Constructors
        public LiffAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "Liff";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, LiffAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddDefaultAuthentication
            hostBuilder.Services.AddLiffAuthentication(authenticationSetting);
        }
    }
}
