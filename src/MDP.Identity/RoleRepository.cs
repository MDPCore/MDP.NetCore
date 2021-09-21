using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface RoleRepository
    {
        // Methods
        void Add(Role role);

        void Update(Role role);

        void Remove(string roleId);

        List<Role> FindAll();
    }
}
