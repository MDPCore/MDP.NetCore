using System.IO;
using System.Text;

namespace MDP.DevKit.OpenAI.Lab
{
    public static class KnowledgeBaseExtensions
    {
        // Constants
        public static readonly List<string> DefaultKnowledgePointList = new List<string>()
        {
            "B2: 美食生活館",
            "B1: 繽紛流行館",
            "1F: 國際美妝館",
            "2F: 流行配件館",
            "3F: 時尚粉領館",
            "4F: 潮流運動館",
            "5F: 都會休閒館"
        };


        // Methods
        public static async Task<dynamic> CreateAnswerAsync(this OpenAIContext openAIContext, string question, List<string>? knowledgePointList = null, float temperature = 0, int maxTokens = 200)
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
            var choicePointList = similarityPointList.Where(o => o.similarityScore >= 0.8).Take(3).ToList();

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
