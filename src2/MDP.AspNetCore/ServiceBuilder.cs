using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    public abstract class ServiceBuilder : MDP.Hosting.ServiceBuilder<WebApplicationBuilder>
    {

    }

    public abstract class ServiceBuilder<TSetting> : MDP.Hosting.ServiceBuilder<WebApplicationBuilder, TSetting>
        where TSetting : class, new()
    {

    }
}
