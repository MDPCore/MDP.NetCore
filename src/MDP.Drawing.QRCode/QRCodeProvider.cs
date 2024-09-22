using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Drawing.QRCode
{
    public interface QRCodeProvider
    {
        // Methods
        string CreateToBase64(string content, int width, int height);
    }
}
