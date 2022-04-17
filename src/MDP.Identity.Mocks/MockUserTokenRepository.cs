using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{
    public class MockUserTokenRepository : MockRepository<UserToken, string, string, string>, UserTokenRepository
    {
        // Constructors
        public MockUserTokenRepository() : base(userToken => Tuple.Create(userToken.UserId, userToken.LoginType, userToken.TokenType))
        {
            // Default

        }


        // Methods
        public UserToken FindByTokenType(string userId, string loginType, string tokenType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(tokenType) == true) throw new ArgumentException(nameof(tokenType));

            #endregion

            // Find
            return this.EntityList.FirstOrDefault(o => o.UserId == userId && o.LoginType == loginType && o.TokenType == tokenType);
        }
    }
}
