using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    [MDP.Registration.Service<ModelRepository>()]
    public partial class RestModelRepository : ModelRepository
    {
        // Fields
        private readonly RestClientFactory _restClientFactory;


        // Constructors
        public RestModelRepository(RestClientFactory restClientFactory)
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
            public string message { get; set; } = string.Empty;

            public string type { get; set; } = string.Empty;

            public string param { get; set; } = string.Empty;

            public string code { get; set; } = string.Empty;
        }

        public class ModelResultModel
        {
            // Properties
            public string id { get; set; } = string.Empty;

            public string owned_by { get; set; } = string.Empty;

            public long created { get; set; } = 0;


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

    public partial class RestModelRepository : ModelRepository
    {
        // Methods
        public List<Model> FindAll()
        {
            // RestClient
            using (var restClient = _restClientFactory.CreateClient("OpenAIService"))
            {
                // ResultModel
                var resultModel = restClient.GetAsync<FindAllResultModel>("/v1/models").Result;
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

    public partial class RestModelRepository : ModelRepository
    {
        // Methods
        public Model? FindByModelId(string modelId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(modelId) == true) throw new ArgumentException($"{nameof(modelId)}=null");

            #endregion

            // RestClient
            using (var restClient = _restClientFactory.CreateClient("OpenAIService"))
            {
                // ResultModel
                var resultModel = restClient.GetAsync<ModelResultModel>($"/v1/models/{modelId}").Result;
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                // Result
                var model = resultModel.ToModel();
                if (model == null) return null;

                // Return
                return model;
            }
        }
    }
}
