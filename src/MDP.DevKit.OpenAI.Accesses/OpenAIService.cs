using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    public abstract class OpenAIService
    {
        // Fields
        private readonly RestClientFactory _restClientFactory;


        // Constructors
        internal OpenAIService(RestClientFactory restClientFactory)
        {
            #region Contracts

            if (restClientFactory == null) throw new ArgumentException($"{nameof(restClientFactory)}=null");

            #endregion

            // Default
            _restClientFactory = restClientFactory;
        }


        // Methods
        public async Task<TResultModel?> GetAsync<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Execute
            try
            {
                // RestClient
                using (var restClient = _restClientFactory.CreateClient("OpenAIService"))
                {
                    // Send
                    var resultModel = await restClient.GetAsync<TResultModel, TErrorModel>(requestUri, headers, query, content);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
            }
            catch (RestResponseException<ErrorModel> responseException) 
            {
                // OpenAIException
                var openAIException = responseException.Model?.error?.ToException();
                if (openAIException != null) throw openAIException;

                // Exception
                throw;
            }
        }

        public async Task<TResultModel?> PostAsync<TResultModel, TErrorModel>(string? requestUri = null, object? headers = null, object? query = null, object? content = null)
            where TResultModel : class
            where TErrorModel : class
        {
            // Execute
            try
            {
                // RestClient
                using (var restClient = _restClientFactory.CreateClient("OpenAIService"))
                {
                    // Send
                    var resultModel = await restClient.PostAsync<TResultModel, TErrorModel>(requestUri, headers, query, content);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
            }
            catch (RestResponseException<ErrorModel> responseException)
            {
                // OpenAIException
                var openAIException = responseException.Model?.error?.ToException();
                if (openAIException != null) throw openAIException;

                // Exception
                throw;
            }
        }
    }
}
