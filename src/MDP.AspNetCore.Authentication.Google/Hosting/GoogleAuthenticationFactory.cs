using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Google
{
    [MDP.Registration.Factory<WebApplicationBuilder, GoogleAuthenticationSetting>("Authentication", "Google")]
    public class GoogleAuthenticationFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, GoogleAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddGoogleAuthentication
            webApplicationBuilder.Services.AddGoogleAuthentication(authenticationSetting);
        }
    }
}
