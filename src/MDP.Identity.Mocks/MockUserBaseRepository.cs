using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{

    public abstract class MockUserBaseRepository<TUser> : MockRepository<TUser, string>, UserBaseRepository<TUser> 
        where TUser : UserBase
    {
        // Constructors
        public MockUserBaseRepository() : base(user => Tuple.Create(user.UserId))
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

        public abstract TUser FindByProperty(string propertyName, string propertyValue);
    }
}
