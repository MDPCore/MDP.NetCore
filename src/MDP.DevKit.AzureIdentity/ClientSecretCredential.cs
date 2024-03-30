using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.AzureIdentity
{
    [Service<Azure.Core.TokenCredential>(singleton: true)]
    public class ClientSecretCredential : Azure.Identity.ClientSecretCredential
    {
        // Constructors
        public ClientSecretCredential(string tenantId, string clientId, string clientSecret) : base(tenantId, clientId, clientSecret) { }
    }
}
