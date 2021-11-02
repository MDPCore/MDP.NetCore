using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{

    public class MockUserLoginRepository : MockRepository<UserLogin, string, string>, UserLoginRepository
    {
        // Constructors
        public MockUserLoginRepository() : base(userLogin => Tuple.Create(userLogin.UserId, userLogin.LoginType))
        {
            // Default

        }



        // Methods
        public void RemoveAll(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // RemoveAll
            this.EntityList.RemoveAll(o => o.UserId == userId);
        }


        public UserLogin FindByLoginType(string userId, string loginType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));

            #endregion

            // Find
            return this.EntityList.FirstOrDefault(o => o.UserId == userId && o.LoginType == loginType);
        }

        public List<UserLogin> FindAllByLoginValue(string loginType, string loginValue)
        {
            #region Contracts

            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));

            #endregion

            // Find
            return this.EntityList.FindAll(o => o.LoginType == loginType && o.LoginValue == loginValue);
        }
    }
}
