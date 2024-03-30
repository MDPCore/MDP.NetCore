using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.AzureIdentity
{
    [Service<Azure.Core.TokenCredential>(singleton: true)]
    public class VisualStudioCredential : Azure.Identity.VisualStudioCredential
    {
        // Constructors
        public VisualStudioCredential(string tenantId) : base(new Azure.Identity.VisualStudioCredentialOptions() { TenantId = tenantId }) { }
    }
}
