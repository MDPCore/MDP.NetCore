using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class IdentityContext<TUser, TUserRepository>
        where TUser : User
        where TUserRepository : class, UserRepository<TUser>
    {
        // Fields
        private readonly RoleRepository _roleRepository = null;

        private readonly TUserRepository _userRepository = null;

        private readonly UserRoleRepository _userRoleRepository = null;

        private readonly UserLoginRepository _userLoginRepository = null;

        private readonly List<IdentityService> _identityServiceList = null;


        // Constructors
        public IdentityContext
        (
            RoleRepository roleRepository,
            TUserRepository userRepository,
            UserRoleRepository userRoleRepository,
            UserLoginRepository userLoginRepository,
            List<IdentityService> identityServiceList
        )
        {
            #region Contracts

            if (roleRepository == null) throw new ArgumentException(nameof(roleRepository));
            if (userRepository == null) throw new ArgumentException(nameof(userRepository));
            if (userRoleRepository == null) throw new ArgumentException(nameof(userRoleRepository));
            if (userLoginRepository == null) throw new ArgumentException(nameof(userLoginRepository));
            if (identityServiceList == null) throw new ArgumentException(nameof(identityServiceList));

            #endregion

            // Default
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userLoginRepository = userLoginRepository;
            _identityServiceList = identityServiceList;

            // IdentityService
            foreach (var identityService in _identityServiceList.OfType<IdentityService<TUser>>()) 
            { 
                identityService.Initialize
                (
                    _userRepository,
                    _userLoginRepository
                ); 
            }
        }


        // Properties
        public RoleRepository RoleRepository { get { return _roleRepository; } }

        public TUserRepository UserRepository { get { return _userRepository; } }

        public UserRoleRepository UserRoleRepository { get { return _userRoleRepository; } }


        // Methods
        public TIdentityService GetService<TIdentityService>()
            where TIdentityService : class
        {
            // Get
            return _identityServiceList.OfType<TIdentityService>().FirstOrDefault();
        }
    }
}
