using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MDP.Network.Rest
{
    public class RestClient : IDisposable
    {
        // Fields
        private readonly HttpClient _httpClient;


        // Constructors
        internal RestClient(HttpClient httpClient)
        {
            #region Contracts

            if (httpClient == null) throw new ArgumentException($"{nameof(httpClient)}=null");

            #endregion

            // Default
            _httpClient = httpClient;
        }

        public void Dispose()
        {
            // HttpClient
            _httpClient.Dispose();
        }


        // Methods
        public async Task<TResult?> GetAsync<TResult>(string? requestUri = null, Dictionary<string,string>? query = null) where TResult : class
        {
            // Require
            if (string.IsNullOrEmpty(requestUri) == true) requestUri = string.Empty;
            if (query == null) query = new Dictionary<string, string>();

            // RequestUriBuilder
            var requestUriBuilder = new UriBuilder(requestUri);
            {
                // RequestQuery
                var requestQuery = new NameValueCollection();
                foreach (var queryKey in query.Keys)
                {
                    requestQuery.Add(queryKey, query[queryKey]);
                }
                requestUriBuilder.Query = requestQuery.ToString();
            }

            // RequestUri
            requestUri = requestUriBuilder.ToString();
            if (string.IsNullOrEmpty(requestUri) == true) throw new InvalidOperationException($"{nameof(requestUri)}=null");

            // RequestMessage
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            // Return
            return await this.SendAsync<TResult>(requestMessage);
        }

        public async Task<TResult?> PostAsync<TResult>(string? requestUri = null, object? payload = null) where TResult : class
        {
            // Require
            if (string.IsNullOrEmpty(requestUri) == true) requestUri = string.Empty;
            if (payload == null) payload = new { };

            // RequestPayload
            var requestPayload = System.Text.Json.JsonSerializer.Serialize(payload);
            if (string.IsNullOrEmpty(requestPayload)==true) throw new InvalidOperationException($"{nameof(requestPayload)}=null");

            // RequestMessage
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            requestMessage.Content = new StringContent(requestPayload, Encoding.UTF8, "application/json");

            // Return
            return await this.SendAsync<TResult>(requestMessage);
        }

        private async Task<TResult?> SendAsync<TResult>(HttpRequestMessage requestMessage) where TResult : class
        {
            #region Contracts

            if (requestMessage == null) throw new ArgumentException($"{nameof(requestMessage)}=null");

            #endregion

            // ResponseMessage
            var responseMessage = await _httpClient.SendAsync(requestMessage);
            if (responseMessage.IsSuccessStatusCode == false)
            {
                var content = await responseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content) == false) throw new HttpRequestException(content);
                if (string.IsNullOrEmpty(content) == true) throw new HttpRequestException($"An unexpected error occurred(response.StatusCode={responseMessage.StatusCode}).");
            }

            // ResponsePayload
            var responsePayload = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responsePayload) == true) return null;

            // ResponseResult
            var responseResult = System.Text.Json.JsonSerializer.Deserialize<TResult>(responsePayload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (responseResult == null) throw new InvalidOperationException($"{nameof(responseResult)}={responsePayload}");

            // Return
            return responseResult;
        }
    }
}
