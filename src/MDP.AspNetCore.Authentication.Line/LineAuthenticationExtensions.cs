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
    public static class LineAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddLine(this AuthenticationBuilder builder, Action<LineAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // Return
            return builder.AddLine(LineDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddLine(this AuthenticationBuilder builder, string authenticationScheme, Action<LineAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationOptions
            if (configureOptions != null) builder.Services.Configure(authenticationScheme, configureOptions);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<LineAuthenticationOptions>, LineAuthenticationPostConfigureOptions>());

            // OAuthAuthenticationOptions
            builder.Services.AddOptions<OAuthOptions>(authenticationScheme).Configure<IOptionsMonitor<LineAuthenticationOptions>>((oauthOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // OAuthOptions
                oauthOptions.ClientId = authenticationOptions.ClientId;
                oauthOptions.ClientSecret = authenticationOptions.ClientSecret;
                oauthOptions.SignInScheme = authenticationOptions.SignInScheme;

                // Scope
                oauthOptions.Scope.Add("profile");
                oauthOptions.Scope.Add("openid");
                oauthOptions.Scope.Add("email");

                // Claim
                oauthOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "userId");
                oauthOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
                oauthOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                // Endpoint
                oauthOptions.CallbackPath = new PathString("/signin-line");
                oauthOptions.AuthorizationEndpoint = "https://access.line.me/oauth2/v2.1/authorize";
                oauthOptions.TokenEndpoint = "https://api.line.me/oauth2/v2.1/token";
                oauthOptions.UserInformationEndpoint = "https://api.line.me/v2/profile";

                // Events
                oauthOptions.Events = new OAuthEvents
                {
                    // Ticket
                    OnCreatingTicket = async context =>
                    {
                        // Request
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                        // Response
                        var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();

                        // UserInfo
                        var userInfo = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                        if (userInfo == null) throw new InvalidOperationException($"{nameof(userInfo)}=null");
                        context.RunClaimActions(userInfo.RootElement);

                        // Email
                        var idToken = context.TokenResponse?.Response?.RootElement.GetProperty("id_token").ToString();
                        var securityToken = idToken != null ? new JwtSecurityToken(idToken) : null;
                        var emailClaim = securityToken?.Claims.FirstOrDefault(o => o.Type == "email");                        
                        if (emailClaim != null) context.Identity.AddClaim(new Claim(ClaimTypes.Email, emailClaim.Value));
                    }
                };
            });

            // OAuthAuthentication
            builder.AddOAuth(authenticationScheme, null, null);

            // Return
            return builder;
        }
    }
}
