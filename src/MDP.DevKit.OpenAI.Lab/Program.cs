using System.Text.Json;

namespace MDP.DevKit.OpenAI.Lab
{
    public class Program
    {
        // Methods
        public static void Run(OpenAIContext openAIContext)
        {
            #region Contracts

            if (openAIContext == null) throw new ArgumentException($"{nameof(openAIContext)}=null");

            #endregion

            // Execute
            var result = openAIContext.ModelRepository.FindAll();
            //var result = openAIContext.ModelRepository.FindByModelId("text-davinci-003");

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
