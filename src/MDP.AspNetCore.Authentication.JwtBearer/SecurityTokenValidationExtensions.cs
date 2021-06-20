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

namespace MDP.AspNetCore.Authentication.JwtBearer
{
    public static class JwtBearerExtensions
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

            // ValidationOptions
            if (configureOptions != null) builder.Services.Configure(authenticationScheme, configureOptions);

            // JwtBearer
            builder.Services.AddOptions<JwtBearerOptions>(authenticationScheme).Configure<IOptionsMonitor<SecurityTokenValidationOptions>>((jwtOptions, validationOptionsMonitor) =>
            {
                // ValidationOptions
                var validationOptions = validationOptionsMonitor.Get(authenticationScheme);
                if (validationOptions == null) throw new InvalidOperationException($"{nameof(validationOptions)}=null");

                // ValidationParameters
                jwtOptions.TokenValidationParameters = new SecurityTokenValidationParameters(jwtOptions.TokenValidationParameters)
                {
                    // Setting
                    AuthenticationType = "JwtBearer",

                    // Issuer
                    ValidateIssuer = true,
                    ValidIssuer = validationOptions.Issuer,

                    // Audience
                    ValidateAudience = false,
                    ValidAudience = null,

                    // Lifetime
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    // Signing                        
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(validationOptions.SignKey)),
                };
            });
            builder.AddJwtBearer(authenticationScheme, null, null);

            // Return
            return builder;
        }
    }
}
