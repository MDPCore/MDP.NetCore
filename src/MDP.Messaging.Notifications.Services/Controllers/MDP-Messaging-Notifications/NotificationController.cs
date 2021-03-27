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
            if (string.IsNullOrEmpty(actionModel.UserId)==true) throw new ArgumentException(nameof(actionModel.UserId));
            if (string.IsNullOrEmpty(actionModel.Title) == true) throw new ArgumentException(nameof(actionModel.Title));
            if (string.IsNullOrEmpty(actionModel.Text) == true) throw new ArgumentException(nameof(actionModel.Text));

            #endregion

            // Notification
            var notification = new Notification()
            {
                Title = actionModel.Title,
                Text = actionModel.Text,
                Icon = null,
                Uri = actionModel.Uri,
            };

            // Send
            _notificationContext.Send(notification, actionModel.UserId);

            // Return
            return new SendResultModel();
        }


        // Class
        public class SendActionModel
        {
            // Properties
            public string UserId { get; set; }

            public string Title { get; set; }

            public string Text { get; set; }

            public string Uri { get; set; }
        }

        public class SendResultModel
        {
            // Properties
            
        }
    }
}
