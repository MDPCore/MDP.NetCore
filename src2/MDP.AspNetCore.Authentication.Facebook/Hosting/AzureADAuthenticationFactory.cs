using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Facebook
{
    [MDP.Registration.Factory<WebApplicationBuilder, FacebookAuthenticationSetting>("Authentication", "Facebook")]
    public class FacebookAuthenticationFactoryFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, FacebookAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddFacebookAuthentication
            webApplicationBuilder.Services.AddFacebookAuthentication(authenticationSetting);
        }
    }
}
