using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging.Accesses
{
    public abstract class RestBaseService
    {
        // Fields
        private readonly RestClientFactory _restClientFactory;


        // Constructors
        internal RestBaseService(RestClientFactory restClientFactory)
        {
            #region Contracts

            if (restClientFactory == null) throw new ArgumentException($"{nameof(restClientFactory)}=null");

            #endregion

            // Default
            _restClientFactory = restClientFactory;
        }


        // Methods
        protected async Task<TResultModel> GetAsync<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Execute
            try
            {
                // RestClient
                using (var restClient = _restClientFactory.CreateClient("MDP.DevKit.LineMessaging", "LineMessageService"))
                {
                    // Send
                    var resultModel = await restClient.GetAsync<TResultModel, ErrorModel>(requestUri, headers, query, content, resultFactory);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
            }
            catch (RestException<ErrorModel> restException)
            {
                // ResponseException
                var responseException = restException.ErrorModel?.ToException();
                if (responseException != null) throw responseException;

                // Exception
                throw;
            }
        }

        protected async Task<TResultModel> PostAsync<TResultModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null, Func<JsonElement, TResultModel>? resultFactory = null)
            where TResultModel : class
        {
            // Execute
            try
            {
                // RestClient
                using (var restClient = _restClientFactory.CreateClient("MDP.DevKit.LineMessaging", "LineMessageService"))
                {
                    // Send
                    var resultModel = await restClient.PostAsync<TResultModel, ErrorModel>(requestUri, headers, query, content, resultFactory);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
            }
            catch (RestException<ErrorModel> restException)
            {
                // ResponseException
                var responseException = restException.ErrorModel?.ToException();
                if (responseException != null) throw responseException;

                // Exception
                throw;
            }
        }
    }
}
