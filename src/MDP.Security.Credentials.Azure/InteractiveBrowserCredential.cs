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
    public class InteractiveBrowserCredential : AzureLib.Identity.InteractiveBrowserCredential
    {
        // Constructors
        public InteractiveBrowserCredential(string tenantId) : base(new AzureLib.Identity.InteractiveBrowserCredentialOptions() { TenantId = tenantId }) { }
    }
}
