using MDP.Network.Rest;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging.Accesses
{
    [MDP.Registration.Service<EventService>()]
    public class RestEventService : EventService
    {
        // Methods
        public List<Event> CreateEvent(string content)
        {
            #region Contracts

            if (string.IsNullOrEmpty(content) == true) throw new ArgumentException($"{nameof(content)}=null");

            #endregion

            // Result
            List<Event> eventList = new List<Event>();

            // ContentDocument
            using (var contentDocument = JsonDocument.Parse(content))
            {
                // EventList
                foreach (var eventElement in contentDocument.RootElement.GetProperty<JsonElement>("events").EnumerateArray())
                {
                    // Event
                    var @event = this.CreateEvent(eventElement);
                    if (@event == null) continue;

                    // Add
                    eventList.Add(@event);
                }
            }

            // Return
            return eventList;
        }

        private Event? CreateEvent(JsonElement eventElement)
        {
            // EventType
            var eventType = eventElement.GetProperty<string>("type")?.ToLower();
            if (string.IsNullOrEmpty(eventType) == true) throw new InvalidOperationException($"{nameof(eventType)}=null");

            // Event
            Event? @event = null;
            switch (eventType)
            {
                // MessageEvent
                case MessageEvent.DefaultEventType: 
                    @event = this.CreateMessageEvent(eventElement);  
                    break;

                // FollowEvent
                case FollowEvent.DefaultEventType:
                    var followEvent = new FollowEvent();
                    {
                        followEvent.ReplyToken = eventElement.GetProperty<string>("replyToken") ?? throw new InvalidOperationException($"{nameof(followEvent.ReplyToken)}=null");
                    }
                    @event = followEvent;
                    break;

                // UnfollowEvent
                case UnfollowEvent.DefaultEventType:
                    var unfollowEvent = new UnfollowEvent();
                    {

                    }
                    @event = unfollowEvent;
                    break;

                // JoinEvent
                case JoinEvent.DefaultEventType:
                    var joinEvent = new JoinEvent();
                    {
                        joinEvent.ReplyToken = eventElement.GetProperty<string>("replyToken") ?? throw new InvalidOperationException($"{nameof(joinEvent.ReplyToken)}=null");
                    }
                    @event = joinEvent;
                    break;

                // LeaveEvent
                case LeaveEvent.DefaultEventType:
                    var leaveEvent = new LeaveEvent();
                    {

                    }
                    @event = leaveEvent;
                    break;
            }
            if (@event == null) throw new InvalidOperationException($"{nameof(@event)}=null, {nameof(eventType)}={eventType}");

            // Property
            @event.EventId = eventElement.GetProperty<string>("webhookEventId") ?? throw new InvalidOperationException($"{nameof(@event.EventId)}=null");
            @event.Source = this.CreateSource(eventElement.GetProperty<JsonElement>("source"));
            @event.IsRedelivery = eventElement.GetProperty<JsonElement>("deliveryContext").GetProperty<bool?>("isRedelivery") ?? throw new InvalidOperationException($"{nameof(@event.IsRedelivery)}=null");
            @event.Timestamp = eventElement.GetProperty<long?>("timestamp") ?? throw new InvalidOperationException($"{nameof(@event.Timestamp)}=null");
            @event.Mode = this.CreateMode(eventElement.GetProperty<string>("mode"));

            // Return
            return @event;
        }

        private MessageEvent CreateMessageEvent(JsonElement eventElement)
        {
            // MessageElement
            var messageElement = eventElement.GetProperty<JsonElement>("message");

            // MessageType
            var messageType = messageElement.GetProperty<string>("type")?.ToLower();
            if (string.IsNullOrEmpty(messageType) == true) throw new InvalidOperationException($"{nameof(messageType)}=null");
                       
            // MessageEvent
            MessageEvent? messageEvent = null;
            switch (messageType)
            {
                // TextMessageEvent
                case TextMessageEvent.DefaultMessageType:
                    var textMessageEvent = new TextMessageEvent();
                    {
                        textMessageEvent.Text = messageElement.GetProperty<string>("text") ?? throw new InvalidOperationException($"{nameof(textMessageEvent.Text)}=null");
                    }
                    messageEvent = textMessageEvent;
                    break;

                // ImageMessageEvent
                case ImageMessageEvent.DefaultMessageType:
                    var imageMessageEvent = new ImageMessageEvent();
                    {
                        imageMessageEvent.ContentProvider = this.CreateContentProvider(messageElement) ?? throw new InvalidOperationException($"{nameof(imageMessageEvent.ContentProvider)}=null");
                    }
                    messageEvent = imageMessageEvent;
                    break;

                // VideoMessageEvent
                case VideoMessageEvent.DefaultMessageType:
                    var videoMessageEvent = new VideoMessageEvent();
                    {
                        videoMessageEvent.ContentProvider = this.CreateContentProvider(messageElement) ?? throw new InvalidOperationException($"{nameof(videoMessageEvent.ContentProvider)}=null");
                        videoMessageEvent.Duration = messageElement.GetProperty<int?>("duration") ?? throw new InvalidOperationException($"{nameof(videoMessageEvent.Duration)}=null");
                    }
                    messageEvent = videoMessageEvent;
                    break;

                // AudioMessageEvent
                case AudioMessageEvent.DefaultMessageType:
                    var audioMessageEvent = new AudioMessageEvent();
                    {
                        audioMessageEvent.ContentProvider = this.CreateContentProvider(messageElement) ?? throw new InvalidOperationException($"{nameof(audioMessageEvent.ContentProvider)}=null");
                        audioMessageEvent.Duration = messageElement.GetProperty<int?>("duration") ?? throw new InvalidOperationException($"{nameof(audioMessageEvent.Duration)}=null");
                    }
                    messageEvent = audioMessageEvent;
                    break;

                // FileMessageEvent
                case FileMessageEvent.DefaultMessageType:
                    var fileMessageEvent = new FileMessageEvent();
                    {
                        fileMessageEvent.ContentProvider = new LineContentProvider()
                        {
                            MessageId = messageElement.GetProperty<string>("id") ?? throw new InvalidOperationException($"lineContentProvider.MessageId=null")
                        };
                        fileMessageEvent.FileName = messageElement.GetProperty<string>("fileName") ?? throw new InvalidOperationException($"{nameof(fileMessageEvent.FileName)}=null");
                        fileMessageEvent.FileSize = messageElement.GetProperty<int?>("fileSize") ?? throw new InvalidOperationException($"{nameof(fileMessageEvent.FileSize)}=null");
                    }
                    messageEvent = fileMessageEvent;
                    break;

                // LocationMessageEvent
                case LocationMessageEvent.DefaultMessageType:
                    var locationMessageEvent = new LocationMessageEvent();
                    {
                        locationMessageEvent.Title = messageElement.GetProperty<string>("title") ?? throw new InvalidOperationException($"{nameof(locationMessageEvent.Title)}=null");
                        locationMessageEvent.Address = messageElement.GetProperty<string>("address") ?? throw new InvalidOperationException($"{nameof(locationMessageEvent.Address)}=null");
                        locationMessageEvent.Latitude = messageElement.GetProperty<double?>("latitude") ?? throw new InvalidOperationException($"{nameof(locationMessageEvent.Latitude)}=null");
                        locationMessageEvent.Longitude = messageElement.GetProperty<double?>("longitude") ?? throw new InvalidOperationException($"{nameof(locationMessageEvent.Longitude)}=null");
                    }
                    messageEvent = locationMessageEvent;
                    break;

                // StickerMessageEvent
                case StickerMessageEvent.DefaultMessageType:
                    var stickerMessageEvent = new StickerMessageEvent();
                    {
                        stickerMessageEvent.StickerResourceType = messageElement.GetProperty<string>("stickerResourceType") ?? throw new InvalidOperationException($"{nameof(stickerMessageEvent.StickerResourceType)}=null");
                        stickerMessageEvent.PackageId = messageElement.GetProperty<string>("packageId") ?? throw new InvalidOperationException($"{nameof(stickerMessageEvent.PackageId)}=null");
                        stickerMessageEvent.StickerId = messageElement.GetProperty<string>("stickerId") ?? throw new InvalidOperationException($"{nameof(stickerMessageEvent.StickerId)}=null");
                    }
                    messageEvent = stickerMessageEvent;
                    break;
            }
            if (messageEvent == null) throw new InvalidOperationException($"{nameof(messageEvent)}=null, {nameof(messageType)}={messageType}");

            // Property
            messageEvent.MessageId = messageElement.GetProperty<string>("id") ?? throw new InvalidOperationException($"{nameof(messageEvent.MessageId)}=null");
            messageEvent.ReplyToken = eventElement.GetProperty<string>("replyToken") ?? throw new InvalidOperationException($"{nameof(messageEvent.ReplyToken)}=null");

            // Return
            return messageEvent;
        }
                

        private Source CreateSource(JsonElement sourceElement)
        {
            // SourceType
            var sourceType = sourceElement.GetProperty<string>("type")?.ToLower();
            if (string.IsNullOrEmpty(sourceType) == true) throw new InvalidOperationException($"{nameof(sourceType)}=null");

            // Source
            Source? source = null;
            switch (sourceType)
            {
                // UserSource
                case UserSource.DefaultSourceType:
                    var userSource = new UserSource();
                    {
                        userSource.UserId = sourceElement.GetProperty<string>("userId") ?? throw new InvalidOperationException($"{nameof(userSource.UserId)}=null");
                    }
                    source = userSource;
                    break;

                // GroupSource
                case GroupSource.DefaultSourceType:
                    var groupSource = new GroupSource();
                    {
                        groupSource.GroupId = sourceElement.GetProperty<string>("groupId") ?? throw new InvalidOperationException($"{nameof(userSource.UserId)}=null");
                        groupSource.UserId = sourceElement.GetProperty<string>("userId") ?? null;
                    }
                    source = groupSource;
                    break;

                // RoomSource
                case RoomSource.DefaultSourceType:
                    var roomSource = new RoomSource();
                    {
                        roomSource.RoomId = sourceElement.GetProperty<string>("roomId") ?? throw new InvalidOperationException($"{nameof(userSource.UserId)}=null");
                        roomSource.UserId = sourceElement.GetProperty<string>("userId") ?? null;
                    }
                    source = roomSource;
                    break;
            }
            if (source == null) throw new InvalidOperationException($"{nameof(source)}=null, {nameof(sourceType)}={sourceType}");

            // Return
            return source;
        }

        private ChannelState CreateMode(string? modeString)
        {
            #region Contracts

            if (string.IsNullOrEmpty(modeString) == true) throw new ArgumentException($"{nameof(modeString)}=null");

            #endregion

            // ChannelState
            ChannelState? channelState = null;
            switch (modeString.ToLower())
            {
                case "active": channelState = ChannelState.Active; break;
                case "standby": channelState = ChannelState.Standby; break;
            }
            if (channelState == null) throw new InvalidOperationException($"{nameof(channelState)}=null, {nameof(modeString)}={modeString}");

            // Return
            return channelState.Value;
        }

        private ContentProvider CreateContentProvider(JsonElement messageElement)
        {
            // ContentProviderElement
            var contentProviderElement = messageElement.GetProperty<JsonElement>("contentProvider");

            // ContentProviderType
            var contentProviderType = contentProviderElement.GetProperty<string>("type")?.ToLower();
            if (string.IsNullOrEmpty(contentProviderType) == true) throw new InvalidOperationException($"{nameof(contentProviderType)}=null");

            // ContentProvider
            ContentProvider? contentProvider = null;
            switch (contentProviderType)
            {
                // LineContentProvider
                case LineContentProvider.DefaultContentProviderType:
                    var lineContentProvider = new LineContentProvider();
                    {
                        lineContentProvider.MessageId = messageElement.GetProperty<string>("id") ?? throw new InvalidOperationException($"{nameof(lineContentProvider.MessageId)}=null");
                    }
                    contentProvider = lineContentProvider;
                    break;

                // ExternalContentProvider
                case ExternalContentProvider.DefaultContentProviderType:
                    var externalContentProvider = new ExternalContentProvider();
                    {
                        externalContentProvider.OriginalContentUrl = contentProviderElement.GetProperty<string>("originalContentUrl") ?? throw new InvalidOperationException($"{nameof(externalContentProvider.OriginalContentUrl)}=null");
                        externalContentProvider.PreviewImageUrl = contentProviderElement.GetProperty<string>("previewImageUrl") ?? throw new InvalidOperationException($"{nameof(externalContentProvider.PreviewImageUrl)}=null");
                    }
                    contentProvider = externalContentProvider;
                    break;
            }
            if (contentProvider == null) throw new InvalidOperationException($"{nameof(contentProvider)}=null, {nameof(contentProviderType)}={contentProviderType}");

            // Return
            return contentProvider;
        }
    }
}
