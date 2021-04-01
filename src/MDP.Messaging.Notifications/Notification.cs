using System;
using System.Collections.Generic;
using System.Text;

namespace MDP.Messaging.Notifications
{
    public class Notification
    {
        // Properties
        public string Type { get; set; } = null;

        public string Title { get; set; } = null;

        public string Text { get; set; } = null;

        public Dictionary<string, string> Metadata { get; } = new Dictionary<string, string>();


        // Methods
        public bool IsValid()
        {
            // Type
            if (string.IsNullOrEmpty(this.Type) == true) return false;

            // Title
            if (string.IsNullOrEmpty(this.Title) == true) return false;

            // Text
            if (string.IsNullOrEmpty(this.Text) == true) return false;

            // Metadata


            // Return
            return true;
        }
    }
}
