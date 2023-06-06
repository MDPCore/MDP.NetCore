using System.Text.Json;

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
            var result = await openAIContext.CompletionService.CreateAsync("text-davinci-003", "Say this is a test");

            // Display
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
