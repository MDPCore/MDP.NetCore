using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public class TextMessage : Message
    {
        // Constants
        private const string DefaultMessageType = "Text";


        // Constructors
        public TextMessage() : base(DefaultMessageType) { }


        // Properties
        public string Text { get; set; } = string.Empty;
    }
}
