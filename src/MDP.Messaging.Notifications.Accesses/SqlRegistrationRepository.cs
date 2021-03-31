using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Accesses
{
    public class SqlRegistrationRepository : RegistrationRepository
    {
        // Fields
        private readonly string _connectionString = null;


        // Constructors
        public SqlRegistrationRepository(string connectionString)
        {
            #region Contracts

            if (string.IsNullOrEmpty(connectionString) == true) throw new ArgumentException(nameof(connectionString));

            #endregion

            // Default
            _connectionString = connectionString;
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
