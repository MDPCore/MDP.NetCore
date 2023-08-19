using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions
{
    public class DistributeContext
    {
        // Fields
        private readonly ResourceLockRepository _resourceLockRepository;

        private readonly int _retryCount = 3;

        private readonly int _retryInterval = 100; // ms


        // Constructors
        public DistributeContext
        (
            ResourceLockRepository resourceLockRepository
        )
        {
            #region Contracts

            if (resourceLockRepository == null) throw new ArgumentException($"{nameof(resourceLockRepository)}=null");

            #endregion

            // Default
            _resourceLockRepository = resourceLockRepository;
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
            var resourceLock = this.LockResource(resourceId, lockerId, expiredSecond);

            // ResourceLocker
            if (resourceLock == null)
            {
                // Return
                return new ResourceLocker(resourceId, lockerId);
            }
            else
            {
                // Return
                return new ResourceLocker(resourceLock, _resourceLockRepository);
            }
        }


        public ResourceLock? LockResource(string resourceId, string lockerId, uint expiredSecond)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");
            if (string.IsNullOrEmpty(lockerId) == true) throw new ArgumentException($"{nameof(lockerId)}=null");

            #endregion

            // Execute
            for (var executeCount = 0; executeCount < _retryCount; executeCount++)
            {
                // Acquire
                var resourceLock = this.AcquireResourceLock(resourceId, lockerId, expiredSecond);
                if (resourceLock == null) continue;
                if (resourceLock.LockerId == lockerId) return resourceLock;

                // Wait
                if (executeCount < _retryCount - 1)
                {
                    Thread.Sleep(_retryInterval);
                }
            }

            // Return
            return null;
        }

        public void UnlockResource(string resourceId, string lockerId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");
            if (string.IsNullOrEmpty(lockerId) == true) throw new ArgumentException($"{nameof(lockerId)}=null");

            #endregion

            // Remove
            _resourceLockRepository.RemoveByLockerId(resourceId, lockerId);
        }

        private ResourceLock? AcquireResourceLock(string resourceId, string lockerId, uint expiredSecond)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");
            if (string.IsNullOrEmpty(lockerId) == true) throw new ArgumentException($"{nameof(lockerId)}=null");

            #endregion

            // Now
            var nowTime = DateTime.Now;

            // ResourceLock
            var resourceLock = new ResourceLock()
            {
                ResourceId = resourceId,
                LockerId = lockerId,
                ExpiredTime = nowTime.AddSeconds(expiredSecond),
                CreatedTime = nowTime,
            };

            // Acquire
            if (_resourceLockRepository.Add(resourceLock) == true)
            {
                // Return
                return resourceLock;
            }

            // Exist
            resourceLock = _resourceLockRepository.FindByResourceId(resourceId);
            if (resourceLock == null) return null;

            // Expired
            if (resourceLock.ExpiredTime <= nowTime)
            {
                // Remove
                _resourceLockRepository.RemoveByExpiredTime(resourceId, nowTime);

                // Return
                return null;
            }

            // Return
            return resourceLock;
        }
    }
}
