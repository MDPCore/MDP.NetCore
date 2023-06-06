using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MDP.DevKit.OpenAI.RestModelService;

namespace MDP.DevKit.OpenAI
{
    [MDP.Registration.Service<CompletionService>()]
    public partial class RestCompletionService : CompletionService
    {
        // Fields
        private readonly RestClientFactory _restClientFactory;


        // Constructors
        public RestCompletionService(RestClientFactory restClientFactory)
        {
            #region Contracts

            if (restClientFactory == null) throw new ArgumentException($"{nameof(restClientFactory)}=null");

            #endregion

            // Default
            _restClientFactory = restClientFactory;
        }


        // Class
        public class ErrorCompletion
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

       
    }

    public partial class RestCompletionService : CompletionService
    {
        // Methods
        public async Task<Completion> CreateAsync(string model, string prompt, float temperature = 1, int maxTokens = 16)
        {
            #region Contracts

            if (string.IsNullOrEmpty(model) == true) throw new ArgumentException($"{nameof(model)}=null");
            if (string.IsNullOrEmpty(prompt) == true) throw new ArgumentException($"{nameof(prompt)}=null");

            #endregion

            // RestClient
            using (var restClient = _restClientFactory.CreateClient("OpenAIService"))
            {
                // Send
                var resultModel = await restClient.PostAsync<CreateResultModel, ErrorCompletion>($"/v1/completions", content: new {
                    model= model,
                    prompt= prompt,
                    temperature = temperature,
                    maxTokens = maxTokens,
                });
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                // Result
                var completion = resultModel.ToCompletion();
                if (completion == null) throw new InvalidOperationException($"{nameof(completion)}=null");

                // Return
                return completion;
            }
        }


        // Class
        public class CreateResultModel
        {
            // Properties
            public string id { get; set; } = string.Empty;

            public string model { get; set; } = string.Empty;

            public List<ChoiceResultModel>? choices { get; set; } = null;

            public UsageResultModel? usage { get; set; } = null;


            // Methods
            public Completion ToCompletion()
            {
                // Create
                var completion = new Completion()
                {
                  
                };

                // Return
                return completion;
            }


            // Class
            public class ChoiceResultModel
            {
                // Properties
                public string text { get; set; } = string.Empty;

                public int index { get; set; } = 0;

                public int? logprobs { get; set; } = null;

                public string finish_reason { get; set; } = string.Empty;
            }

            public class UsageResultModel
            {
                // Properties
                public int prompt_tokens { get; set; } = 0;

                public int completion_tokens { get; set; } = 0;

                public int total_tokens { get; set; } = 0;
            }
        }
    }
}
