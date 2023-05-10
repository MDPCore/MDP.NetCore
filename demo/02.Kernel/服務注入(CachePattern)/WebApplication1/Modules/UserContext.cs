using MDP.Registration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApplication1
{
    [Service<UserContext>(singleton: true)]
    public class UserContext
    {
        // Fields
        private readonly UserRepository _userRepository;


        // Constructors
        public UserContext(UserRepository userRepository)
        {
            #region Contracts

            if (userRepository == null) throw new ArgumentException($"{nameof(userRepository)}=null");

            #endregion

            // Default
            _userRepository = userRepository;
        }


        // Methods
        public User? Find()
        {
            // User
            var user = _userRepository.Find();

            // Return
            return user;
        }
    }
}
