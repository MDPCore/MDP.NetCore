using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{
    public class MockUserRoleRepository : MockRepository<UserRole, string, string>, UserRoleRepository
    {
        // Constructors
        public MockUserRoleRepository() : base(userRole => Tuple.Create(userRole.UserId, userRole.RoleId))
        {
            // Default

        }


        // Methods
        public void Add(List<UserRole> userRoleList)
        {
            #region Contracts

            if (userRoleList == null) throw new ArgumentException(nameof(userRoleList));

            #endregion

            // Add
            this.EntityList.AddRange(userRoleList);
        }

        public List<UserRole> FindAllByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // FindAll
            return this.EntityList.FindAll(o => o.UserId == userId);
        }
    }
}
