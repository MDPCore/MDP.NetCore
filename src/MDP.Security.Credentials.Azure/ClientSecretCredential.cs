using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureLib = Azure;

namespace MDP.Security.Credentials.Azure
{
    [Service<AzureLib.Core.TokenCredential>(singleton: true, autoRegister: false)]
    public class ClientSecretCredential : AzureLib.Identity.ClientSecretCredential
    {
        // Constructors
        public ClientSecretCredential(string tenantId, string clientId, string clientSecret) : base(tenantId, clientId, clientSecret) { }
    }
}
