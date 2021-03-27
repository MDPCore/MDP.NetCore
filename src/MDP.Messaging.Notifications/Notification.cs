using System;
using System.Collections.Generic;
using System.Text;

namespace MDP.Messaging.Notifications
{
    public class Notification
    {
        // Properties
        public string Title { get; set; }

        public string Text { get; set; }

        public string Icon { get; set; }

        public string Uri { get; set; }


        // Methods
        public bool IsValid()
        {
            // Title
            if (string.IsNullOrEmpty(this.Title) == true) return false;

            // Text
            if (string.IsNullOrEmpty(this.Text) == true) return false;

            // Icon

            // Uri


            // Return
            return true;
        }
    }
}
