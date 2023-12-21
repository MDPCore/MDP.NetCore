using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Caching.Memory
{
    public enum ExpirationType
    {
        /// <summary>
        /// 所有快取項目統一的失效時間
        /// </summary>
        UnifiedExpiration,

        /// <summary>
        /// 每個快取項目各自的失效時間
        /// </summary>
        IndividualExpiration
    }
}
