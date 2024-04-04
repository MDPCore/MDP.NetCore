using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureLib = Azure;

namespace MDP.Security.Credentials.Azure
{
    [Service<AzureLib.Core.TokenCredential>(singleton: true, autoRegister:true)]
    public class DefaultAzureCredential : AzureLib.Identity.ChainedTokenCredential
    {
        // Constructors
        public DefaultAzureCredential() : base(new AzureLib.Identity.AzureCliCredential(), new AzureLib.Identity.DefaultAzureCredential()) { }
    }
}
