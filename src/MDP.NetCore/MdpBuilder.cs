using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    public class MdpBuilder
    {
        // Constructors
        public MdpBuilder(IHostBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException(nameof(hostBuilder));

            #endregion

            // Default
            this.HostBuilder = hostBuilder;
        }


        // Properties
        public IHostBuilder HostBuilder { get; private set; }
    }
}
