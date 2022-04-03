using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{
    public class MockUserTokenRepository : MockRepository<UserToken, string, string>, UserTokenRepository
    {
        // Constructors
        public MockUserTokenRepository() : base(userToken => Tuple.Create(userToken.UserId, userToken.TokenType))
        {
            // Default

        }


        // Methods
        public UserToken FindByTokenType(string userId, string loginType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            
            #endregion

            // Find
            return this.EntityList.FirstOrDefault(o => o.UserId == userId && o.TokenType == loginType);
        }
    }
}
