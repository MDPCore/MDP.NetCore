using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface UserRepository
    {
        // Methods
        bool Exists(string userId);
    }

    public interface UserRepository<TUser> : UserRepository
        where TUser : class
    {
        // Methods
        bool Exists(TUser user);

        void Add(TUser user);

        TUser FindByUserId(string userId);
    }
}
