using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    public static class HttpRequestExtensions
    {
        // Methods
        public static bool HasAccept(this HttpRequest request, List<string> acceptList)
        {
            #region Contracts

            if (request == null) throw new ArgumentException($"{nameof(request)}=null");
            if (acceptList == null) throw new ArgumentException($"{nameof(acceptList)}=null");

            #endregion

            // AcceptList
            foreach (var accept in acceptList)
            {
                // Require
                if (string.IsNullOrEmpty(accept)==true) throw new InvalidProgramException($"{nameof(accept)}=true");

                // Contains
                if (request.Headers.Accept.Any(o => o.Contains(accept, StringComparison.OrdinalIgnoreCase)) == true) return true;
            }

            // Return
            return false;
        }
    }
}
