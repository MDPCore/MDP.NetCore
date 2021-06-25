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

namespace MDP.AspNetCore.Authentication.JwtBearer
{
    public static class SecurityTokenValidationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, Action<SecurityTokenValidationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // Return
            return builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, string authenticationScheme, Action<SecurityTokenValidationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationOptions
            if (configureOptions != null) builder.Services.Configure(authenticationScheme, configureOptions);

            // JwtBearerScheme
            builder.Services.AddOptions<JwtBearerOptions>(authenticationScheme).Configure<IOptionsMonitor<SecurityTokenValidationOptions>>((jwtBearerOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // jwtBearerOptions.SecurityTokenValidators
                {
                    jwtBearerOptions.SecurityTokenValidators.Clear();
                    jwtBearerOptions.SecurityTokenValidators.Add(new SecurityTokenValidationHandler());
                }

                // jwtBearerOptions.ValidationParameters
                {
                    // Setting
                    jwtBearerOptions.TokenValidationParameters.AuthenticationType = "JwtBearer";

                    // Issuer
                    jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
                    jwtBearerOptions.TokenValidationParameters.ValidIssuer = authenticationOptions.Issuer;

                    // Audience
                    jwtBearerOptions.TokenValidationParameters.ValidateAudience = false;
                    jwtBearerOptions.TokenValidationParameters.ValidAudience = null;

                    // Lifetime
                    jwtBearerOptions.TokenValidationParameters.ValidateLifetime = true;
                    jwtBearerOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;

                    // Signing                        
                    jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.SignKey));
                }
            });
            builder.AddJwtBearer(authenticationScheme, null, null);

            // Return
            return builder;
        }
    }
}
