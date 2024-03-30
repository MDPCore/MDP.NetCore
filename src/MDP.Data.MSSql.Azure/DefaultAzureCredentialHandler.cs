using Azure.Identity;
using MDP.Registration;
using System.Collections.Generic;

namespace MDP.Data.MSSql.Azure
{
    [Service<SqlClientHandler>(singleton: true)]
    public class DefaultAzureCredentialHandler : AzureCredentialHandler
    {
        // Constructors
        public DefaultAzureCredentialHandler(List<string> scopes) : base(new MDP.DevKit.AzureIdentity.DefaultAzureCredential(), scopes) { }
    }
}
