using System.Net.Http;
using MDP.Network.Http;

namespace MDP.Network.Rest
{
    public class RestClientFactory
    {
        // Fields
        private readonly IHttpClientFactory _httpClientFactory;


        // Constructors
        public RestClientFactory(IHttpClientFactory httpClientFactory)
        {
            #region Contracts

            if (httpClientFactory == null) throw new ArgumentException($"{nameof(httpClientFactory)}=null");

            #endregion

            // Default
            _httpClientFactory = httpClientFactory;
        }


        // Methods
        public RestClient CreateClient()
        {
            // HttpClient
            var httpClient = _httpClientFactory.CreateClient();
            if (httpClient == null) throw new InvalidOperationException ($"{nameof(httpClient)}=null");

            // Return
            return new RestClient(httpClient);
        }

        public RestClient CreateClient(string name)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name)==true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // HttpClient
            var httpClient = _httpClientFactory.CreateClient(name);
            if (httpClient == null) throw new InvalidOperationException($"{nameof(httpClient)}=null");
            
            // Return
            return new RestClient(httpClient);
        }

        public RestClient CreateClient(string @namespace, string name)
        {
            #region Contracts

            if (string.IsNullOrEmpty(@namespace) == true) throw new ArgumentException($"{nameof(@namespace)}=null");
            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // HttpClient
            var httpClient = _httpClientFactory.CreateClient(@namespace, name);
            if (httpClient == null) throw new InvalidOperationException($"{nameof(httpClient)}=null");

            // Return
            return new RestClient(httpClient);
        }
    }
}
