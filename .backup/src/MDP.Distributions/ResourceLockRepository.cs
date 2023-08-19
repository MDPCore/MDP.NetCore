using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions
{
    public interface ResourceLockRepository
    {
        // Methods
        bool Add(ResourceLock resourceLock);

        void RemoveByLockerId(string resourceId, string lockerId);

        void RemoveByExpiredTime(string resourceId, DateTime nowTime);

        ResourceLock? FindByResourceId(string resourceId);
    }
}
