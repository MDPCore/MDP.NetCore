using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public interface SignatureService
    {
        // Methods
        public bool ValidateSignature(string content, string signature);
    }
}
