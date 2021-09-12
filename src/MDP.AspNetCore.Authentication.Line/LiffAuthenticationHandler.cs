using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MDP.AspNetCore.Authentication.Line
{
    public class LiffAuthenticationHandler : RemoteAuthenticationHandler<LiffAuthenticationOptions>
    {
        // Constructors
        public LiffAuthenticationHandler(IOptionsMonitor<LiffAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) { }


        // Methods
        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            try
            {
                // ReturnUrl
                var returnUrl = this.Request.Query["returnUrl"];
                if (string.IsNullOrEmpty(returnUrl) == true) returnUrl = @"/";

                // AccessToken
                var accessToken = this.Request.Query["access_token"];
                if (string.IsNullOrEmpty(accessToken) == true) return HandleRequestResult.Fail("AccessToken was not found.");
                this.VerifyAccessToken(accessToken);

                // UserInfo
                var userInfo = await this.GetUserInfo(accessToken);
                if (userInfo == null) throw new InvalidOperationException($"{nameof(userInfo)}=null");

                // Identity
                var identity = new ClaimsIdentity(this.Scheme.Name);
                foreach (var action in this.Options.ClaimActions)
                {
                    action.Run(userInfo.RootElement, identity, this.Scheme.Name);
                }

                // RedirectUri
                var redirectUri = (new PathString("/Account/ExternalSignIn")).Add(QueryString.Create(new Dictionary<string, string>()
                {
                    { "returnUrl", returnUrl }
                }));
                if (string.IsNullOrEmpty(redirectUri) == true) throw new InvalidOperationException($"{nameof(redirectUri)}=null");
                               
                // Success
                return HandleRequestResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), new AuthenticationProperties() { RedirectUri = redirectUri }, this.Scheme.Name));
            }
            catch(Exception ex)
            {
                // Fail
                return HandleRequestResult.Fail(ex.Message);
            }
        }

        private async void VerifyAccessToken(string accessToken)
        {
            #region Contracts

            if (string.IsNullOrEmpty(accessToken) == true) throw new ArgumentException(nameof(accessToken));

            #endregion

            // Request
            var request = new HttpRequestMessage(HttpMethod.Get, this.Options.VerifyEndpoint + "?access_token=" + accessToken);

            // Response
            var response = await this.Options.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // VerifyResult
            var verifyResult = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (verifyResult == null) throw new InvalidOperationException($"{nameof(verifyResult)}=null");

            // ClientId
            var clientId = verifyResult.RootElement.GetProperty("client_id").ToString();
            if (string.IsNullOrEmpty(clientId) == true) throw new InvalidOperationException($"{nameof(clientId)} is emptied.");
            if (clientId != this.Options.ClientId) throw new InvalidOperationException($"{nameof(clientId)} is failed.");

            // ExpiresIn
            var expiresIn = verifyResult.RootElement.GetProperty("expires_in").ToString();
            if (string.IsNullOrEmpty(expiresIn) == true) throw new InvalidOperationException($"{nameof(expiresIn)} is emptied.");
            if (int.Parse(expiresIn) <= 0) throw new InvalidOperationException($"{nameof(expiresIn)} is failed.");
        }

        private async Task<JsonDocument> GetUserInfo(string accessToken)
        {
            #region Contracts

            if (string.IsNullOrEmpty(accessToken) == true) throw new ArgumentException(nameof(accessToken));

            #endregion

            // Request
            var request = new HttpRequestMessage(HttpMethod.Get, this.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Response
            var response = await this.Options.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            // UserInfo
            var userInfo = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (userInfo == null) throw new InvalidOperationException($"{nameof(userInfo)}=null");

            // Return
            return userInfo;
        }
    }
}
