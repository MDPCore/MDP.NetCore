using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{
    public class MockUserRepository : MockRepository<MockUser, string>, UserRepository<MockUser>
    {
        // Constructors
        public MockUserRepository() : base(user => Tuple.Create(user.UserId))
        {
            // Default

        }


        // Methods
        public bool Exists(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // User
            var user = this.EntityList.FirstOrDefault(o => o.UserId == userId);
            if (user == null) return false;

            // Return
            return true;
        }

        public bool Exists(MockUser user)
        {
            #region Contracts

            if (user == null) throw new ArgumentException(nameof(user));

            #endregion

            // Exists
            return this.Exists(user.UserId);
        }

        public MockUser FindByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Find
            return this.EntityList.FirstOrDefault(o => o.UserId == userId);
        }
    }
}
