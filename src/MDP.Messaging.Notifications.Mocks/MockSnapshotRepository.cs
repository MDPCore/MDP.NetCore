using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLK.Mocks;

namespace MDP.Messaging.Notifications.Mocks
{
    public class MockRegistrationRepository : MockRepository<Registration, string, string>, RegistrationRepository
    {
        // Constructors
        public MockRegistrationRepository() : base(registration => Tuple.Create(registration.UserId, registration.DeviceType))
        {
            // Default

        }


        // Methods
        public void RemoveAllByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // RemoveAll
            this.EntityList.RemoveAll(registration => registration.UserId == userId);
        }

        public List<Registration> FindAllByUserId(List<string> userIdList)
        {
            #region Contracts

            if (userIdList == null) throw new ArgumentException(nameof(userIdList));

            #endregion

            // FindAll
            return this.EntityList.Where(registration => userIdList.Contains(registration.UserId) == true).ToList();
        }
    }
}
