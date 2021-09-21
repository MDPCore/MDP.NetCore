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

        void Update(UserLogin userLogin);

        void RemoveAll(string userId);

        UserLogin FindByLoginType(string userId, string loginType);

        UserLogin FindByLoginValue(string loginType, string loginValue);
    }
}
