using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Line
{
    public class LiffAuthenticationPostConfigureOptions : IPostConfigureOptions<LiffAuthenticationOptions>
    {
        // Methods
        public void PostConfigure(string name, LiffAuthenticationOptions options)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException(nameof(name));
            if (options == null) throw new ArgumentException(nameof(options));

            #endregion

            // Require
            if (string.IsNullOrEmpty(options.ClientId) == true) throw new InvalidOperationException($"{nameof(options.ClientId)}=null");
            if (string.IsNullOrEmpty(options.ClientSecret) == true) throw new InvalidOperationException($"{nameof(options.ClientSecret)}=null");

            // Backchannel
            if (options.Backchannel == null)
            {
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                options.Backchannel.Timeout = options.BackchannelTimeout;
                options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            }
        }
    }
}
