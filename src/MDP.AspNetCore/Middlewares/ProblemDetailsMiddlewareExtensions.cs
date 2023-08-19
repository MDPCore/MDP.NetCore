using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using System;

namespace MDP.AspNetCore
{
    public static partial class ProblemDetailsMiddlewareExtensions
    {
        // Methods        
        public static WebApplicationBuilder AddProblemDetails(this WebApplicationBuilder hostBuilder)
        {
            #region Contracts

            if (hostBuilder == null) throw new ArgumentException($"{nameof(hostBuilder)}=null");

            #endregion

            // ProblemDetails
            hostBuilder.Services.AddProblemDetails();

            // Return
            return hostBuilder;
        }
    }

    public static partial class ProblemDetailsMiddlewareExtensions
    {
        // Methods 
        public static WebApplication UseProblemDetails(this WebApplication host)
        {
            #region Contracts

            if (host == null) throw new ArgumentException($"{nameof(host)}=null");

            #endregion

            // ProblemDetails
            ProblemDetailsExtensions.UseProblemDetails(host);

            // Return
            return host;
        }
    }
}
