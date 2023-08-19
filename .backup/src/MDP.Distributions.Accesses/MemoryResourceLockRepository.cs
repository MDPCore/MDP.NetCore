using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions.Accesses
{
    public class MemoryResourceLockRepository : ResourceLockRepository
    {
        // Fields
        private readonly object _syncRoot = new object();

        private readonly Dictionary<string, ResourceLock> _resourceLockDictionary = new Dictionary<string, ResourceLock>();


        // Methods
        public bool Add(ResourceLock resourceLock)
        {
            #region Contracts

            if (resourceLock == null) throw new ArgumentException($"{nameof(resourceLock)}=null");

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_resourceLockDictionary.ContainsKey(resourceLock.ResourceId) == true) return false;

                // Add
                _resourceLockDictionary.Add(resourceLock.ResourceId, resourceLock);

                // Return
                return true;
            }
        }

        public void RemoveByLockerId(string resourceId, string lockerId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");
            if (string.IsNullOrEmpty(lockerId) == true) throw new ArgumentException($"{nameof(lockerId)}=null");

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_resourceLockDictionary.ContainsKey(resourceId) == false) return;

                // ResourceLock
                var resourceLock = _resourceLockDictionary[resourceId];
                if (resourceLock == null) throw new InvalidOperationException($"{nameof(resourceLock)}=null");

                // LockerId
                if (resourceLock.LockerId == lockerId)
                {
                    // Remove
                    _resourceLockDictionary.Remove(resourceId);
                }
            }
        }

        public void RemoveByExpiredTime(string resourceId, DateTime nowTime)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_resourceLockDictionary.ContainsKey(resourceId) == false) return;

                // ResourceLock
                var resourceLock = _resourceLockDictionary[resourceId];
                if (resourceLock == null) throw new InvalidOperationException($"{nameof(resourceLock)}=null");

                // ExpiredTime
                if (resourceLock.ExpiredTime <= nowTime)
                {
                    // Remove
                    _resourceLockDictionary.Remove(resourceId);
                }
            }
        }

        public ResourceLock? FindByResourceId(string resourceId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");

            #endregion

            // Sync
            lock (_syncRoot)
            {
                // Require
                if (_resourceLockDictionary.ContainsKey(resourceId) == false) return null;

                // ResourceLock
                var resourceLock = _resourceLockDictionary[resourceId];
                if (resourceLock == null) throw new InvalidOperationException($"{nameof(resourceLock)}=null");

                // Return
                return resourceLock;
            }
        }
    }
}
