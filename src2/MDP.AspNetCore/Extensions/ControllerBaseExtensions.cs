using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerBaseExtensions
    {
        // Methods
        public static ObjectResult Problem(this ControllerBase controller, ProblemDetails problemDetails)
        {
            #region Contracts

            if (controller == null) throw new ArgumentException($"{nameof(controller)}=null");
            if (problemDetails == null) throw new ArgumentException($"{nameof(problemDetails)}=null");

            #endregion

            // Return
            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
        }
    }
}