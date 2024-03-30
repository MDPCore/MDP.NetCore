using Azure.Identity;
using MDP.Registration;
using System.Collections.Generic;
using System.Net.Http;

namespace MDP.Network.Http.Azure
{
    [Service<DelegatingHandler>(singleton: true)]
    public class DefaultAzureCredentialHandler : AzureCredentialHandler
    {
        // Constructors
        public DefaultAzureCredentialHandler(List<string> scopes) : base(new MDP.DevKit.AzureIdentity.DefaultAzureCredential(), scopes) { }
    }
}
