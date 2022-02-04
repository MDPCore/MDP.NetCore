using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface UserRepository<TUser> where TUser : User
    {
        // Methods
        TUser FindByUserId(string userId);
    }
}
