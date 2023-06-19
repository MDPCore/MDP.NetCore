using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public class StickerMessage : Message
    {
        // Constants
        private const string DefaultMessageType = "Sticker";


        // Constructors
        public StickerMessage() : base(DefaultMessageType) { }


        // Properties
        public int PackageId { get; set; } = 1;

        public int StickerId { get; set; } = 1;
    }
}
