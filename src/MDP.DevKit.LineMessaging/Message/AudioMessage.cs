using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public class AudioMessage : Message
    {
        // Constants
        private const string DefaultMessageType = "Audio";


        // Constructors
        public AudioMessage() : base(DefaultMessageType) { }


        // Properties
        public string OriginalContentUrl { get; set; } = string.Empty;

        public int Duration { get; set; } = 0; // Milliseconds
    }
}
