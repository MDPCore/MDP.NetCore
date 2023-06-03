using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace MDP.Network.Http.Lab
{
    public class Program
    {
        // Methods
        public static void Run(IHttpClientFactory httpClientFactory)
        {
            #region Contracts

            if (httpClientFactory == null) throw new ArgumentException($"{nameof(httpClientFactory)}=null");

            #endregion

            // HttpClient
            using (var httpClient = httpClientFactory.CreateClient("DefaultService"))
            {
                // RequestMessage
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "");

                // ResponseMessage
                var responseMessage = httpClient.SendAsync(requestMessage).Result;
                if (responseMessage.IsSuccessStatusCode == false)
                {
                    var content = responseMessage.Content.ReadAsStringAsync().Result;
                    if (string.IsNullOrEmpty(content) == false) throw new HttpRequestException(content);
                    if (string.IsNullOrEmpty(content) == true) throw new HttpRequestException($"An unexpected error occurred(response.StatusCode={responseMessage.StatusCode}).");
                }

                // Payload
                var payload = responseMessage.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(payload) == true) throw new HttpRequestException($"An unexpected error occurred(response.Payload=null).");
                
                // Display
                Console.WriteLine(payload);
            }                
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}
