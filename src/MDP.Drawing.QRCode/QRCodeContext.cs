using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Drawing.QRCode
{
    [Service<QRCodeContext>(singleton: true)]
    public class QRCodeContext
    {
        // Fields
        private readonly QRCodeProvider _qrCodeProvider = null;


        //  Constructors
        public QRCodeContext(QRCodeProvider qrCodeProvider)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(qrCodeProvider);

            #endregion

            // Default
            _qrCodeProvider = qrCodeProvider;
        }


        // Methods
        public string CreateToBase64(string content, int width = 256, int height = 256)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(content);

            #endregion

            // Return
            return _qrCodeProvider.CreateToBase64(content, width, height);
        }
    }
}
