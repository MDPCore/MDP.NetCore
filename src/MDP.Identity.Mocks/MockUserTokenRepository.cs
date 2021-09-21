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
        public void RemoveAll(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // RemoveAll
            this.EntityList.RemoveAll(o => o.UserId == userId);
        }

        public UserToken FindByTokenType(string userId, string tokenType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(tokenType) == true) throw new ArgumentException(nameof(tokenType));

            #endregion

            // Find
            return this.EntityList.FirstOrDefault(o => o.UserId == userId && o.TokenType== tokenType);
        }       
    }
}
