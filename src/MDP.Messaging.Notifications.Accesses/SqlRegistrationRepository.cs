using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Accesses
{
    public class SqlRegistrationRepository : RegistrationRepository
    {
        // Constructors
        public SqlRegistrationRepository()
        {
            
        }


        // Methods
        public void Add(Registration registration)
        {
            throw new NotImplementedException();
        }

        public void Update(Registration registration)
        {
            throw new NotImplementedException();
        }

        public void RemoveByUserId(string userId, string deviceType)
        {
            throw new NotImplementedException();
        }

        public Registration FindByUserId(string userId, string deviceType)
        {
            throw new NotImplementedException();
        }

        public List<Registration> FindAllByUserId(List<string> userIdList)
        {
            throw new NotImplementedException();
        }        
    }
}
