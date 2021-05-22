using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public interface IServiceConfiguration
    {
        // Properties
        public IConfiguration RootSection { get; }

        public IConfiguration ServiceSection { get; }
    }
}
