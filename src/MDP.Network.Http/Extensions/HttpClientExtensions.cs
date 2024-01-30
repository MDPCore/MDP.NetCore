using MDP.Network.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using static MDP.Network.Http.HttpClientFactoryFactory;

namespace System.Net.Http
{
    public static partial class HttpClientExtensions
    {
        // Constants
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


        // Methods
        private static async Task<TResultModel> SendAsync<TResultModel, TErrorModel>(this HttpClient httpClient, HttpMethod httpMethod, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null, Func<JsonElement, TErrorModel> errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            #region Contracts

            if (httpClient == null) throw new ArgumentException($"{nameof(httpClient)}=null");
            if (httpMethod == null) throw new ArgumentException($"{nameof(httpMethod)}=null");

            #endregion

            // RequestMessage
            var requestMessage = new HttpRequestMessage(httpMethod, CreateRequestUri(httpClient.BaseAddress?.ToString(), requestUri, query));
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
                requestMessage.Content = CreateRequestContent(content);
            }

            // ResponseMessage
            var responseMessage = await httpClient.SendAsync(requestMessage);
            {
                // IsSuccess
                if (responseMessage.IsSuccessStatusCode == true)
                {
                    // ResultModel
                    var resultModel = await CreateResponseModel(responseMessage, resultFactory);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
                else
                {
                    // ErrorModel
                    var errorModel = await CreateResponseModel(responseMessage, errorFactory);
                    if (errorModel == null) responseMessage.EnsureSuccessStatusCode();

                    // Throw
                    throw new HttpException<TErrorModel>(responseMessage.StatusCode, $"An unexpected error occurred(statusCode={(int)(responseMessage.StatusCode)})", errorModel);
                }
            }
        }

        private static string CreateRequestUri(string baseAddress = null, string requestUri = null, object query = null)
        {
            // Default
            if (string.IsNullOrEmpty(requestUri) == true) requestUri = string.Empty;
            if (requestUri.StartsWith("/") == true) requestUri = requestUri.Substring(1, requestUri.Length - 1);

            // QueryString
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

            // BaseAddress
            if (string.IsNullOrEmpty(baseAddress) == false)
            {
                // RelativePath
                if (Uri.TryCreate(requestUri, UriKind.Relative, out _) == false) throw new InvalidOperationException($"The requestUri must be a relative path.{nameof(requestUri)}={requestUri}");
            }
            else
            {
                // AbsolutePath
                if (Uri.TryCreate(requestUri, UriKind.Absolute, out _) == false) throw new InvalidOperationException($"The requestUri must be a absolute path.{nameof(requestUri)}={requestUri}");
            }

            // Return
            return requestUri;
        }

        private static HttpContent CreateRequestContent(object content = null)
        {
            // RequestContent by Null
            if (content == null) return null;

            // RequestContent by String
            if (content.GetType() == typeof(string))
            {
                // RequestContentString
                var requestContentString = content as string;
                if (string.IsNullOrEmpty(requestContentString) == true) requestContentString = string.Empty;

                // Return
                return new StringContent(requestContentString, Encoding.UTF8, "application/x-www-form-urlencoded");
            }

            // RequestContent by Dictionary<string, string>
            if (content.GetType() == typeof(Dictionary<string, string>))
            {
                // RequestContentDictionary
                var requestContentDictionary = content as Dictionary<string, string>;
                if (requestContentDictionary == null) throw new InvalidOperationException($"{nameof(requestContentDictionary)}=null");

                // RequestContentString
                var requestContentString = new StringBuilder();
                foreach (var requestContentPair in requestContentDictionary)
                {
                    // And
                    if (requestContentString.Length > 0)
                    {
                        requestContentString.Append('&');
                    }

                    // RequestContentPair
                    requestContentString.Append(WebUtility.UrlEncode(requestContentPair.Key));
                    requestContentString.Append('=');
                    requestContentString.Append(WebUtility.UrlEncode(requestContentPair.Value));
                }

                // Return
                return new StringContent(requestContentString.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            }

            // RequestContent by Object
            {
                // RequestContentString
                var requestContentString = System.Text.Json.JsonSerializer.Serialize(content, _serializerOptions);
                if (string.IsNullOrEmpty(requestContentString) == true) throw new InvalidOperationException($"{nameof(requestContentString)}=null");

                // Return
                return new StringContent(requestContentString, Encoding.UTF8, "application/json");
            }
        }

        private static async Task<TResponseModel> CreateResponseModel<TResponseModel>(HttpResponseMessage responseMessage, Func<JsonElement, TResponseModel> responseModelFactory = null)
               where TResponseModel : class
        {
            #region Contracts

            if (responseMessage == null) throw new ArgumentException($"{nameof(responseMessage)}=null");

            #endregion

            // ResponseModelString
            var responseModelString = await responseMessage.Content.ReadAsStringAsync();

            // ResponseModel by Factory
            if (responseModelFactory != null)
            {
                // Require
                if (string.IsNullOrEmpty(responseModelString) == true) responseModelString = "{}";

                // Document
                var responseModelDocument = JsonDocument.Parse(responseModelString);
                if (responseModelDocument == null) throw new InvalidOperationException($"{nameof(responseModelDocument)}=null");

                // Factory
                using (responseModelDocument)
                {
                    // ResponseModel
                    var responseModel = responseModelFactory.Invoke(responseModelDocument.RootElement);

                    // Return
                    return responseModel;
                }
            }

            // ResponseModel by String
            if (typeof(TResponseModel) == typeof(string))
            {
                // Require
                if (string.IsNullOrEmpty(responseModelString) == true) responseModelString = string.Empty;

                // ResponseModel
                var responseModel = responseModelString as TResponseModel;

                // Return
                return responseModel;
            }

            // ResponseModel by Deserialize
            {
                // Require
                if (string.IsNullOrEmpty(responseModelString) == true) responseModelString = "{}";

                // ResponseModel
                var responseModel = System.Text.Json.JsonSerializer.Deserialize<TResponseModel>(responseModelString, _serializerOptions);

                // Return
                return responseModel;
            }
        }
    }

    public static partial class HttpClientExtensions
    {
        // Methods
        public static Task<TResultModel> GetAsync<TResultModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null)
            where TResultModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, dynamic>(HttpMethod.Get, requestUri, headers, query, content, resultFactory);
        }

        public static Task<TResultModel> GetAsync<TResultModel, TErrorModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null, Func<JsonElement, TErrorModel> errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, TErrorModel>(HttpMethod.Get, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }

    public static partial class HttpClientExtensions
    {
        // Methods
        public static Task<TResultModel> PostAsync<TResultModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null)
            where TResultModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, dynamic>(HttpMethod.Post, requestUri, headers, query, content, resultFactory);
        }

        public static Task<TResultModel> PostAsync<TResultModel, TErrorModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null, Func<JsonElement, TErrorModel> errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, TErrorModel>(HttpMethod.Post, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }

    public static partial class HttpClientExtensions
    {
        // Methods
        public static Task<TResultModel> PutAsync<TResultModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null)
            where TResultModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, dynamic>(HttpMethod.Put, requestUri, headers, query, content, resultFactory);
        }

        public static Task<TResultModel> PutAsync<TResultModel, TErrorModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null, Func<JsonElement, TErrorModel> errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, TErrorModel>(HttpMethod.Put, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }

    public static partial class HttpClientExtensions
    {
        // Methods
        public static Task<TResultModel> DeleteAsync<TResultModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null)
            where TResultModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, dynamic>(HttpMethod.Delete, requestUri, headers, query, content, resultFactory);
        }

        public static Task<TResultModel> DeleteAsync<TResultModel, TErrorModel>(this HttpClient httpClient, string requestUri = null, object headers = null, object query = null, object content = null, Func<JsonElement, TResultModel> resultFactory = null, Func<JsonElement, TErrorModel> errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return httpClient.SendAsync<TResultModel, TErrorModel>(HttpMethod.Delete, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }
}
