using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging.Accesses
{
    [MDP.Registration.Service<UserService>()]
    public partial class RestUserService : RestBaseService, UserService
    {
        // Constructors
        public RestUserService(RestClientFactory restClientFactory) : base(restClientFactory) { }


        // Methods
        public async Task<User?> GetProfileAsync(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException($"{nameof(userId)}=null");

            #endregion

            // RequestUrl
            var requestUri = $"/profile/{userId}";

            // RequestContent
            {

            }

            // ResultFactory
            var resultFactory = (JsonElement resultElement) => 
            {
                // Result
                var result = new User
                {
                    UserId = resultElement.GetProperty<string>("userId") ?? string.Empty,
                    Name  = resultElement.GetProperty<string>("displayName") ?? string.Empty,
                    Mail  = string.Empty,
                    Phone  = string.Empty,
                    PictureUrl  = resultElement.GetProperty<string>("pictureUrl") ?? string.Empty,
                    Language = resultElement.GetProperty<string>("language") ?? string.Empty,
                    StatusMessage = resultElement.GetProperty<string>("statusMessage") ?? string.Empty,
                };

                // Return
                return result;
            };

            // Execute
            try
            {
                // GetAsync
                var resultModel = await this.GetAsync<User>(requestUri, resultFactory: resultFactory);
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                // Return
                return resultModel;
            }
            catch (LineMessageException exception) when (exception.Message == "Not found")
            {
                // Return
                return null;
            }
        }
    }
}
