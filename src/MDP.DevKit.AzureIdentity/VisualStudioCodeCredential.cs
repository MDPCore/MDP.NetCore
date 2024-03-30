using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.AzureIdentity
{
    [Service<Azure.Core.TokenCredential>(singleton: true)]
    public class VisualStudioCodeCredential : Azure.Identity.VisualStudioCodeCredential
    {
        // Constructors
        public VisualStudioCodeCredential(string tenantId) : base(new Azure.Identity.VisualStudioCodeCredentialOptions() { TenantId = tenantId }) { }
    }
}
