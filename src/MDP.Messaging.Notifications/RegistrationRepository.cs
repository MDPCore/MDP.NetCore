using System;
using System.Collections.Generic;
using System.Text;

namespace MDP.Messaging.Notifications
{
    public interface RegistrationRepository
    {
        // Methods
        void Add(Registration registration);

        void Update(Registration registration);

        void RemoveByUserId(string userId, string deviceType);

        Registration FindByUserId(string userId, string deviceType);

        List<Registration> FindAllByUserId(List<string> userIdList);
    }
}
