using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.Messaging.Notifications
{
    [MDP.Module("MDP-Messaging-Notifications")]
    public partial class NotificationController : Controller
    {
        // Fields
        private readonly NotificationContext _notificationContext = null;


        // Constructors
        public NotificationController(NotificationContext notificationContext)
        {
            #region Contracts

            if (notificationContext == null) throw new ArgumentException(nameof(notificationContext));

            #endregion

            // Default
            _notificationContext = notificationContext; 
        }
    }

    public partial class NotificationController : Controller
    {
        // Methods
        public ActionResult<SendResultModel> Send([FromBody] SendActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));
            if (actionModel.Notification == null) throw new ArgumentException(nameof(actionModel.Notification));
            if (string.IsNullOrEmpty(actionModel.UserId)==true) throw new ArgumentException(nameof(actionModel.UserId));
            
            #endregion

            // Send
            _notificationContext.Send(actionModel.Notification, actionModel.UserId);

            // Return
            return new SendResultModel();
        }


        // Class
        public class SendActionModel
        {
            // Properties
            public Notification Notification { get; set; }

            public string UserId { get; set; }
        }

        public class SendResultModel
        {
            // Properties
            
        }
    }
}
