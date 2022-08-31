using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Line.Hosting
{
    public class LineAuthenticationBuilder : MDP.AspNetCore.ServiceBuilder<LineAuthenticationSetting>
    {
        // Constructors
        public LineAuthenticationBuilder()
        {
            // Default
            this.ServiceNamespace = "Authentication";
            this.ServiceName = "Line";
        }


        // Methods
        protected override void ConfigureService(WebApplicationBuilder hostBuilder, LineAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddDefaultAuthentication
            hostBuilder.Services.AddLineAuthentication(authenticationSetting);
        }
    }
}
