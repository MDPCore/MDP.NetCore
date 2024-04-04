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
    public class VisualStudioCodeCredential : AzureLib.Identity.VisualStudioCodeCredential
    {
        // Constructors
        public VisualStudioCodeCredential(string tenantId) : base(new AzureLib.Identity.VisualStudioCodeCredentialOptions() { TenantId = tenantId }) { }
    }
}
