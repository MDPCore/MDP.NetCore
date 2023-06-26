using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging.Accesses
{
    [MDP.Registration.Service<SignatureService>()]
    public class RestSignatureService : SignatureService
    {
        // Fields
        private readonly string _channelSecret = string.Empty;


        // Constructors
        public RestSignatureService(string channelSecret)
        {
            #region Contracts

            if (string.IsNullOrEmpty(channelSecret) == true) throw new ArgumentException($"{nameof(channelSecret)}=null");

            #endregion

            // Default
            _channelSecret = channelSecret;
        }


        // Methods
        public bool ValidateSignature(string content, string signature)
        {
            #region Contracts

            if (string.IsNullOrEmpty(content) == true) throw new ArgumentException($"{nameof(content)}=null");
            if (string.IsNullOrEmpty(signature) == true) throw new ArgumentException($"{nameof(signature)}=null");

            #endregion

            // Validate
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] channelSecretBytes = Encoding.UTF8.GetBytes(_channelSecret);
            if (Convert.ToBase64String(new HMACSHA256(channelSecretBytes).ComputeHash(contentBytes)) != signature)
            {
                return false;
            }

            // Return
            return true;
        }
    }
}
