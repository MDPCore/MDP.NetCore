using System.Text.Json;

namespace MDP.Network.Rest.Lab
{
    public class Program
    {
        // Methods
        public static async Task Run(RestClientFactory restClientFactory)
        {
            #region Contracts

            if (restClientFactory == null) throw new ArgumentException($"{nameof(restClientFactory)}=null");

            #endregion

            // RestClient
            using (var restClient = restClientFactory.CreateClient("DefaultService"))
            {
                // ResponseContent
                var responseContent = await restClient.PostAsync<object>("post?ccc=789", content: new
                {
                    aaa = 123,
                    bbb = 456,
                });
                if ((responseContent) == null) throw new InvalidOperationException($"{nameof(responseContent)}=null");

                // RequestContentString
                var requestContentString = System.Text.Json.JsonSerializer.Serialize(responseContent, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                if (string.IsNullOrEmpty(requestContentString) == true) throw new InvalidOperationException($"{nameof(requestContentString)}=null");

                // Display
                Console.WriteLine(requestContentString);
            }                
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
