using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    [MDP.Registration.Service<TextEmbeddingService>()]
    public partial class RestTextEmbeddingService : RestBaseService, TextEmbeddingService
    {
        // Constructors
        public RestTextEmbeddingService(RestClientFactory restClientFactory) : base(restClientFactory) { }
    }

    public partial class RestTextEmbeddingService : TextEmbeddingService
    {
        // Methods
        public async Task<TextEmbedding> CreateAsync(string prompt, string model = "text-embedding-ada-002")
        {
            #region Contracts

            if (string.IsNullOrEmpty(prompt) == true) throw new ArgumentException($"{nameof(prompt)}=null");
            if (string.IsNullOrEmpty(model) == true) throw new ArgumentException($"{nameof(model)}=null");

            #endregion

            // Send
            var resultModel = await this.PostAsync<TextEmbeddingResultModel>($"/v1/embeddings", content: new
            {
                input = prompt,
                model = model
            });
            if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

            // Result
            var textEmbedding = resultModel.ToTextEmbedding();
            if (textEmbedding == null) throw new InvalidOperationException($"{nameof(textEmbedding)}=null");

            // Return
            return textEmbedding;
        }


        // Class
        private class TextEmbeddingResultModel
        {
            // Properties
            public string model { get; set; } = string.Empty;

            public List<EmbeddingVectorResultModel> data { get; set; } = null;

            public EmbeddingUsageResultModel usage { get; set; } = null;


            // Methods
            public TextEmbedding ToTextEmbedding()
            {
                // Create
                var textEmbedding = new TextEmbedding()
                {
                    Model = this.model,
                    Data = this.data?.Select(o => o.ToEmbeddingVector()).ToList() ?? new List<EmbeddingVector>(),
                    Usage = this.usage?.ToEmbeddingUsage(),
                };

                // Return
                return textEmbedding;
            }
        }

        private class EmbeddingVectorResultModel
        {
            // Properties
            public int index { get; set; } = 0;

            public double[] embedding { get; set; } = new double[0];


            // Methods
            public EmbeddingVector ToEmbeddingVector()
            {
                // Create
                var embeddingVector = new EmbeddingVector()
                {
                    Vector = this.embedding ?? new double[0],
                };

                // Return
                return embeddingVector;
            }
        }
    }
}
