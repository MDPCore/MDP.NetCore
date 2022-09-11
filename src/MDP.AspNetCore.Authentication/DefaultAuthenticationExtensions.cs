using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication
{
    public static class DefaultAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddDefaultAuthentication(this IServiceCollection services, DefaultAuthenticationSetting? authenticationSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException($"{nameof(services)}=null");
         
            #endregion

            // AuthenticationSetting
            if (authenticationSetting == null) authenticationSetting = new DefaultAuthenticationSetting();
          
            // AuthenticationBuilder   
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                // DefaultScheme
                options.DefaultScheme = PolicyAuthenticationDefaults.AuthenticationScheme;
            });

            // Policy
            authenticationBuilder.AddPolicy(new PolicyAuthenticationSetting()
            {
                // DefaultScheme
                DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme
            });

            // Cookie
            authenticationBuilder.AddCookie(options =>
            {
                // Options
                options.LoginPath = new PathString("/Login");
                options.LogoutPath = new PathString("/Logout");
                options.AccessDeniedPath = new PathString("/AccessDenied");
            });

            // ExternalCookie
            authenticationBuilder.AddExternalCookie(options =>
            {
                // Options
                options.LoginPath = new PathString("/Login");
                options.LogoutPath = new PathString("/Logout");
                options.AccessDeniedPath = new PathString("/AccessDenied");
            });

            // Return
            return authenticationBuilder;
        }
    }
}
