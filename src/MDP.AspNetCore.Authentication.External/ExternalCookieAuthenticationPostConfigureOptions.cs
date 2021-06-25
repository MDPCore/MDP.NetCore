using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.External
{
    public class ExternalCookieAuthenticationPostConfigureOptions : IPostConfigureOptions<ExternalCookieAuthenticationOptions>
    {
        // Methods
        public void PostConfigure(string name, ExternalCookieAuthenticationOptions options)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException(nameof(name));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Require
            if (string.IsNullOrEmpty(options.DefaultScheme) == true) throw new InvalidOperationException($"{nameof(options.DefaultScheme)}=null");
            if (string.IsNullOrEmpty(options.CallbackPath) == true) throw new InvalidOperationException($"{nameof(options.CallbackPath)}=null");
        }
    }
}
