using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public class Message
    {
        // Constructors
        public Message()
        {

        }

        protected Message(string messageType)
        {
            #region Contracts

            if (string.IsNullOrEmpty(messageType) == true) throw new ArgumentException($"{nameof(messageType)}=null");

            #endregion

            // Default
            this.MessageType = messageType;
        }


        // Properties
        public string MessageType { get; set; } = string.Empty;

        public Sender? Sender { get; set; } = null;
    }
}