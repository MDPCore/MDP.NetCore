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
        public static void Verify(this List<UserRole> userRoleList)
        {
            #region Contracts

            if (userRoleList == null) throw new ArgumentException(nameof(userRoleList));

            #endregion

            // UserRole
            foreach (var userRole in userRoleList)
            {
                userRole.Verify();
            }

            // UserId
            if (userRoleList.GroupBy(o => o.UserId).ToList().Count > 1) throw new InvalidOperationException($"{nameof(userRoleList)}.UserId is failed.");
        }

        public static string GetUserId(this List<UserRole> userRoleList)
        {
            #region Contracts

            if (userRoleList == null) throw new ArgumentException(nameof(userRoleList));

            #endregion

            // Require
            userRoleList.Verify();
            if (userRoleList.Count == 0) return null;

            // Return
            return userRoleList.FirstOrDefault()?.UserId;
        }
    }
}
