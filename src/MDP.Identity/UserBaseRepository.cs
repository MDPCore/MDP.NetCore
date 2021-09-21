using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public interface UserBaseRepository<TUser> where TUser : UserBase 
    {
        // Methods
        void Add(TUser user);

        void Update(TUser user);

        void Remove(string userId);

        TUser FindByUserId(string userId);

        TUser FindByProperty(string propertyName, string propertyValue);

        List<TUser> FindAll();
    }
}
