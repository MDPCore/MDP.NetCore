using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    internal static class UserRoleListExtensions
    {
        // Methods
        public static string GetUserId(this List<UserRole> userRoleList)
        {
            #region Contracts

            if (userRoleList == null) throw new ArgumentException(nameof(userRoleList));

            #endregion

            // Require
            if (userRoleList.Count == 0) return null;

            // Return
            return userRoleList.FirstOrDefault()?.UserId;
        }
    }
}
