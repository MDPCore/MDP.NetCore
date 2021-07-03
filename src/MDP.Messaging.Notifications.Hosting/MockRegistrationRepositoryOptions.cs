using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications.Hosting
{
    public class MockRegistrationRepositoryOptions
    {
        // Properties
        public List<Registration> RegistrationList { get; set; } = new List<Registration>();
    }
}
