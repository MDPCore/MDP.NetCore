using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class UserRoleService
    {
        // Fields
        private readonly UserRoleRepository _userRoleRepository = null;


        // Constructors
        public UserRoleService(UserRoleRepository userRoleRepository)
        {
            #region Contracts

            if (userRoleRepository == null) throw new ArgumentException(nameof(userRoleRepository));

            #endregion

            // Default
            _userRoleRepository = userRoleRepository;
        }


        // Methods
        public void Update(string userId, List<UserRole> userRoleList)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (userRoleList == null) throw new ArgumentException(nameof(userRoleList));

            #endregion

            // Require
            userRoleList.Verify();
            if (userId != (userRoleList.GetUserId() ?? userId)) throw new InvalidOperationException($"{nameof(userRoleList)}.UserId is failed.");

            // Update
            _userRoleRepository.RemoveAll(userId);
            _userRoleRepository.Add(userRoleList);
        }

        public List<UserRole> FindAllByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Find
            return _userRoleRepository.FindAllByUserId(userId);
        }
    }
}
