using System.IO;

namespace MDP.DevKit.LineMessaging.Lab
{
    public static class ContentProviderExtensions
    {
        // Methods
        public static async Task<List<string>> WriteToAsync(this List<Event> eventList, string filePathFormat, LineMessageContext lineMessageContext)
        {
            #region Contracts

            if (eventList == null) throw new ArgumentException($"{nameof(eventList)}=null");
            if (string.IsNullOrEmpty(filePathFormat) == true) throw new ArgumentException($"{nameof(filePathFormat)}=null");
            if (lineMessageContext == null) throw new ArgumentException($"{nameof(lineMessageContext)}=null");

            #endregion

            // Result
            var filePathList = new List<string>();

            // Event
            foreach (var @event in eventList)
            {
                // ContentProvider
                ContentProvider? contentProvider = null;
                {
                    // EventType
                    if (@event is ImageMessageEvent imageMessageEvent) contentProvider = imageMessageEvent.ContentProvider;
                }
                if (contentProvider == null) throw new InvalidOperationException($"{nameof(contentProvider)}=null");

                // FilePath
                var filePath = Path.GetFullPath(string.Format(filePathFormat, @event.EventId));
                if (string.IsNullOrEmpty(filePath) == true) throw new InvalidOperationException(nameof(filePath));
                filePathList.Add(filePath);

                // DirectoryPath
                var directoryPath = Path.GetDirectoryName(filePath);
                if (string.IsNullOrEmpty(directoryPath) == true) throw new InvalidOperationException(nameof(directoryPath));
                if (Directory.Exists(directoryPath) == false) Directory.CreateDirectory(directoryPath);

                // ContentStream
                using (var contentStream = await lineMessageContext.ContentService.CreateOriginalContentAsync(contentProvider) as MemoryStream)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        contentStream?.WriteTo(fileStream);
                    }
                }
            }

            // Return
            return filePathList;
        }
    }
}
