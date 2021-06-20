using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.JwtBearer
{
    public static class HttpContextExtensions
    {
        // Methods
        public static bool HasJwtBearer(this HttpContext context)
        {
            #region Contracts

            if (context == null) throw new ArgumentException(nameof(context));

            #endregion

            // Require
            var authorization = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authorization) == true) return false;
            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == false) return false;

            // Return
            return true;
        }
    }
}
