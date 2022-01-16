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

        private readonly List<LoginService<TUser>> _loginServiceList = null;


        // Constructors
        public IdentityContext
        (
            RoleRepository roleRepository,
            TUserRepository userRepository,
            UserRoleRepository userRoleRepository,
            UserLoginRepository userLoginRepository,
            List<LoginService<TUser>> loginServiceList
        )
        {
            #region Contracts

            if (roleRepository == null) throw new ArgumentException(nameof(roleRepository));
            if (userRepository == null) throw new ArgumentException(nameof(userRepository));
            if (userRoleRepository == null) throw new ArgumentException(nameof(userRoleRepository));
            if (userLoginRepository == null) throw new ArgumentException(nameof(userLoginRepository));
            if (loginServiceList == null) throw new ArgumentException(nameof(loginServiceList));

            #endregion

            // Default
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userLoginRepository = userLoginRepository;
            _loginServiceList = loginServiceList;

            // LoginService
            foreach (var loginService in _loginServiceList) 
            { 
                loginService.Initialize
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
        public TLoginService GetLoginService<TLoginService>()
            where TLoginService : class
        {
            // Get
            return _loginServiceList.Cast<TLoginService>().FirstOrDefault();
        }
    }
}
