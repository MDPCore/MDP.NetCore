using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions.Client
{
    public class DistributeContext
    {
        // Fields
        private readonly ResourceLockProvider _resourceLockProvider;


        // Constructors
        public DistributeContext
        (
            ResourceLockProvider resourceLockProvider
        )
        {
            #region Contracts

            if (resourceLockProvider == null) throw new ArgumentException($"{nameof(resourceLockProvider)}=null");
           
            #endregion

            // Default
            _resourceLockProvider = resourceLockProvider;
        }


        // Methods
        public ResourceLocker Lock(string resourceId, string? lockerId = null, uint expiredSecond = 30)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");

            #endregion

            // Retry
            while (true)
            {
                // TryLock
                var resourceLocker = this.TryLock(resourceId, lockerId, expiredSecond);
                if (resourceLocker == null) continue;

                // IsAcquired
                if (resourceLocker.IsAcquired == false)
                {
                    using (resourceLocker)
                    {
                        continue;
                    }
                }

                // Return
                return resourceLocker;
            }
        }

        public ResourceLocker TryLock(string resourceId, string? lockerId = null, uint expiredSecond = 30)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");

            #endregion

            // LockerId
            if (string.IsNullOrEmpty(lockerId) == true)
            {
                lockerId = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(lockerId) == true) throw new InvalidOperationException($"{nameof(lockerId)}=null");

            // ResourceLock
            var resourceLock = _resourceLockProvider.LockResource(resourceId, lockerId, expiredSecond);

            // ResourceLocker
            if (resourceLock == null)
            {
                // Return
                return new ResourceLocker(resourceId, lockerId);
            }
            else
            {
                // Return
                return new ResourceLocker(resourceLock, _resourceLockProvider);
            }
        }
    }
}
