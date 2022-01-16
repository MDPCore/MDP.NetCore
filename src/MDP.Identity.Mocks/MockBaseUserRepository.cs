using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{
    public abstract class MockUserRepository<TUser> : MockRepository<TUser, string>, UserRepository<TUser> 
        where TUser : User
    {
        // Constructors
        public MockUserRepository() : base(user => Tuple.Create(user.UserId))
        {
            // Default

        }


        // Methods
        public TUser FindByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Find
            return this.EntityList.FirstOrDefault(o => o.UserId == userId);
        }
    }
}
