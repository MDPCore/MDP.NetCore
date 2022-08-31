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
    public class SecurityTokenAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<SecurityTokenAuthenticationSetting>
    {
        // Constructors
        public SecurityTokenAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "JwtBearer";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, SecurityTokenAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddJwtBearer
            hostBuilder.Services.AddJwtBearer(authenticationSetting);
        }
    }
}
