using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace MDP.AspNetCore.Authentication
{
    public static class SecurityTokenAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddJwtBearer(this IServiceCollection services, SecurityTokenAuthenticationSetting? authenticationSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException($"{nameof(services)}=null");

            #endregion

            // AddJwtBearer
            return services.AddJwtBearer(SecurityTokenAuthenticationDefaults.AuthenticationScheme, authenticationSetting);
        }

        public static AuthenticationBuilder AddJwtBearer(this IServiceCollection services, string authenticationScheme, SecurityTokenAuthenticationSetting? authenticationSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException($"{nameof(services)}=null");
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException($"{nameof(authenticationScheme)}=null");

            #endregion

            // AuthenticationSetting
            if (authenticationSetting == null) authenticationSetting = new SecurityTokenAuthenticationSetting();

            // AuthenticationBuilder   
            var authenticationBuilder = services.AddAuthentication();

            // JwtBearer
            authenticationBuilder.AddJwtBearer(authenticationScheme, null, authenticationOptions =>
            {
                // AuthenticationType
                authenticationOptions.TokenValidationParameters.AuthenticationType = authenticationScheme;

                // Issuer
                if (string.IsNullOrEmpty(authenticationSetting.Issuer) == false)
                {
                    authenticationOptions.TokenValidationParameters.ValidateIssuer = true;
                    authenticationOptions.TokenValidationParameters.ValidIssuer = authenticationSetting.Issuer;
                }
                else
                {
                    authenticationOptions.TokenValidationParameters.ValidateIssuer = false;
                    authenticationOptions.TokenValidationParameters.ValidIssuer = null;
                }

                // Audience
                authenticationOptions.TokenValidationParameters.ValidateAudience = false;
                authenticationOptions.TokenValidationParameters.ValidAudience = null;

                // Lifetime
                authenticationOptions.TokenValidationParameters.ValidateLifetime = true;
                authenticationOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;

                // SignIng
                if (string.IsNullOrEmpty(authenticationSetting.SignKey) == false)
                {
                    authenticationOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    authenticationOptions.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSetting.SignKey));
                }
                else
                {
                    authenticationOptions.TokenValidationParameters.ValidateIssuerSigningKey = false;
                    authenticationOptions.TokenValidationParameters.IssuerSigningKey = null;
                }

                // SecurityTokenValidators
                {
                    authenticationOptions.SecurityTokenValidators.Clear();
                    authenticationOptions.SecurityTokenValidators.Add(new SecurityTokenAuthenticationHandler());
                }
            });

            // Return
            return authenticationBuilder;
        }
    }
}
