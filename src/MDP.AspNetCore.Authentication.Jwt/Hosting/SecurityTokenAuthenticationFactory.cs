using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Jwt
{
    [MDP.Registration.Factory<WebApplicationBuilder, SecurityTokenAuthenticationSetting>("Authentication", "Jwt")]
    public class SecurityTokenAuthenticationFactoryFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, SecurityTokenAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddSecurityTokenAuthentication
            webApplicationBuilder.Services.AddSecurityTokenAuthentication(authenticationSetting);
        }
    }
}
