using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.GitHub
{
    public static class GitHubAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, Action<GitHubAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // Return
            return builder.AddGitHub(GitHubDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string authenticationScheme, Action<GitHubAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationOptions
            if (configureOptions != null) builder.Services.Configure(authenticationScheme, configureOptions);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<GitHubAuthenticationOptions>, GitHubAuthenticationPostConfigureOptions>());

            // OAuthAuthenticationOptions
            builder.Services.AddOptions<OAuthOptions>(authenticationScheme).Configure<IOptionsMonitor<GitHubAuthenticationOptions>>((oauthOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // OAuthOptions
                oauthOptions.ClientId = authenticationOptions.ClientId;
                oauthOptions.ClientSecret = authenticationOptions.ClientSecret;
                oauthOptions.SignInScheme = authenticationOptions.SignInScheme;

                // Claim
                oauthOptions.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                oauthOptions.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                oauthOptions.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                // Endpoint
                oauthOptions.CallbackPath = new PathString("/signin-github");
                oauthOptions.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                oauthOptions.TokenEndpoint = "https://github.com/login/oauth/access_token";
                oauthOptions.UserInformationEndpoint = "https://api.github.com/user";

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
