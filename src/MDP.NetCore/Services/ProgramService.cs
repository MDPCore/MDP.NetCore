using CLK.Reflection;
using MDP.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    public class ProgramService<TProgram> : BackgroundService where TProgram : class
    {
        // Fields
        private readonly IHostApplicationLifetime _applicationLifetime;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger _logger;

        private readonly string _methodName;


        // Constructors
        public ProgramService(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider,  ILogger<TProgram> logger, string methodName = "Run")
        {
            #region Contracts

            if (applicationLifetime == null) throw new ArgumentException($"{nameof(applicationLifetime)}=null");
            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");
            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");
            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException($"{nameof(methodName)}= null");

            #endregion

            // Default
            _applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _methodName = methodName;
        }


        // Methods
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Run
                await this.ExecuteMethod(_methodName);
            }
            finally
            {
                // End
                _applicationLifetime.StopApplication();
            }
        }

        private async Task ExecuteMethod(string methodName, Dictionary<string, object>? parameters = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

            try
            {
                // Require
                if (parameters == null) parameters = new Dictionary<string, object>();

                // Instance
                var instance = _serviceProvider.GetRequiredService<TProgram>();
                if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

                // MethodInfo
                var methodInfo = typeof(TProgram).FindMethod(methodName);
                if (methodInfo == null) throw new InvalidOperationException($"{nameof(methodInfo)}=null");

                // ParameterValueArray
                var parameterValueArray = this.CreateParameterValueArray(methodInfo.GetParameters(), parameters);
                if (parameterValueArray == null) throw new InvalidOperationException($"{nameof(parameterValueArray)}=null");

                // ExecuteMethod
                var executeResult = methodInfo.Invoke
                (
                    obj: instance,
                    parameters: parameterValueArray,
                    invokeAttr: BindingFlags.DoNotWrapExceptions,
                    binder: null,
                    culture: null
                ) as Task;
                if (executeResult != null) await executeResult;
            }
            catch(Exception exception)
            {
                // Log
                _logger.LogError
                (
                    exception,
                    string.Empty,
                    new Dictionary<string, string>()
                    {

                    }
                );
            }
        }

        private object[] CreateParameterValueArray(ParameterInfo[] parameterInfoArray, Dictionary<string, object> parameters)
        {
            #region Contracts

            if (parameterInfoArray == null) throw new ArgumentException(nameof(parameterInfoArray));
            if (parameters == null) throw new ArgumentException(nameof(parameterInfoArray));

            #endregion

            // ParameterValueArray
            var parameterValueArray = new object[parameterInfoArray.Length];
            for (var i = 0; i < parameterInfoArray.Length; i++)
            {
                // ParameterInfo
                var parameterInfo = parameterInfoArray[i];
                if (parameterInfo == null) throw new InvalidOperationException($"{nameof(parameterInfo)}=null");
                if (parameterInfo.Name == null) throw new InvalidOperationException($"{nameof(parameterInfo.Name)}=null");

                // ParameterValue
                {
                    // IServiceProvider
                    if (parameterInfo.ParameterType == typeof(IServiceProvider)) { parameterValueArray[i] = _serviceProvider; continue; }

                    // Parameters
                    if (parameters.ContainsKey(parameterInfo.Name) == true) { parameterValueArray[i] = parameters[parameterInfo.Name]; continue; }

                    // Injection
                    parameterValueArray[i] = _serviceProvider.GetService(parameterInfo.ParameterType)!; continue;
                }
            }

            // Return
            return parameterValueArray;
        }
    }
}
