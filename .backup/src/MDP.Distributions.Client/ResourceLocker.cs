using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Distributions.Client
{
    public class ResourceLocker : IDisposable
    {
        // Fields
        private readonly ResourceLockProvider? _resourceLockProvider = null;


        // Constructors
        internal ResourceLocker(string resourceId, string lockerId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(resourceId) == true) throw new ArgumentException($"{nameof(resourceId)}=null");
            if (string.IsNullOrEmpty(lockerId) == true) throw new ArgumentException($"{nameof(lockerId)}=null");

            #endregion

            // Default
            this.ResourceId = resourceId;
            this.LockerId = lockerId;
            this.IsAcquired = false;
            this.ExpiredTime = DateTime.Now;
            this.CreatedTime = DateTime.Now;

            // ResourceLockProvider
            _resourceLockProvider = null;
        }

        internal ResourceLocker(ResourceLock resourceLock, ResourceLockProvider resourceLockProvider)
        {
            #region Contracts

            if (resourceLock == null) throw new ArgumentException($"{nameof(resourceLock)}=null");
            if (resourceLockProvider == null) throw new ArgumentException($"{nameof(resourceLockProvider)}=null");

            #endregion

            // Default
            this.ResourceId = resourceLock.ResourceId;
            this.LockerId = resourceLock.LockerId;
            this.IsAcquired = true;
            this.ExpiredTime = resourceLock.ExpiredTime;
            this.CreatedTime = resourceLock.CreatedTime;

            // ResourceLockProvider
            _resourceLockProvider = resourceLockProvider;
        }

        public void Dispose()
        {
            // Require
            if (_resourceLockProvider == null) return;

            // Unlock
            try
            {
                _resourceLockProvider.UnlockResource(this.ResourceId, this.LockerId);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }


        // Properties
        public string ResourceId { get; private set; } = string.Empty;

        public string LockerId { get; private set; } = string.Empty;

        public bool IsAcquired { get; private set; } = false;

        public DateTime ExpiredTime { get; private set; } = DateTime.Now;

        public DateTime CreatedTime { get; private set; } = DateTime.Now;
    }
}
