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

        private readonly CompletionService _completionService;


        // Constructors
        public OpenAIContext(ModelService modelService, CompletionService completionService)
        {
            #region Contracts

            if (modelService == null) throw new ArgumentException($"{nameof(modelService)}=null");
            if (completionService == null) throw new ArgumentException($"{nameof(completionService)}=null");

            #endregion

            // Default
            _modelService = modelService;
            _completionService = completionService;
        }


        // Properties
        public ModelService ModelService { get { return _modelService; } }

        public CompletionService CompletionService { get { return _completionService; } }
    }
}
