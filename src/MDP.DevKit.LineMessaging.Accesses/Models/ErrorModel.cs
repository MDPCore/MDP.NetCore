using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging.Accesses
{
    internal class ErrorModel
    {
        // Properties
        public string? message { get; set; } = string.Empty;

        public detail[]? details { get; set; } = null;


        // Class
        public class detail
        {
            // Properties
            public string? message { get; set; } = string.Empty;

            public string? property { get; set; } = string.Empty;
        }


        // Methods
        public LineMessageException ToException()
        {
            // Create
            var exception = new LineMessageException
            (
                message: this.message,
                details: this.details?.Select(o => new LineMessageException.Detail()
                {
                    Message = o.message,
                    Property = o.property,
                }).ToList()
            );

            // Return
            return exception;
        }
    }
}
