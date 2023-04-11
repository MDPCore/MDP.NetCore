using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore
{
    public static class ProblemDetailsExtensions
    {
        // Constants
        private const string DefaultTraceIdPropertyName = "traceId";


        // Methods
        public static string? GetTraceId(this ProblemDetails problemDetails)
        {
            #region Contracts

            if (problemDetails == null) throw new ArgumentException($"{nameof(problemDetails)}=null");
            
            #endregion

            // Return
            return problemDetails.Extensions[DefaultTraceIdPropertyName]?.ToString();
        }
    }
}
