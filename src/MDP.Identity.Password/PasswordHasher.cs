using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity.Password
{
    public interface PasswordHasher
    {
        // Methods
        string HashPassword(string password);
    }
}
