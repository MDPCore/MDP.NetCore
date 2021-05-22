using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications
{
    [MDP.AspNetCore.Module("MDP-Messaging-Notifications")]
    public partial class RegistrationController : Controller
    {
        // Fields
        private readonly NotificationContext _notificationContext = null;


        // Constructors
        public RegistrationController(NotificationContext notificationContext)
        {
            #region Contracts

            if (notificationContext == null) throw new ArgumentException(nameof(notificationContext));

            #endregion

            // Default
            _notificationContext = notificationContext; 
        }
    }

    public partial class RegistrationController : Controller
    {
        // Methods
        public ActionResult<RegisterResultModel> Register([FromBody] RegisterActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));
            if (actionModel.Registration == null) throw new ArgumentException(nameof(actionModel.Registration));

            #endregion

            // Register
            _notificationContext.Register(actionModel.Registration);

            // Return
            return new RegisterResultModel();
        }


        // Class
        public class RegisterActionModel
        {
            // Properties
            public Registration Registration { get; set; }
        }

        public class RegisterResultModel
        {
            // Properties
            
        }
    }

    public partial class RegistrationController : Controller
    {
        // Methods
        public ActionResult<UnregisterResultModel> Unregister([FromBody] UnregisterActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));
            if (string.IsNullOrEmpty(actionModel.UserId)==true) throw new ArgumentException(nameof(actionModel.UserId));
            if (string.IsNullOrEmpty(actionModel.DeviceType) == true) throw new ArgumentException(nameof(actionModel.DeviceType));

            #endregion

            // Unregister
            var registration = _notificationContext.RegistrationRepository.FindByUserId(actionModel.UserId, actionModel.DeviceType);
            if(registration!=null) _notificationContext.Unregister(actionModel.UserId, actionModel.DeviceType);

            // Return
            return new UnregisterResultModel()
            {
                Registration = registration
            };
        }


        // Class
        public class UnregisterActionModel
        {
            // Properties
            public string UserId { get; set; }

            public string DeviceType { get; set; }
        }

        public class UnregisterResultModel
        {
            // Properties
            public Registration Registration { get; set; }
        }
    }
}
