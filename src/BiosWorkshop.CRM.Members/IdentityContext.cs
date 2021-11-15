using MDP.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiosWorkshop.CRM.Members
{
    public class IdentityContext : IdentityContext<MemberUser, MemberUserRepository>
    {
        // Constructors
        public IdentityContext
        (
            RoleRepository roleRepository,
            MemberUserRepository userRepository,
            UserRoleRepository userRoleRepository,
            UserLoginRepository userLoginRepository,
            UserTokenRepository userTokenRepository,
            List<IdentityService<MemberUser>> identityServiceList
        ) : base
        (
            roleRepository,
            userRepository,
            userRoleRepository,
            userLoginRepository,
            userTokenRepository,
            identityServiceList
        ) { }
    }
}
