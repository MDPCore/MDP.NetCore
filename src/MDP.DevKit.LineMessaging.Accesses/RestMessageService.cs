using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging.Accesses
{
    [MDP.Registration.Service<MessageService>()]
    public partial class RestMessageService : RestBaseService, MessageService
    {
        // Constructors
        public RestMessageService(RestClientFactory restClientFactory) : base(restClientFactory) { }


        // Methods
        public Task<string> SendMessageAsync(string userId, Message message, string? replyToken = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException($"{nameof(userId)}=null");
            if (message == null) throw new ArgumentException($"{nameof(message)}=null");

            #endregion

            // SendMessageAsync
            return this.SendMessageAsync(userId, new List<Message>() { message }, replyToken);
        }

        public async Task<string> SendMessageAsync(string userId, List<Message> messageList, string? replyToken = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException($"{nameof(userId)}=null");
            if (messageList == null) throw new ArgumentException($"{nameof(messageList)}=null");

            #endregion

            // Messages
            var messages = messageList.Select(message => this.SerializeMessages(message)).ToList();
            if (messages.Count == 0) throw new InvalidOperationException($"{nameof(messages)}.Count == 0");
                        
            // RequestUrl
            var requestUri = string.Empty;
            if (replyToken == null) requestUri = @"/message/push";
            if (replyToken != null) requestUri = @"/message/reply";

            // RequestContent
            dynamic requestContent = new ExpandoObject();
            {
                // Destination 
                if (string.IsNullOrEmpty(replyToken) == true)
                {
                    requestContent.to = userId;
                }
                else
                {
                    requestContent.replyToken = replyToken;
                }

                // Message
                requestContent.messages = messages;
            }

            // ResultFactory
            {

            }

            // Execute
            {
                // PostAsync
                var resultModel = await this.PostAsync<string>(requestUri, content: requestContent);
                if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                // Return
                return resultModel;
            }
        }

        private dynamic SerializeMessages(Message message)
        {
            #region Contracts

            if (message == null) throw new ArgumentException($"{nameof(message)}=null");

            #endregion

            // MessageModel
            dynamic messageModel = new ExpandoObject();
            {
                // Sender
                if (message.Sender != null)
                {
                    messageModel.sender = new
                    {
                        name = message.Sender.Name,
                        iconUrl = message.Sender.IconUrl,
                    };
                }
            }

            // TextMessage
            if (message is TextMessage textMessage)
            {
                messageModel.type = "text";
                messageModel.text = textMessage.Text.Replace("'", "\"");
                return messageModel;
            }

            // StickerMessage
            if (message is StickerMessage stickerMessage)
            {
                messageModel.type = "sticker";
                messageModel.packageId = stickerMessage.PackageId;
                messageModel.stickerId = stickerMessage.StickerId;
                return messageModel;
            }

            // ImageMessage
            if (message is ImageMessage imageMessage)
            {
                messageModel.type = "image";
                messageModel.originalContentUrl = imageMessage.OriginalContentUrl;
                messageModel.previewImageUrl = imageMessage.PreviewImageUrl;
                return messageModel;
            }

            // VideoMessage
            if (message is VideoMessage videoMessage)
            {
                messageModel.type = "video";
                messageModel.originalContentUrl = videoMessage.OriginalContentUrl;
                messageModel.previewImageUrl = videoMessage.PreviewImageUrl;
                return messageModel;
            }

            // AudioMessage
            if (message is AudioMessage audioMessage)
            {
                messageModel.type = "audio";
                messageModel.originalContentUrl = audioMessage.OriginalContentUrl;
                messageModel.duration = audioMessage.Duration;
                return messageModel;
            }

            // LocationMessage
            if (message is LocationMessage locationMessage)
            {
                messageModel.type = "location";
                messageModel.title = locationMessage.Title;
                messageModel.address = locationMessage.Address;
                messageModel.latitude = locationMessage.Latitude;
                messageModel.longitude = locationMessage.Longitude;
                return messageModel;
            }

            // Throw
            throw new InvalidOperationException($"Unknown Message Type={message.GetType()}");
        }
    }
}
