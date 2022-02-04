using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface IdentityService
    {

    }

    public abstract class IdentityService<TUser> : IdentityService
        where TUser : User
    {
        // Constructors
        internal void Initialize
        (
            UserRepository<TUser> userRepository,
            UserLoginRepository userLoginRepository
        )
        {
            #region Contracts

            if (userRepository == null) throw new ArgumentException(nameof(userRepository));
            if (userLoginRepository == null) throw new ArgumentException(nameof(userLoginRepository));

            #endregion

            // Default
            this.UserRepository = userRepository;
            this.UserLoginRepository = userLoginRepository;
        }


        // Properties
        protected UserRepository<TUser> UserRepository { get; private set; }

        protected UserLoginRepository UserLoginRepository { get; private set; }
    }
}
