using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace MDP.Network.Rest
{
    public partial class RestClient : IDisposable
    {
        // Constants
        private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


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
        private TResultModel Send<TResultModel, TErrorModel>(HttpMethod httpMethod, string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.SendAsync<TResultModel, TErrorModel>(httpMethod, requestUri, headers, query, content, resultFactory, errorFactory).GetAwaiter().GetResult();
        }

        private async Task<TResultModel> SendAsync<TResultModel, TErrorModel>(HttpMethod httpMethod, string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
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
                // IsSuccess
                if (responseMessage.IsSuccessStatusCode == true)
                {
                    // ResultModel
                    var resultModel = await this.CreateResultModel(responseMessage, resultFactory);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
                else
                {
                    // ErrorModel
                    var errorModel = await this.CreateErrorModel(responseMessage, errorFactory);
                    if (errorModel == null) responseMessage.EnsureSuccessStatusCode();

                    // Throw
                    throw new RestException<TErrorModel>(responseMessage.StatusCode, $"An unexpected error occurred(statusCode={(int)(responseMessage.StatusCode)})", errorModel);
                }
            }
        }


        private async Task<TResultModel> CreateResultModel<TResultModel>(HttpResponseMessage responseMessage, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            #region Contracts

            if (responseMessage == null) throw new ArgumentException($"{nameof(responseMessage)}=null");

            #endregion

            // ResultModel
            TResultModel? resultModel = null;

            // ContentStream
            if (typeof(TResultModel) == typeof(Stream))
            {
                // Create
                using (var contentStream = await responseMessage.Content.ReadAsStreamAsync())
                {
                    // Stream
                    var memoryStream = new MemoryStream();
                    {
                        await contentStream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                    }
                    resultModel = memoryStream as TResultModel;
                }
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                // Return
                return resultModel;
            }

            // ContentString
            var contentString = await responseMessage.Content.ReadAsStringAsync();
            {
                // Default
                if (string.IsNullOrEmpty(contentString) == true) contentString = string.Empty;

                // Create
                if (resultFactory != null)
                {
                    // Default
                    if (string.IsNullOrEmpty(contentString) == true) contentString = "{}";

                    // ResultDocument
                    var resultDocument = JsonDocument.Parse(contentString);
                    if (resultDocument == null) throw new InvalidOperationException($"{nameof(resultDocument)}=null");

                    // ResultFactory
                    using (resultDocument) { resultModel = resultFactory.Invoke(resultDocument.RootElement); }
                }
                else if (typeof(TResultModel) == typeof(string))
                {
                    // Default
                    if (string.IsNullOrEmpty(contentString) == true) contentString = string.Empty;

                    // String
                    resultModel = contentString as TResultModel;
                }
                else
                {
                    // Default
                    if (string.IsNullOrEmpty(contentString) == true) contentString = "{}";

                    // Deserialize
                    resultModel = System.Text.Json.JsonSerializer.Deserialize<TResultModel>(contentString, _serializerOptions);
                }
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}={contentString}");

                // Return
                return resultModel;
            }
        }

        private async Task<TErrorModel?> CreateErrorModel<TErrorModel>(HttpResponseMessage responseMessage, Func<JsonElement, TErrorModel>? errorFactory = null)
               where TErrorModel : class
        {
            #region Contracts

            if (responseMessage == null) throw new ArgumentException($"{nameof(responseMessage)}=null");

            #endregion

            // ErrorModel
            TErrorModel? errorModel = null;

            // ErrorString
            var errorString = await responseMessage.Content.ReadAsStringAsync();
            {
                // Default
                if (string.IsNullOrEmpty(errorString) == true) return null;

                // Create
                if (errorFactory != null)
                {
                    // Default
                    if (string.IsNullOrEmpty(errorString) == true) errorString = "{}";

                    // ErrorDocument
                    var errorDocument = JsonDocument.Parse(errorString);
                    if (errorDocument == null) throw new InvalidOperationException($"{nameof(errorDocument)}=null");

                    // ErrorFactory
                    using (errorDocument) { errorModel = errorFactory.Invoke(errorDocument.RootElement); }
                }
                else if (typeof(TErrorModel) == typeof(string))
                {
                    // Default
                    if (string.IsNullOrEmpty(errorString) == true) errorString = string.Empty;

                    // String
                    errorModel = errorString as TErrorModel;
                }
                else
                {
                    // Default
                    if (string.IsNullOrEmpty(errorString) == true) errorString = "{}";

                    // Deserialize
                    errorModel = System.Text.Json.JsonSerializer.Deserialize<TErrorModel>(errorString, _serializerOptions);
                }

                // Return
                return errorModel;
            }
        }
    }

    public partial class RestClient : IDisposable
    {
        // Methods
        public TResultModel Get<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.Send<TResultModel, dynamic>(HttpMethod.Get, requestUri, headers, query, content, resultFactory);
        }

        public TResultModel Get<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.Send<TResultModel, TErrorModel>(HttpMethod.Get, requestUri, headers, query, content, resultFactory, errorFactory);
        }


        public Task<TResultModel> GetAsync<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.SendAsync<TResultModel, dynamic>(HttpMethod.Get, requestUri, headers, query, content, resultFactory);
        }

        public Task<TResultModel> GetAsync<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.SendAsync<TResultModel, TErrorModel>(HttpMethod.Get, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }

    public partial class RestClient : IDisposable
    {
        // Methods
        public TResultModel Post<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.Send<TResultModel, dynamic>(HttpMethod.Post, requestUri, headers, query, content, resultFactory);
        }

        public TResultModel Post<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.Send<TResultModel, TErrorModel>(HttpMethod.Post, requestUri, headers, query, content, resultFactory, errorFactory);
        }


        public Task<TResultModel> PostAsync<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.SendAsync<TResultModel, dynamic>(HttpMethod.Post, requestUri, headers, query, content, resultFactory);
        }

        public Task<TResultModel> PostAsync<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.SendAsync<TResultModel, TErrorModel>(HttpMethod.Post, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }

    public partial class RestClient : IDisposable
    {
        // Methods
        public TResultModel Put<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.Send<TResultModel, dynamic>(HttpMethod.Put, requestUri, headers, query, content, resultFactory);
        }

        public TResultModel Put<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.Send<TResultModel, TErrorModel>(HttpMethod.Put, requestUri, headers, query, content, resultFactory, errorFactory);
        }


        public Task<TResultModel> PutAsync<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.SendAsync<TResultModel, dynamic>(HttpMethod.Put, requestUri, headers, query, content, resultFactory);
        }

        public Task<TResultModel> PutAsync<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.SendAsync<TResultModel, TErrorModel>(HttpMethod.Put, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }

    public partial class RestClient : IDisposable
    {
        // Methods
        public TResultModel Delete<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.Send<TResultModel, dynamic>(HttpMethod.Delete, requestUri, headers, query, content, resultFactory);
        }

        public TResultModel Delete<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.Send<TResultModel, TErrorModel>(HttpMethod.Delete, requestUri, headers, query, content, resultFactory, errorFactory);
        }


        public Task<TResultModel> DeleteAsync<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Return
            return this.SendAsync<TResultModel, dynamic>(HttpMethod.Delete, requestUri, headers, query, content, resultFactory);
        }

        public Task<TResultModel> DeleteAsync<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null, Func<JsonElement, TErrorModel>? errorFactory = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Return
            return this.SendAsync<TResultModel, TErrorModel>(HttpMethod.Delete, requestUri, headers, query, content, resultFactory, errorFactory);
        }
    }
}