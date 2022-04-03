using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface UserRoleRepository
    {
        // Methods
        void Add(UserRole userRole);

        List<UserRole> FindAllByUserId(string userId);
    }
}
