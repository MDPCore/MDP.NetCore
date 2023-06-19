using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging
{
    [MDP.Registration.Service<LineMessageContext>(singleton: true)]
    public class LineMessageContext
    {
        // Fields
        private readonly UserService _userService;

        private readonly MessageService _messageService;


        // Constructors
        public LineMessageContext
        (
            UserService userService,
            MessageService messageService
        )
        {
            #region Contracts

            if (userService == null) throw new ArgumentException($"{nameof(userService)}=null");
            if (messageService == null) throw new ArgumentException($"{nameof(messageService)}=null");

            #endregion

            // Default
            _userService = userService;
            _messageService = messageService;
        }


        // Properties
        public UserService UserService { get { return _userService; } }

        public MessageService MessageService { get { return _messageService; } }
    }
}
