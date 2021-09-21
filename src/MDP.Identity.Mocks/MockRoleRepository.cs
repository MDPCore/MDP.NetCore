using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Mocks
{

    public class MockRoleRepository : MockRepository<Role, string>, RoleRepository
    {
        // Constructors
        public MockRoleRepository() : base(role => Tuple.Create(role.RoleId))
        {
            // Default

        }


        // Methods
       
    }
}
