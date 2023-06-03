using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
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
        public Task<TResponseContent?> GetAsync<TResponseContent>(string? requestUri = null, object? headers = null, object? query = null) where TResponseContent : class
        {
            // Return
            return this.SendAsync<TResponseContent>(HttpMethod.Get, requestUri, headers, query, null);
        }

        public Task<TResponseContent?> PostAsync<TResponseContent>(string? requestUri = null, object? headers = null, object? content = null) where TResponseContent : class
        {
            // Return
            return this.SendAsync<TResponseContent>(HttpMethod.Post, requestUri, headers, null, content);
        }

        private async Task<TResponseContent?> SendAsync<TResponseContent>(HttpMethod httpMethod, string? requestUri = null, object? headers = null, object? query = null, object? content = null) where TResponseContent : class
        {
            // RequestUri
            if (string.IsNullOrEmpty(requestUri) == true) requestUri = string.Empty;
            if (requestUri.StartsWith("/") == true) requestUri = requestUri.Substring(1, requestUri.Length - 1);
            if (query != null)
            {
                // RequestQuery
                var requestQuery = HttpUtility.ParseQueryString(new UriBuilder(requestUri).Query);
                query.GetType().GetProperties().ToList().ForEach(property =>
                {
                    // PropertyName
                    var propertyName = property.Name;
                    if (string.IsNullOrEmpty(propertyName) == true) throw new InvalidOperationException($"{nameof(propertyName)}=null");

                    // PropertyValue
                    var propertyValue = property.GetValue(query)?.ToString();

                    // Add
                    requestQuery.Add(propertyName, propertyValue);
                });

                // RequestUri
                var markIndex = requestUri.IndexOf('?');
                if (markIndex >= 0) requestUri = requestUri.Remove(markIndex);
                if (requestQuery.Count > 0) requestUri += $"?{requestQuery.ToString()}";
            }
            if (string.IsNullOrEmpty(_httpClient.BaseAddress?.ToString()) == false)
            {
                // RelativePath
                if (Uri.TryCreate(requestUri, UriKind.Relative, out _) == false) throw new InvalidOperationException($"The requestUri must be a relative path.{nameof(requestUri)}={requestUri}");
            }
            else
            {
                // AbsolutePath
                if (Uri.TryCreate(requestUri, UriKind.Absolute, out _) == false) throw new InvalidOperationException($"The requestUri must be a absolute path.{nameof(requestUri)}={requestUri}");
            }

            // RequestMessage
            var requestMessage = new HttpRequestMessage(httpMethod, requestUri);
            {
                // RequestHeaders
                if (headers != null)
                {
                    headers.GetType().GetProperties().ToList().ForEach(property =>
                    {
                        // PropertyName
                        var propertyName = property.Name;
                        if (string.IsNullOrEmpty(propertyName) == true) throw new InvalidOperationException($"{nameof(propertyName)}=null");

                        // PropertyValue
                        var propertyValue = property.GetValue(headers)?.ToString();

                        // Add
                        requestMessage.Headers.Add(propertyName, propertyValue);
                    });
                }

                // RequestContent
                if (content != null)
                {
                    // RequestContentString
                    var requestContentString = System.Text.Json.JsonSerializer.Serialize(content);
                    if (string.IsNullOrEmpty(requestContentString) == true) throw new InvalidOperationException($"{nameof(requestContentString)}=null");
                    
                    // Set
                    requestMessage.Content = new StringContent(requestContentString, Encoding.UTF8, "application/json");
                }
            }

            // ResponseMessage
            var responseMessage = await _httpClient.SendAsync(requestMessage);
            {
                // ErrorContent
                if (responseMessage.IsSuccessStatusCode == false)
                {
                    var errorContent = await responseMessage.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorContent) == false) throw new HttpRequestException(errorContent);
                    if (string.IsNullOrEmpty(errorContent) == true) throw new HttpRequestException($"An unexpected error occurred(responseMessage.StatusCode={responseMessage.StatusCode}).");
                }

                // ResponseContentString
                var responseContentString = await responseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContentString) == true) return null;

                // ResponseContent
                var responseContent = System.Text.Json.JsonSerializer.Deserialize<TResponseContent>(responseContentString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (responseContent == null) throw new InvalidOperationException($"{nameof(responseContent)}={responseContentString}");

                // Return
                return responseContent;
            }
        }
    }
}
