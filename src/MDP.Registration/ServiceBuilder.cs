using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    public interface ServiceBuilder
    {
        // Methods
        void Add(ServiceRegistration serviceRegistration);
    }
}
