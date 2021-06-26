using CLK.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    public class ProgramService<TProgram> : BackgroundService where TProgram : class
    {
        // Fields
        private readonly IServiceProvider _serviceProvider = null;

        private readonly IHostApplicationLifetime _hostApplicationLifetime = null;

        private readonly string _methodName = null;


        // Constructors
        public ProgramService(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime, string methodName = "Run")
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException(nameof(serviceProvider));
            if (hostApplicationLifetime == null) throw new ArgumentException(nameof(hostApplicationLifetime));
            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

            // Default
            _serviceProvider = serviceProvider;
            _hostApplicationLifetime = hostApplicationLifetime;
            _methodName = methodName;
        }


        // Methods
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Execute
            return Task.Run(() =>
            {
                // Run
                this.ExecuteMethod(_methodName);

                // End
                _hostApplicationLifetime.StopApplication();
            });
        }

        private void ExecuteMethod(string methodName, Dictionary<string, object> parameters = null) 
        {
            #region Contracts

            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

            // ExecuteMethod
            this.ExecuteMethod<object>(methodName, parameters);
        }

        private TResultType ExecuteMethod<TResultType>(string methodName, Dictionary<string, object> parameters = null) 
        {
            #region Contracts

            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

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
            return (TResultType)methodInfo.Invoke
            (
                obj: instance,
                parameters: parameterValueArray,
                invokeAttr: BindingFlags.DoNotWrapExceptions,
                binder: null,
                culture: null
            );
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

                // ParameterValue
                {
                    // IServiceProvider
                    if (parameterInfo.ParameterType == typeof(IServiceProvider)) { parameterValueArray[i] = _serviceProvider; continue; }

                    // Parameters
                    if (parameters.ContainsKey(parameterInfo.Name) == true) { parameterValueArray[i] = parameters[parameterInfo.Name]; continue; }

                    // Injection
                    parameterValueArray[i] = _serviceProvider.GetService(parameterInfo.ParameterType); continue;
                }
            }

            // Return
            return parameterValueArray;
        }
    }
}
