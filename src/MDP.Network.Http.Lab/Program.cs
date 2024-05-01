using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MDP.Network.Http.Lab
{
    public class Program
    {
        // Methods
        public static async Task Run(IHttpClientFactory httpClientFactory)
        {
            #region Contracts

            if (httpClientFactory == null) throw new ArgumentException($"{nameof(httpClientFactory)}=null");

            #endregion

            // HttpClient
            var httpClient = httpClientFactory.CreateClient("DefaultService");
            if (httpClient == null) throw new InvalidOperationException($"{nameof(httpClient)}=null");

            // GetAsync
            var resultModel = await httpClient.GetAsync<string>("get");
            if (string.IsNullOrEmpty(resultModel) == true) throw new InvalidOperationException($"{nameof(resultModel)}=null");

            // Display
            Console.WriteLine(resultModel);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
