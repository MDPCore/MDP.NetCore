using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.AzureIdentity
{
    [Service<Azure.Core.TokenCredential>(singleton: true)]
    public class AzureCliCredential : Azure.Identity.AzureCliCredential
    {
        // Constructors
        public AzureCliCredential(string tenantId) : base(new Azure.Identity.AzureCliCredentialOptions() { TenantId = tenantId }) { }
    }
}
