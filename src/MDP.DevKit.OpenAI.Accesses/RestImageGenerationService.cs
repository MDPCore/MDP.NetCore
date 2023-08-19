using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Accesses
{
    [MDP.Registration.Service<ImageGenerationService>()]
    public partial class RestImageGenerationService : RestBaseService, ImageGenerationService
    {
        // Constructors
        public RestImageGenerationService(RestClientFactory restClientFactory) : base(restClientFactory) { }
    }

    public partial class RestImageGenerationService : ImageGenerationService
    {
        // Methods
        public async Task<ImageGeneration> CreateAsync(string prompt, int n = 1, string size = "1024x1024")
        {
            #region Contracts

            if (string.IsNullOrEmpty(prompt) == true) throw new ArgumentException($"{nameof(prompt)}=null");

            #endregion

            // Send
            var resultModel = await this.PostAsync<ImageGenerationResultModel>($"/v1/images/generations", content: new
            {
                prompt = prompt,
                n = n,
                size = size,
                response_format = "b64_json",
            });
            if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

            // Result
            var imageGeneration = resultModel.ToImageGeneration();
            if (imageGeneration == null) throw new InvalidOperationException($"{nameof(imageGeneration)}=null");

            // Return
            return imageGeneration;
        }


        // Class
        private class ImageGenerationResultModel
        {
            // Properties
            public List<ImageMessageResultModel> data { get; set; } = null;


            // Methods
            public ImageGeneration ToImageGeneration()
            {
                // Create
                var imageGeneration = new ImageGeneration()
                {
                    Data = this.data?.Select(o => o.ToImageMessage()).ToList() ?? new List<ImageMessage>()
                };

                // Return
                return imageGeneration;
            }
        }

        private class ImageMessageResultModel
        {
            // Properties
            public string b64_json { get; set; } = string.Empty;


            // Methods
            public ImageMessage ToImageMessage()
            {
                // ImageStream
                MemoryStream imageStream = null;
                if (string.IsNullOrEmpty(this.b64_json) == false)
                {
                    // ImageBytes
                    var imageBytes = Convert.FromBase64String(this.b64_json);

                    // ImageStream
                    imageStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    imageStream.Position = 0;
                }
                
                // Create
                var imageMessage = new ImageMessage()
                {
                    Image = imageStream
                };

                // Return
                return imageMessage;
            }
        }
    }
}
