using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    public interface MessageService
    {
        // Methods
        Task<string> SendMessageAsync(string userId, Message message, string? replyToken = null);

        Task<string> SendMessageAsync(string userId, List<Message> messageList, string? replyToken = null);
    }
}