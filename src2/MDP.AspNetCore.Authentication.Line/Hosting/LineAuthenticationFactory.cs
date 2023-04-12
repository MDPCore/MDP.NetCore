using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Line
{
    [MDP.Registration.Factory<WebApplicationBuilder, LineAuthenticationSetting>("Authentication", "Line")]
    public class LineAuthenticationFactoryFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, LineAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddLineAuthentication
            webApplicationBuilder.Services.AddLineAuthentication(authenticationSetting);
        }
    }
}
