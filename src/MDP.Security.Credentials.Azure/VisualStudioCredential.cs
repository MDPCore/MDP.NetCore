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
    public class VisualStudioCredential : AzureLib.Identity.VisualStudioCredential
    {
        // Constructors
        public VisualStudioCredential(string tenantId) : base(new AzureLib.Identity.VisualStudioCredentialOptions() { TenantId = tenantId }) { }
    }
}
