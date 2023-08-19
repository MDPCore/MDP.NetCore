using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    public class OpenAIException : Exception
    {
        // Constructors
        public OpenAIException(string message = null, string type = null, string param = null, string code = null) : base(message) 
        {
            // Default
            this.Type = type;
            this.Param = param; 
            this.Code = code;
        }


        // Properties
        public string Type { get; set; } = string.Empty;

        public string Param { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;


        // Methods
        public override string ToString()
        {
            // MessageBuilder
            var messageBuilder = new StringBuilder();

            // Base
            //messageBuilder.AppendLine(base.ToString() ?? string.Empty);

            // This
            messageBuilder.AppendLine(System.Text.Json.JsonSerializer.Serialize(new
            {
                Message = this.Message, 
                Type = this.Type,
                Param = this.Param,
                Code = this.Code
            }, new JsonSerializerOptions { WriteIndented = false }));

            // Return
            return messageBuilder.ToString();
        }
    }
}
