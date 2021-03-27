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
        public void RemoveByUserId(string userId, string deviceType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(deviceType) == true) throw new ArgumentException(nameof(deviceType));

            #endregion

            // Remove
            this.Remove(userId, deviceType);
        }

        public Registration FindByUserId(string userId, string deviceType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrEmpty(deviceType) == true) throw new ArgumentException(nameof(deviceType));

            #endregion

            // Find
            return this.FindById(userId, deviceType);
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
