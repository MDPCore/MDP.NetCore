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
            using (var httpClient = httpClientFactory.CreateClient("DefaultService"))
            {
                // RequestMessage
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "get");

                // ResponseMessage
                var responseMessage = await httpClient.SendAsync(requestMessage);
                if (responseMessage.IsSuccessStatusCode == false)
                {
                    var errorContent = await responseMessage.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(errorContent) == false) throw new HttpRequestException(errorContent);
                    if (string.IsNullOrEmpty(errorContent) == true) throw new HttpRequestException($"An unexpected error occurred(responseMessage.StatusCode={responseMessage.StatusCode}).");
                }

                // ResponseContentString
                var responseContentString = await responseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContentString) == true) throw new InvalidOperationException($"{nameof(responseContentString)}=null");

                // Display
                Console.WriteLine(responseContentString);
            }                
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
