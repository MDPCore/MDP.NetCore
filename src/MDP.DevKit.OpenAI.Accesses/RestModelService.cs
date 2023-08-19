using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    [MDP.Registration.Service<ModelService>()]
    public partial class RestModelService : RestBaseService, ModelService
    {
        // Constructors
        public RestModelService(RestClientFactory restClientFactory) : base(restClientFactory) { }


        // Class
        private class ModelResultModel
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
                    Id = this.id,
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
            // Send
            var resultModel = await this.GetAsync<FindAllResultModel>("/v1/models");
            if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

            // Result
            var modelList = resultModel.data?.Select(o => o.ToModel()).ToList();
            if (modelList == null) throw new InvalidOperationException($"{nameof(modelList)}=null");

            // Return
            return modelList;
        }


        // Class
        private class FindAllResultModel
        {
            // Properties
            public List<ModelResultModel> data { get; set; } = null;       
        }
    }

    public partial class RestModelService : ModelService
    {
        // Methods
        public async Task<Model> FindByIdAsync(string model)
        {
            #region Contracts

            if (string.IsNullOrEmpty(model) == true) throw new ArgumentException($"{nameof(model)}=null");

            #endregion

            // Execute
            try
            {
                // Send
                var resultModel = await this.GetAsync<ModelResultModel>($"/v1/models/{model}");
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                // Result
                var modelObject = resultModel.ToModel();
                if (modelObject == null) return null;

                // Return
                return modelObject;
            }
            catch (OpenAIException exception) when (exception.Code == "model_not_found")
            {
                // Return
                return null;
            }
        }
    }
}
