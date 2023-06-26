using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Network.Http
{
    public static class HttpClientFactoryExtensions
    {
        // Methods
        public static HttpClient CreateClient(this IHttpClientFactory httpClientFactory, string @namespace, string name)
        {
            #region Contracts

            if (httpClientFactory == null) throw new ArgumentException($"{nameof(httpClientFactory)}=null");
            if (string.IsNullOrEmpty(@namespace) == true) throw new ArgumentException($"{nameof(@namespace)}=null");
            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // ClientName
            var clientName = $"{@namespace}.{name}";
            if (string.IsNullOrEmpty(clientName) == true) throw new InvalidOperationException($"{nameof(clientName)}=null");

            // Create
            return httpClientFactory.CreateClient(clientName);
        }
    }
}
