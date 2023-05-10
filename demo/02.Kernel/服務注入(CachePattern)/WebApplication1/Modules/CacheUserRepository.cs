using MDP.Registration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApplication1
{
    [Service<UserRepository>()]
    public class CacheUserRepository : UserRepository
    {
        // Fields
        private readonly IMemoryCache _userCache = new MemoryCache(new MemoryCacheOptions());

        private readonly UserRepository _userRepository;


        // Constructors
        public CacheUserRepository(UserRepository userRepository)
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
            // UserCache-Hit
            var user = _userCache.Get<User>("Test");
            if (user != null) return user;

            // UserRepository-Find
            user = _userRepository.Find();
            if (user == null) return user;

            // UserCache-Set
            _userCache.Set<User>("Test", user, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });

            // Return
            return user;
        }
    }
}
