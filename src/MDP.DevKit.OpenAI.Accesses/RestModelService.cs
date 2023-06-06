using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    [MDP.Registration.Service<ModelService>()]
    public partial class RestModelService : ModelService
    {
        // Fields
        private readonly RestClientFactory _restClientFactory;


        // Constructors
        public RestModelService(RestClientFactory restClientFactory)
        {
            #region Contracts

            if (restClientFactory == null) throw new ArgumentException($"{nameof(restClientFactory)}=null");

            #endregion

            // Default
            _restClientFactory = restClientFactory;
        }


        // Class
        public class ErrorModel
        {
            // Properties
            public Error? error { get; set; } = null;


            // Class
            public class Error
            {
                // Properties
                public string message { get; set; } = string.Empty;

                public string type { get; set; } = string.Empty;

                public string param { get; set; } = string.Empty;

                public string code { get; set; } = string.Empty;
            }
        }

        public class ModelResultModel
        {
            // Properties
            public string id { get; set; } = string.Empty;

            public string owned_by { get; set; } = string.Empty;


            // Methods
            public Model ToModel()
            {
                // Create
                var model = new Model()
                {
                    ModelId = this.id,
                    Owner = this.owned_by,
                };

                // Return
                return model;
            }
        }
    }

    public partial class RestModelService : ModelService
    {
        // Methods
        public async Task<List<Model>> FindAllAsync()
        {
            // RestClient
            using (var restClient = _restClientFactory.CreateClient("OpenAIService"))
            {
                // Send
                var resultModel = await restClient.GetAsync<FindAllResultModel, ErrorModel>("/v1/models");
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                // Result
                var modelList = resultModel.data?.Select(o => o.ToModel()).ToList();
                if (modelList == null) throw new InvalidOperationException($"{nameof(modelList)}=null");

                // Return
                return modelList;
            }
        }

        // Class
        public class FindAllResultModel
        {
            // Properties
            public List<ModelResultModel>? data { get; set; } = null;       
        }
    }

    public partial class RestModelService : ModelService
    {
        // Methods
        public async Task<Model?> FindByIdAsync(string modelId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(modelId) == true) throw new ArgumentException($"{nameof(modelId)}=null");

            #endregion

            // Execute
            try
            {
                // RestClient
                using (var restClient = _restClientFactory.CreateClient("OpenAIService"))
                {
                    // Send
                    var resultModel = await restClient.GetAsync<ModelResultModel, ErrorModel>($"/v1/models/{modelId}");
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Result
                    var model = resultModel.ToModel();
                    if (model == null) return null;

                    // Return
                    return model;
                }
            }
            catch (RestResponseException<ErrorModel> responseException) when (responseException.Model?.error?.code == "model_not_found")
            {
                // Return
                return null;
            }
        }
    }
}
