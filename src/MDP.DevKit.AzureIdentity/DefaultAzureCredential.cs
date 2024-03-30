using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.AzureIdentity
{
    [Service<Azure.Core.TokenCredential>(singleton: true)]
    public class DefaultAzureCredential : Azure.Identity.ChainedTokenCredential
    {
        // Constructors
        public DefaultAzureCredential() : base(new Azure.Identity.AzureCliCredential(), new Azure.Identity.DefaultAzureCredential()) { }
    }
}
