using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    [MDP.Registration.Service<ChatCompletionService>()]
    public partial class RestChatCompletionService : RestBaseService, ChatCompletionService
    {
        // Constructors
        public RestChatCompletionService(RestClientFactory restClientFactory) : base(restClientFactory) { }
    }

    public partial class RestChatCompletionService : ChatCompletionService
    {
        // Methods
        public async Task<ChatCompletion> CreateAsync(List<ChatMessage> messages, string model = "gpt-3.5-turbo", float temperature = 1, int maxTokens = 16)
        {
            #region Contracts

            if (messages == null) throw new ArgumentException($"{nameof(messages)}=null");
            if (string.IsNullOrEmpty(model) == true) throw new ArgumentException($"{nameof(model)}=null");

            #endregion

            // Send
            var resultModel = await this.PostAsync<ChatCompletionResultModel>($"/v1/chat/completions", content: new
            {
                messages = messages.Select(o=> ChatMessageActionModel.ToActionModel(o)).ToList(),
                model = model,
                temperature = temperature,
                max_tokens = maxTokens,
            });
            if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

            // Result
            var chatCompletion = resultModel.ToChatCompletion();
            if (chatCompletion == null) throw new InvalidOperationException($"{nameof(chatCompletion)}=null");

            // Return
            return chatCompletion;
        }


        // Class
        private class ChatMessageActionModel
        {
            // Properties
            public string role { get; set; } = string.Empty;

            public string content { get; set; } = string.Empty;


            // Methods
            public static ChatMessageActionModel ToActionModel(ChatMessage chatMessage)
            {
                // Create
                var actionModel = new ChatMessageActionModel()
                {
                    role = chatMessage.Role,
                    content = chatMessage.Content,
                };

                // Return
                return actionModel;
            }
        }

        private class ChatCompletionResultModel
        {
            // Properties
            public string id { get; set; } = string.Empty;

            public string model { get; set; } = string.Empty;

            public List<ChatMessageChoiceResultModel> choices { get; set; } = null;

            public CompletionUsageResultModel usage { get; set; } = null;


            // Methods
            public ChatCompletion ToChatCompletion()
            {
                // Create
                var chatCompletion = new ChatCompletion()
                {
                    Id = this.id,
                    Model = this.model,
                    Choices = choices?.Select(o => o.ToChatMessageChoice()).ToList() ?? new List<ChatMessageChoice>(),
                    Usage = usage?.ToCompletionUsage(),
                };

                // Return
                return chatCompletion;
            }
        }

        private class ChatMessageChoiceResultModel
        {
            // Properties
            public Message message { get; set; }

            public int index { get; set; } = 0;

            public string finish_reason { get; set; } = string.Empty;


            // Methods
            public ChatMessageChoice ToChatMessageChoice()
            {
                // Create
                var chatMessageChoice = new ChatMessageChoice()
                {
                    Role = this.message?.role ?? string.Empty,
                    Content = this.message?.content ?? string.Empty,
                    Index = this.index,
                    FinishReason = this.finish_reason,
                };

                // Return
                return chatMessageChoice;
            }


            // Class
            public class Message
            {
                // Properties
                public string role { get; set; } = string.Empty;

                public string content { get; set; } = string.Empty;
            }
        }
    }
}
