using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Line
{
    public class LineAuthenticationPostConfigureOptions : IPostConfigureOptions<LineAuthenticationOptions>
    {
        // Methods
        public void PostConfigure(string name, LineAuthenticationOptions options)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException(nameof(name));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Require
            if (string.IsNullOrEmpty(options.ClientId) == true) throw new InvalidOperationException($"{nameof(options.ClientId)}=null");
            if (string.IsNullOrEmpty(options.ClientSecret) == true) throw new InvalidOperationException($"{nameof(options.ClientSecret)}=null");
            //if (string.IsNullOrEmpty(options.SignInScheme) == true) throw new InvalidOperationException($"{nameof(options.SignInScheme)}=null");
        }
    }
}
