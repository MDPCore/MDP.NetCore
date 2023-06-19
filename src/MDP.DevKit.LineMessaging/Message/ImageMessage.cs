using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public class ImageMessage : Message
    {
        // Constants
        private const string DefaultMessageType = "Image";


        // Constructors
        public ImageMessage() : base(DefaultMessageType) { }


        // Properties
        public string OriginalContentUrl { get; set; } = string.Empty;

        public string PreviewImageUrl { get; set; } = string.Empty;
    }
}
