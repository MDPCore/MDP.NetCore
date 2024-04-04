using Azure.Identity;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AzureLib = Azure;
using Microsoft.Graph;

namespace MDP.Security.Credentials.Azure.Lab
{
    public class Program
    {
        // Methods
        public static async Task Run(AzureLib.Core.TokenCredential azureCredential)
        {
            #region Contracts

            if (azureCredential == null) throw new ArgumentException($"{nameof(azureCredential)}=null");

            #endregion

            // GraphClient
            using (var graphClient = new GraphServiceClient(azureCredential, new[] { "https://graph.microsoft.com/.default" }))
            {
                // User
                var user = await graphClient.Me.GetAsync();
                if (user == null) throw new InvalidOperationException($"{nameof(user)}=null");

                // Display
                Console.WriteLine($"UserId: {user.Id}");
                Console.WriteLine($"UserName: {user.DisplayName}");
            }
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
