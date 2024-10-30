using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Registration;
using MDP.Network.Http.Azure;
using Azure.Core;

namespace MDP.Network.Http.Lab
{
    [Service<HttpClientHandler>(singleton: false, autoRegister: true)]
    public class GraphAzureCredentialHandler : AzureCredentialHandler
    {
        // Constructors
        public GraphAzureCredentialHandler(TokenCredential azureCredential) : base(azureCredential, new List<string>() { "https://graph.microsoft.com/.default" }) 
        {
        
        }
    }
}
