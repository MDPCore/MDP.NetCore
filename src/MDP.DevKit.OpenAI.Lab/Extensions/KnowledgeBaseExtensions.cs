using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Lab
{
    public static class KnowledgeBaseExtensions
    {
        // Constants
        public static readonly List<string> DefaultKnowledgePointList = new List<string>()
        {
            "B2：美食生活館 - 各種美食餐廳、烘焙店、糕點店、特色咖啡館，以及食品超市，或是售賣烹飪器具、餐具等生活用品店。",
            "B1：繽紛流行館 - 各式各樣的服飾、飾品、流行產品店，如快時尚品牌、流行的配飾店、潮流玩具或科技產品。",
            "1F：國際美妝館 - 各種美妝商品，如國際品牌的化妝品、護膚品、香水等。",
            "2F：流行配件館 - 各式配件的店，如手錶、眼鏡、首飾、帽子、包包等。",
            "3F：時尚白領館 - 各種專為上班族、專業人士所設計的服裝和配件，如正裝、商務包、時尚辦公用品等。",
            "4F：潮流運動館 - 各種運動相關商品，如運動服裝、運動鞋、健身器材，以及運動配件等。",
            "5F：都會休閒館 - 休閒娛樂的地方，如書店、音樂CD店、咖啡館，或是賣休閒服裝、戶外用品的店等。",
        };


        // Methods
        public static async Task<dynamic> CreateAnswerAsync(this OpenAIContext openAIContext, string question, List<string> knowledgePointList = null, float temperature = 0, int maxTokens = 200)
        {
            #region Contracts

            if (openAIContext == null) throw new ArgumentException(nameof(openAIContext));
            if (string.IsNullOrEmpty(question) == true) throw new ArgumentException(nameof(question));

            #endregion

            // Question
            var questionVector = (await openAIContext.TextEmbeddingService.CreateAsync(question)).Data![0]!.Vector;

            // KnowledgePointList
            knowledgePointList = knowledgePointList ?? DefaultKnowledgePointList;

            // SimilarityPointList
            var similarityPointList = new List<dynamic>();
            foreach (var knowledgePoint in knowledgePointList)
            {
                var similarityScore = openAIContext.CosineSimilarityService.Calculate
                (
                   questionVector,
                   (await openAIContext.TextEmbeddingService.CreateAsync(knowledgePoint)).Data![0]!.Vector
                );

                similarityPointList.Add(new
                {
                    knowledgePoint = knowledgePoint,
                    similarityScore = similarityScore
                });
            }
            similarityPointList = similarityPointList.OrderByDescending(o => o.similarityScore).ToList();

            // ChoicePointList
            var choicePointList = similarityPointList.Where(o => o.similarityScore >= 0.7).Take(3).ToList();

            // Prompt
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine($"客戶問題:");
            promptBuilder.AppendLine($"# {question}");
            promptBuilder.AppendLine($"可能答案:");
            {
                if (choicePointList.Count > 0)
                {
                    choicePointList.ForEach(o => promptBuilder.AppendLine($"# {o.knowledgePoint} (關聯度：{o.similarityScore})"));
                }
                else
                {
                    promptBuilder.AppendLine($"# 回答不知道");
                }
            }            
            promptBuilder.AppendLine($"基於上面的內容，你是智能客服機器人，請回答客戶的問題：");

            // KnowledgeAnswer 
            var knowledgeAnswer = await openAIContext.ChatCompletionService.CreateAsync(new List<ChatMessage>()
            {
                new ChatMessage() { Role = "user", Content = $"{promptBuilder}" }
            }, temperature: temperature, maxTokens: maxTokens);

            // Return
            return new
            {
                Question = question,
                Answer = knowledgeAnswer.Choices![0].Content,
                Prompt = promptBuilder.ToString(),
                Knowledge = similarityPointList.Select(o => $"{o.knowledgePoint} {o.similarityScore}"),
            };
        }
    }
}
