using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Lab
{
    public class Program
    {
        // Methods
        public static async Task Run(OpenAIContext openAIContext)
        {
            #region Contracts

            if (openAIContext == null) throw new ArgumentException($"{nameof(openAIContext)}=null");

            #endregion

            // Execute
            //var result = await openAIContext.ModelService.FindAllAsync();
            //var result = await openAIContext.ModelService.FindByIdAsync("text-davinci-0035");
            //var result = await openAIContext.TextEmbeddingService.CreateAsync("The food was delicious and the waiter...");
            //var result = await openAIContext.TextCompletionService.CreateAsync("Say this is a test");
            //var result = await openAIContext.ChatCompletionService.CreateAsync(new List<ChatMessage>() { new ChatMessage() { Role= "user", Content= "Hello!" } });
            //var result = await openAIContext.ImageGenerationService.CreateAsync("A cute baby sea otter").WriteToAsync(@"bin\output\{0}.png");
            //var result = await openAIContext.TextEmbeddingService.CreateAsync("The food was delicious and the waiter...");
            var result = await openAIContext.CreateAnswerAsync("我想喝綠豆湯該去哪一樓?");

            // Display
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }));
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
