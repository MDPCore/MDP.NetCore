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
        private readonly ModelRepository _modelRepository;


        // Constructors
        public OpenAIContext(ModelRepository modelRepository)
        {
            #region Contracts

            if (modelRepository == null) throw new ArgumentException($"{nameof(modelRepository)}=null");

            #endregion

            // Default
            _modelRepository = modelRepository;
        }


        // Properties
        public ModelRepository ModelRepository { get { return _modelRepository; } }
    }
}
