using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MDP.DevKit.LineMessaging.Lab
{
    public partial class HomeController : Controller
    {
        // Fields                
        private readonly LineMessageContext _lineMessageContext;


        // Constructors
        public HomeController(LineMessageContext lineMessageContext)
        {
            #region Contracts

            if (lineMessageContext == null) throw new ArgumentException($"{nameof(lineMessageContext)}=null");

            #endregion

            // Default
            _lineMessageContext = lineMessageContext;
        }
    }

    public partial class HomeController : Controller
    {
        // Methods
        public async Task<ActionResult> Index()
        {
            // Run
            await RunUserService();
            await RunMessageService();

            // Return
            return View();
        }

        public async Task RunUserService()
        {
            // Variables
            var userId = "U89f2966454791597236a20f676e989dc";

            // Execute
            var result = await _lineMessageContext.UserService.GetProfileAsync(userId);

            // Display
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }));
        }

        public async Task RunMessageService()
        {
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

            // PushMessageAsync
            //var result = await _lineMessageContext.MessageService.PushMessageAsync(new TextMessage() { Text = "Hello World", Sender = sender }, userId);
            //var result = await _lineMessageContext.MessageService.PushMessageAsync(new StickerMessage() { PackageId = 1, StickerId = 109, Sender = sender }, userId);
            //var result = await _lineMessageContext.MessageService.PushMessageAsync(new ImageMessage() { OriginalContentUrl = imageUrl, PreviewImageUrl = imageUrl, Sender = sender }, userId);
            //var result = await _lineMessageContext.MessageService.PushMessageAsync(new VideoMessage() { OriginalContentUrl = videoUrl, PreviewImageUrl = imageUrl, Sender = sender }, userId);
            //var result = await _lineMessageContext.MessageService.PushMessageAsync(new AudioMessage() { OriginalContentUrl = audioUrl, Duration = 27000, Sender = sender }, userId);
            var result = await _lineMessageContext.MessageService.PushMessageAsync(new LocationMessage() { Title = "my location", Address = "1-6-1 Yotsuya, Shinjuku-ku, Tokyo, 160-0004, Japan", Latitude = 35.687574, Longitude = 139.72922, Sender = sender }, userId);

            // MulticastMessageAsync
            //var result = await _lineMessageContext.MessageService.MulticastMessageAsync(new TextMessage() { Text = "Hello World", Sender = sender }, new List<string>() { userId });
            //var result = await _lineMessageContext.MessageService.MulticastMessageAsync(new LocationMessage() { Title = "my location", Address = "1-6-1 Yotsuya, Shinjuku-ku, Tokyo, 160-0004, Japan", Latitude = 35.687574, Longitude = 139.72922, Sender = sender }, new List<string>() { userId });

            // Display
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }));
        }
    }
}
