using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class IdentityContext<TUser, TUserRepository>
        where TUser : BaseUser
        where TUserRepository : class, BaseUserRepository<TUser>
    {
        // Fields
        private readonly RoleRepository _roleRepository = null;

        private readonly TUserRepository _userRepository = null;

        private readonly UserRoleRepository _userRoleRepository = null;

        private readonly UserLoginRepository _userLoginRepository = null;

        private readonly UserTokenRepository _userTokenRepository = null;

        private readonly List<IdentityService<TUser>> _identityServiceList = null;

        private readonly UserRoleService _userRoleService = null;


        // Constructors
        public IdentityContext
        (
            RoleRepository roleRepository,
            TUserRepository userRepository,
            UserRoleRepository userRoleRepository,
            UserLoginRepository userLoginRepository,
            UserTokenRepository userTokenRepository,
            List<IdentityService<TUser>> identityServiceList
        )
        {
            #region Contracts

            if (roleRepository == null) throw new ArgumentException(nameof(roleRepository));
            if (userRepository == null) throw new ArgumentException(nameof(userRepository));
            if (userRoleRepository == null) throw new ArgumentException(nameof(userRoleRepository));
            if (userLoginRepository == null) throw new ArgumentException(nameof(userLoginRepository));
            if (userTokenRepository == null) throw new ArgumentException(nameof(userTokenRepository));
            if (identityServiceList == null) throw new ArgumentException(nameof(identityServiceList));

            #endregion

            // Default
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userLoginRepository = userLoginRepository;
            _userTokenRepository = userTokenRepository;
            _identityServiceList = identityServiceList;

            // Service
            _userRoleService = new UserRoleService(_userRoleRepository);

            // IdentityService
            foreach (var identityService in _identityServiceList) 
            { 
                identityService.Initialize
                (
                    _roleRepository,
                    _userRepository,
                    _userRoleRepository,
                    _userLoginRepository,
                    _userTokenRepository
                ); 
            }
        }


        // Properties
        public RoleRepository RoleRepository { get { return _roleRepository; } }

        public TUserRepository UserRepository { get { return _userRepository; } }

        public UserRoleService UserRoleService { get { return _userRoleService; } }


        // Methods
        public void Register(TUser user, List<UserRole> userRoleList)
        {
            #region Contracts

            if (user == null) throw new ArgumentException(nameof(user));
            if (userRoleList == null) throw new ArgumentException(nameof(userRoleList));

            #endregion

            // Require
            if (user.UserId != (userRoleList.GetUserId() ?? user.UserId)) throw new InvalidOperationException($"{nameof(userRoleList)}.UserId is failed.");

            // Add
            _userRepository.Add(user);
            _userRoleRepository.Add(userRoleList);
        }

        public void Unregister(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Remove
            _userRepository.Remove(userId);
            _userRoleRepository.RemoveAll(userId);
            _userLoginRepository.RemoveAll(userId);
            _userTokenRepository.RemoveAll(userId);
        }


        public TService GetService<TService>()
            where TService : class
        {
            // Service
            if (_userRoleService is TService) return _userRoleService as TService;

            // IdentityService
            return _identityServiceList.Cast<TService>().FirstOrDefault();
        }
    }
}
