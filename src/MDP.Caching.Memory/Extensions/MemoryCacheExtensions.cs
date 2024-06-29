using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Caching.Memory
{
    public static class MemoryCacheExtensions
    {
        // Methods
        public static TItem GetValue<TItem>(this IMemoryCache memoryCache, object key, Func<TItem> getValueAction, TimeSpan expireTimeSpan = default)
        {
            #region Contracts

            if (memoryCache == null) throw new ArgumentException($"{nameof(key)}=null");
            if (key == null) throw new ArgumentException($"{nameof(key)}=null");
            if (getValueAction == null) throw new ArgumentException($"{nameof(getValueAction)}=null");

            #endregion

            // Require
            if (expireTimeSpan == default) expireTimeSpan = TimeSpan.FromMinutes(30);

            // TryGetValue
            var value = default(TItem);
            if (memoryCache.TryGetValue(key, out value) == false)
            {
                // GetValueAction
                value = getValueAction();

                // Set
                memoryCache.Set(key, value, DateTimeOffset.Now.Add(expireTimeSpan));
            }

            // Return
            return value;
        }
    }
}