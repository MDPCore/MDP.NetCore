using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    [MDP.Registration.Service<TextCompletionService>()]
    public partial class RestTextCompletionService : RestBaseService, TextCompletionService
    {
        // Constructors
        public RestTextCompletionService(RestClientFactory restClientFactory) : base(restClientFactory) { }
    }

    public partial class RestTextCompletionService : TextCompletionService
    {
        // Methods
        public async Task<TextCompletion> CreateAsync(string prompt, string model = "text-davinci-003", float temperature = 1, int maxTokens = 16)
        {
            #region Contracts

            if (string.IsNullOrEmpty(prompt) == true) throw new ArgumentException($"{nameof(prompt)}=null");
            if (string.IsNullOrEmpty(model) == true) throw new ArgumentException($"{nameof(model)}=null");

            #endregion

            // Send
            var resultModel = await this.PostAsync<TextCompletionResultModel>($"/v1/completions", content: new
            {
                prompt = prompt,
                model = model,
                temperature = temperature,
                max_tokens = maxTokens,
            });
            if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

            // Result
            var textCompletion = resultModel.ToTextCompletion();
            if (textCompletion == null) throw new InvalidOperationException($"{nameof(textCompletion)}=null");

            // Return
            return textCompletion;
        }


        // Class
        private class TextCompletionResultModel
        {
            // Properties
            public string id { get; set; } = string.Empty;

            public string model { get; set; } = string.Empty;

            public List<TextMessageChoiceResultModel> choices { get; set; } = null;

            public CompletionUsageResultModel usage { get; set; } = null;


            // Methods
            public TextCompletion ToTextCompletion()
            {
                // Create
                var textCompletion = new TextCompletion()
                {
                    Id = this.id,
                    Model = this.model,
                    Choices = this.choices?.Select(o => o.ToTextMessageChoice()).ToList() ?? new List<TextMessageChoice>(),
                    Usage = this.usage?.ToCompletionUsage(),
                };

                // Return
                return textCompletion;
            }
        }

        private class TextMessageChoiceResultModel
        {
            // Properties
            public string text { get; set; } = string.Empty;

            public int index { get; set; } = 0;

            public string finish_reason { get; set; } = string.Empty;


            // Methods
            public TextMessageChoice ToTextMessageChoice()
            {
                // Create
                var textMessageChoice = new TextMessageChoice()
                {
                    Text = this.text ?? string.Empty,
                    Index = this.index,
                    FinishReason = this.finish_reason,
                };

                // Return
                return textMessageChoice;
            }
        }
    }
}
