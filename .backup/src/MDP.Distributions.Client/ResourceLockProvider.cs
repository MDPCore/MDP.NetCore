using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions.Client
{
    public interface ResourceLockProvider
    {
        // Methods
        ResourceLock? LockResource(string resourceId, string lockerId, uint expiredSecond);

        void UnlockResource(string resourceId, string lockerId);
    }
}
