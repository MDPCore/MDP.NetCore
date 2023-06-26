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
        private readonly EventService _eventService;

        private readonly SignatureService _signatureService;

        private readonly UserService _userService;

        private readonly ContentService _contentService;

        private readonly MessageService _messageService;


        // Constructors
        public LineMessageContext
        (
            EventService eventService,
            SignatureService signatureService,
            UserService userService,
            ContentService contentService,
            MessageService messageService
        )
        {
            #region Contracts

            if (eventService == null) throw new ArgumentException($"{nameof(eventService)}=null");
            if (signatureService == null) throw new ArgumentException($"{nameof(signatureService)}=null");
            if (userService == null) throw new ArgumentException($"{nameof(userService)}=null");
            if (contentService == null) throw new ArgumentException($"{nameof(contentService)}=null");
            if (messageService == null) throw new ArgumentException($"{nameof(messageService)}=null");

            #endregion

            // Default
            _eventService = eventService;
            _signatureService = signatureService;
            _userService = userService;
            _contentService = contentService;
            _messageService = messageService;
        }


        // Properties
        public UserService UserService { get { return _userService; } }

        public ContentService ContentService { get { return _contentService; } }

        public MessageService MessageService { get { return _messageService; } }


        // Methods
        public List<Event> HandleHook(string content, string signature)
        {
            #region Contracts

            if (string.IsNullOrEmpty(content) == true) throw new ArgumentException($"{nameof(content)}=null");
            if (string.IsNullOrEmpty(signature) == true) throw new ArgumentException($"{nameof(signature)}=null");

            #endregion

            // Require
            if (_signatureService.ValidateSignature(content, signature) == false) throw new InvalidOperationException($"Validate failed");

            // EventList
            var eventList = _eventService.CreateEvent(content);
            if (eventList == null) throw new InvalidOperationException($"{nameof(eventList)}=null");

            // Return
            return eventList;
        }
    }
}
