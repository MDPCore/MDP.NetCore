using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Line
{
    public static class LiffAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddLiff(this AuthenticationBuilder builder, Action<LiffAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // Return
            return builder.AddLiff(LiffDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddLiff(this AuthenticationBuilder builder, string authenticationScheme, Action<LiffAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationOptions
            if (configureOptions != null) builder.Services.Configure(authenticationScheme, configureOptions);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<LiffAuthenticationOptions>, LiffAuthenticationPostConfigureOptions>());

            // OAuthAuthenticationOptions
            builder.Services.AddOptions<LiffAuthenticationOptions>(authenticationScheme).Configure((authenticationOptions) =>
            {
                // Claim
                authenticationOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
                authenticationOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
                authenticationOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                // Endpoint
                authenticationOptions.CallbackPath = new PathString("/signin-liff");
                authenticationOptions.VerifyEndpoint = "https://api.line.me/oauth2/v2.1/verify";
                authenticationOptions.UserInformationEndpoint = "https://api.line.me/v2/profile";                
            });

            // OAuthAuthentication
            builder.AddRemoteScheme<LiffAuthenticationOptions, LiffAuthenticationHandler>(authenticationScheme, null, null);

            // Return
            return builder;
        }
    }
}
