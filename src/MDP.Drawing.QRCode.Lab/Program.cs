using System;
using System.IO;
using System.Threading.Tasks;

namespace MDP.Drawing.QRCode.Lab
{
    public class Program
    {
        // Methods
        public static void Run(QRCodeContext qrCodeContext)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(qrCodeContext);

            #endregion

            // Base64QRCode
            var base64QRCode = qrCodeContext.CreateToBase64(@"https://tw.yahoo.com/");
            if (string.IsNullOrEmpty(base64QRCode) == true) throw new InvalidOperationException($"{nameof(base64QRCode)}=null");
            
            // Display
            Console.WriteLine(base64QRCode);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
