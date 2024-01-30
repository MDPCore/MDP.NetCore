using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.Network.Http
{
    public class HttpException<TErrorModel> : Exception where TErrorModel : class
    {
        // Constructors
        public HttpException(HttpStatusCode statusCode, string message, TErrorModel model) : base(message)
        {
            // Default
            this.StatusCode = statusCode;
            this.ErrorModel = model;
        }


        // Properties
        public HttpStatusCode StatusCode { get; }

        public TErrorModel ErrorModel { get; }


        // Methods
        public override string ToString()
        {
            // MessageBuilder
            var messageBuilder = new StringBuilder();

            // Base
            messageBuilder.AppendLine(base.ToString() ?? string.Empty);

            // ErrorModel
            messageBuilder.AppendLine(System.Text.Json.JsonSerializer.Serialize(this.ErrorModel, new JsonSerializerOptions
            {
                WriteIndented = true
            }));

            // Return
            return messageBuilder.ToString();
        }
    }
}
