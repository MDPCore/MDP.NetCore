using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface UserLoginRepository
    {
        // Methods
        void Add(UserLogin userLogin);

        void Remove(string userId, string loginType);

        UserLogin FindByLoginType(string loginType, string loginValue);
    }
}
