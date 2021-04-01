using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP
{
    public interface IConfiguration<TService> : IServiceConfiguration
        where TService : class
    {

    }
}
