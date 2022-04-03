using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class IdentityContext
    {
        // Fields
        private readonly RoleRepository _roleRepository = null;

        private readonly UserRepository _userRepository = null;

        private readonly UserRoleRepository _userRoleRepository = null;

        private readonly UserLoginRepository _userLoginRepository = null;

        private readonly UserTokenRepository _userTokenRepository = null;

        private readonly List<IdentityService> _identityServiceList = null;


        // Constructors
        public IdentityContext
        (
            RoleRepository roleRepository,
            UserRepository userRepository,
            UserRoleRepository userRoleRepository,
            UserLoginRepository userLoginRepository,
            UserTokenRepository userTokenRepository,
            List<IdentityService> identityServiceList
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

        public UserRepository UserRepository { get { return _userRepository; } }

        public UserRoleRepository UserRoleRepository { get { return _userRoleRepository; } }

        public UserLoginRepository UserLoginRepository { get { return _userLoginRepository; } }

        public UserTokenRepository UserTokenRepository { get { return _userTokenRepository; } }


        // Methods
        public TRepository GetRepository<TRepository>()
           where TRepository : class
        {
            // Get
            if(_roleRepository is TRepository) return _roleRepository as TRepository;
            if (_userRepository is TRepository) return _userRepository as TRepository;
            if (_userRoleRepository is TRepository) return _userRoleRepository as TRepository;
            if (_userLoginRepository is TRepository) return _userLoginRepository as TRepository;
            if (_userTokenRepository is TRepository) return _userTokenRepository as TRepository;

            // Return
            return null;
        }

        public TIdentityService GetService<TIdentityService>()
           where TIdentityService : class
        {
            // Get
            return _identityServiceList.OfType<TIdentityService>().FirstOrDefault();
        }


        public TUser Login<TUser>(string userId) where TUser : class
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // UserRepository
            var userRepository = this.GetRepository<UserRepository<TUser>>();
            if (userRepository == null) throw new InvalidOperationException($"{nameof(userRepository)}=null");

            // User
            var user = userRepository.FindByUserId(userId);
            if (user == null) return null;

            // Return
            return user;
        }

        public TUser Login<TUser>(string loginType, string loginValue) where TUser : class
        {
            #region Contracts

            if (string.IsNullOrEmpty(loginType) == true) throw new ArgumentException(nameof(loginType));
            if (string.IsNullOrEmpty(loginValue) == true) throw new ArgumentException(nameof(loginValue));

            #endregion

            // UserLogin
            var userLogin = _userLoginRepository.FindByLoginType(loginType, loginValue);
            if (userLogin == null) return null;

            // Return
            return this.Login<TUser>(userLogin.UserId);
        }


        public void Register<TUser>(TUser user) where TUser : class
        {
            #region Contracts

            if (user == null) throw new ArgumentException(nameof(user));

            #endregion
                        
            // UserRepository
            var userRepository = this.GetRepository<UserRepository<TUser>>();
            if (userRepository == null) throw new InvalidOperationException($"{nameof(userRepository)}=null");

            // Require
            if (userRepository.Exists(user) == true) throw new InvalidOperationException($"{nameof(user)} is existed");

            // Add
            userRepository.Add(user);
        }

        public void Register(UserLogin userLogin)
        {
            #region Contracts

            if (userLogin == null) throw new ArgumentException(nameof(userLogin));

            #endregion

            // Require
            if (_userRepository.Exists(userLogin.UserId) == false) throw new InvalidOperationException($"{nameof(userLogin.UserId)} not existed");

            // Add
            _userLoginRepository.Add(userLogin);
        }

        public void Register(UserToken userToken)
        {
            #region Contracts

            if (userToken == null) throw new ArgumentException(nameof(userToken));

            #endregion

            // Require
            if (_userRepository.Exists(userToken.UserId) == false) throw new InvalidOperationException($"{nameof(userToken.UserId)} not existed");

            // Remove
            _userTokenRepository.Remove(userToken.UserId, userToken.TokenType);

            // Add
            _userTokenRepository.Add(userToken);
        }
    }
}
