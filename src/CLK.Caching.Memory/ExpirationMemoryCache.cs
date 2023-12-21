using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Caching.Memory
{
    public class ExpirationMemoryCache
    {
        // Fields
        private readonly object _syncRoot = new object();

        private readonly int _expirationMinutes = 10;

        private readonly ExpirationType _expirationType = ExpirationType.UnifiedExpiration;

        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        private DateTime _expirationTime = DateTime.MinValue;


        // Constructors
        public ExpirationMemoryCache(int expirationMinutes = 10, ExpirationType expirationType = ExpirationType.UnifiedExpiration)
        {
            // Require
            if (expirationMinutes <= 0) throw new InvalidOperationException($"{nameof(expirationMinutes)}={expirationMinutes}");

            // Default
            _expirationMinutes = expirationMinutes;
            _expirationType = expirationType;
        }


        // Methods
        public void SetValue<TItem>(object key, TItem value = default(TItem))
        {
            #region Contracts

            if (key == null) throw new ArgumentException($"{nameof(key)}=null");

            #endregion

            // MemoryCache
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = this.CreateExpirationTime()
            });
        }

        public TItem? GetValue<TItem>(object key, Func<TItem?> getValueAction)
        {
            #region Contracts

            if (key == null) throw new ArgumentException($"{nameof(key)}=null");
            if (getValueAction == null) throw new ArgumentException($"{nameof(getValueAction)}=null");

            #endregion

            // Result
            TItem? value = default(TItem);

            // TryGetValue
            if (_memoryCache.TryGetValue(key, out value) == false)
            {
                // GetValueAction
                value = getValueAction();

                // Set
                this.SetValue(key, value);
            }

            // Return
            return value;
        }

        public bool TryGetValue<TItem>(object key, out TItem? value)
        {
            #region Contracts

            if (key == null) throw new ArgumentException($"{nameof(key)}=null");

            #endregion

            // MemoryCache
            return _memoryCache.TryGetValue(key, out value);
        }


        private DateTime CreateExpirationTime()
        {
            // Sync
            lock (_syncRoot)
            {
                // CurrentTime
                var currentTime = DateTime.Now;
                if (_expirationTime > currentTime && _expirationType == ExpirationType.UnifiedExpiration) return _expirationTime;

                // ExpirationTime
                var expirationTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, currentTime.Minute, currentTime.Second);
                do
                {
                    expirationTime = expirationTime.AddMinutes(_expirationMinutes);
                }
                while ((expirationTime > currentTime) == false);

                // Update
                _expirationTime = expirationTime;

                // Return
                return _expirationTime;
            }
        }
    }
}
