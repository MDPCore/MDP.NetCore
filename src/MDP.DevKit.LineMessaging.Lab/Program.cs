using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MDP.DevKit.LineMessaging.Lab
{
    public class Program
    {
        // Methods
        public static async Task Run(LineMessageContext lineMessageContext)
        {
            #region Contracts

            if (lineMessageContext == null) throw new ArgumentException($"{nameof(lineMessageContext)}=null");

            #endregion

            // Run
            await RunUserService(lineMessageContext);
            await RunMessageService(lineMessageContext);
        }

        public static async Task RunUserService(LineMessageContext lineMessageContext)
        {
            #region Contracts

            if (lineMessageContext == null) throw new ArgumentException($"{nameof(lineMessageContext)}=null");

            #endregion

            // Variables
            var userId = "U89f2966454791597236a20f676e989dc";

            // Execute
            var result = await lineMessageContext.UserService.GetProfileAsync(userId);

            // Display
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }));
        }

        public static async Task RunMessageService(LineMessageContext lineMessageContext)
        {
            #region Contracts

            if (lineMessageContext == null) throw new ArgumentException($"{nameof(lineMessageContext)}=null");

            #endregion

            // Variables
            var userId = "U89f2966454791597236a20f676e989dc";
            var senderName = "Clark";
            var imageUrl = @"https://sprofile.line-scdn.net/0hwTIGPma7KHtkPj653TZWBBRuKxFHT3FpQQpgSQQ-fh9fCD0sSlg1HwNrdEpRWz0rTwtgG1ZpdBhoLV8demjUT2MOdkxdCWguS1xjnQ";
            var videoUrl = @"https://file-examples.com/storage/fefb234bc0648a3e7a1a47d/2017/04/file_example_MP4_480_1_5MG.mp4";
            var audioUrl = @"https://file-examples.com/storage/fefb234bc0648a3e7a1a47d/2017/04/file_example_MP4_480_1_5MG.mp4";

            // Sender
            var sender = new Sender();
            sender.Name = senderName;
            sender.IconUrl = imageUrl;

            // Execute
            //var result = await lineMessageContext.MessageService.SendMessageAsync(userId, new TextMessage() { Text = "Hello World", Sender = sender });
            //var result = await lineMessageContext.MessageService.SendMessageAsync(userId, new StickerMessage() { PackageId = 1, StickerId = 109, Sender = sender });
            //var result = await lineMessageContext.MessageService.SendMessageAsync(userId, new ImageMessage() { OriginalContentUrl = imageUrl, PreviewImageUrl = imageUrl, Sender = sender });
            //var result = await lineMessageContext.MessageService.SendMessageAsync(userId, new VideoMessage() { OriginalContentUrl = videoUrl, PreviewImageUrl = imageUrl, Sender = sender });
            //var result = await lineMessageContext.MessageService.SendMessageAsync(userId, new AudioMessage() { OriginalContentUrl = audioUrl, Duration = 27000, Sender = sender });
            var result = await lineMessageContext.MessageService.SendMessageAsync(userId, new LocationMessage() { Title = "my location", Address = "1-6-1 Yotsuya, Shinjuku-ku, Tokyo, 160-0004, Japan", Latitude = 35.687574, Longitude = 139.72922, Sender = sender });

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
