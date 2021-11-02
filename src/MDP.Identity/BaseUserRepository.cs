using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface BaseUserRepository<TUser> where TUser : BaseUser 
    {
        // Methods
        void Add(TUser user);

        void Update(TUser user);

        void Remove(string userId);

        TUser FindByUserId(string userId);

        List<TUser> FindAll();

        List<TUser> FindAllByProperty(string propertyName, string propertyValue);
    }
}
