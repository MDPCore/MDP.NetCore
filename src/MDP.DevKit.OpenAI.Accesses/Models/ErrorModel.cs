using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    internal class ErrorModel
    {
        // Properties
        public Error error { get; set; } = null;


        // Class
        public class Error
        {
            // Properties
            public string message { get; set; } = string.Empty;

            public string type { get; set; } = string.Empty;

            public string param { get; set; } = string.Empty;

            public string code { get; set; } = string.Empty;
        }


        // Methods
        public OpenAIException ToException()
        {
            // Create
            var exception = new OpenAIException
            (
                message: this.error?.message,
                type: this.error?.type,
                param: this.error?.param,
                code: this.error?.code
            );

            // Return
            return exception;
        }
    }
}
