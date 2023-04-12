using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.AzureAD
{
    [MDP.Registration.Factory<WebApplicationBuilder, AzureADAuthenticationSetting>("Authentication", "AzureAD")]
    public class AzureADAuthenticationFactoryFactory
    {
        // Methods
        public void ConfigureService(WebApplicationBuilder webApplicationBuilder, AzureADAuthenticationSetting authenticationSetting)
        {
            #region Contracts

            if (webApplicationBuilder == null) throw new ArgumentException($"{nameof(webApplicationBuilder)}=null");
            if (authenticationSetting == null) throw new ArgumentException($"{nameof(authenticationSetting)}=null");

            #endregion

            // AddAzureADAuthentication
            webApplicationBuilder.Services.AddAzureADAuthentication(authenticationSetting);
        }
    }
}
