using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI
{
    [MDP.Registration.Service<OpenAIContext>(singleton: true)]
    public class OpenAIContext
    {
        // Fields
        private readonly ModelService _modelService;

        private readonly TextEmbeddingService _textEmbeddingService;

        private readonly TextCompletionService _textCompletionService;

        private readonly ChatCompletionService _chatCompletionService;

        private readonly ImageGenerationService _imageGenerationService;


        // Constructors
        public OpenAIContext
        (
            ModelService modelService,
            TextEmbeddingService textEmbeddingService,
            TextCompletionService textCompletionService,
            ChatCompletionService chatCompletionService,
            ImageGenerationService imageGenerationService
        )
        {
            #region Contracts

            if (modelService == null) throw new ArgumentException($"{nameof(modelService)}=null");
            if (textEmbeddingService == null) throw new ArgumentException($"{nameof(textEmbeddingService)}=null");
            if (textCompletionService == null) throw new ArgumentException($"{nameof(textCompletionService)}=null");
            if (chatCompletionService == null) throw new ArgumentException($"{nameof(chatCompletionService)}=null");
            if (imageGenerationService == null) throw new ArgumentException($"{nameof(imageGenerationService)}=null");

            #endregion

            // Default
            _modelService = modelService;
            _textEmbeddingService = textEmbeddingService;
            _textCompletionService = textCompletionService;
            _chatCompletionService = chatCompletionService;
            _imageGenerationService = imageGenerationService;
        }


        // Properties
        public ModelService ModelService { get { return _modelService; } }

        public TextEmbeddingService TextEmbeddingService { get { return _textEmbeddingService; } }

        public TextCompletionService TextCompletionService { get { return _textCompletionService; } }

        public ChatCompletionService ChatCompletionService { get { return _chatCompletionService; } }

        public ImageGenerationService ImageGenerationService { get { return _imageGenerationService; } }

        public CosineSimilarityService CosineSimilarityService { get; } = new CosineSimilarityService();
    }
}
