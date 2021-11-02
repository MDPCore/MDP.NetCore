using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{
    public abstract class MockBaseUserRepository<TUser> : MockRepository<TUser, string>, BaseUserRepository<TUser> 
        where TUser : BaseUser
    {
        // Constructors
        public MockBaseUserRepository() : base(user => Tuple.Create(user.UserId))
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

        public virtual List<TUser> FindAllByProperty(string propertyName, string propertyValue)
        {
            #region Contracts

            if (string.IsNullOrEmpty(propertyName) == true) throw new ArgumentException(nameof(propertyName));
            if (string.IsNullOrEmpty(propertyValue) == true) throw new ArgumentException(nameof(propertyValue));

            #endregion

            // Throw
            throw new NotSupportedException($"{nameof(propertyName)}={propertyName}");
        }
    }
}
