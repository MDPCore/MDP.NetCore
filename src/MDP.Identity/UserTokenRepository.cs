using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface UserTokenRepository
    {
        // Methods
        void Add(UserToken userToken);

        void RemoveAll(string userId);

        UserToken FindByTokenType(string userId, string tokenType);
    }
}
