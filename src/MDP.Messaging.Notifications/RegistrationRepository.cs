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

        void RemoveAllByUserId(string userId);

        List<Registration> FindAllByUserId(List<string> userIdList);
    }
}
