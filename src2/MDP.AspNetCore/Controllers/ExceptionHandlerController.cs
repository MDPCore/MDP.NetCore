using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MDP.AspNetCore
{
    public class ExceptionHandlerController : Controller
    {
        // Fields
        private readonly IHostEnvironment _environment;

        private readonly ILogger _logger;


        // Constructors
        public ExceptionHandlerController(IHostEnvironment environment, ILogger<ExceptionHandlerController> logger)
        {
            #region Contracts

            if (environment == null) throw new ArgumentException($"{nameof(environment)}=null");
            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");

            #endregion

            // Default
            _environment = environment;
            _logger = logger;
        }


        // Methods
        [AllowAnonymous]
        [Route("/Error", Name="Error")]
        public ActionResult Error()
        {
            // Require
            if (this.HttpContext.Response.HasStarted == true) throw new InvalidProgramException($"{nameof(this.HttpContext.Response.HasStarted)}=true");

            // ExceptionHandler
            var exceptionHandler = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionHandler == null) throw new InvalidOperationException($"{nameof(exceptionHandler)}=null");

            // Exception
            var exception = this.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (exception == null) throw new InvalidOperationException($"{nameof(exception)}=null");

            // Title
            var title = exception.GetType().FullName + ": " + exception.Message;

            // Detail
            var detail = exception.StackTrace;
            if (_environment.IsProduction() == true) detail = null;

            // Instance 
            var instance = exceptionHandler.Path;
            if (string.IsNullOrEmpty(instance) == true) throw new InvalidOperationException($"{nameof(instance)}=null");

            // ProblemDetails
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                this.HttpContext,
                title: title,
                detail: detail,
                instance: instance
            );

            // Log
            _logger.LogError
            (
                exception,
                string.Empty,
                new Dictionary<string, string>()
                {

                }
            );

            // Return
            if (this.HttpContext?.Request.HasAccept(new List<string>() { "html" }) == true)
            {
                // View
                return this.View(problemDetails);
            }
            else
            {
                // Problem-RFC7807 
                return this.Problem(problemDetails);
            }
        }
    }
}
