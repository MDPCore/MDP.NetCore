using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.Network.Rest
{
    public class RestResponseException<TExceptionModel> : Exception where TExceptionModel : class
    {
        // Constructors
        public RestResponseException(string? message, TExceptionModel? model, HttpStatusCode? statusCode) : base(message)        
        {
            // Default
            this.Model = model;
            this.StatusCode = statusCode;
        }


        // Properties
        public TExceptionModel? Model { get; }

        public HttpStatusCode? StatusCode { get; }


        // Methods
        public override string ToString()
        {
            // MessageBuilder
            var messageBuilder = new StringBuilder();
                    
            // Base
            messageBuilder.AppendLine(base.ToString() ?? string.Empty);

            // Model
            messageBuilder.AppendLine(System.Text.Json.JsonSerializer.Serialize(this.Model, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            // Return
            return messageBuilder.ToString();
        }
    }
}
