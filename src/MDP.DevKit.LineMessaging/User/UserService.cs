using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public interface UserService
    {
        // Methods
        Task<User?> GetProfileAsync(string userId);
    }
}
