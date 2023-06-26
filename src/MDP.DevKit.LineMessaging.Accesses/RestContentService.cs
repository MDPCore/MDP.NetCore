using MDP.Network.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.DevKit.LineMessaging.Accesses
{
    public class RestContentService : ContentService
    {
        // Fields
        private readonly RestClientFactory _restClientFactory;


        // Constructors
        public RestContentService(RestClientFactory restClientFactory)
        {
            #region Contracts

            if (restClientFactory == null) throw new ArgumentException($"{nameof(restClientFactory)}=null");

            #endregion

            // Default
            _restClientFactory = restClientFactory;
        }


        // Methods
        public Task<Stream> CreatePreviewContentAsync(ContentProvider contentProvider)
        {
            #region Contracts

            if (contentProvider == null) throw new ArgumentException($"{nameof(contentProvider)}=null");

            #endregion

            // LineContentProvider
            if (contentProvider is LineContentProvider lineContentProvider)
            {
                // RequestUrl
                var requestUri = $"/message/{lineContentProvider.MessageId}/content/preview";

                // ContentStream
                var contentStream = this.GetLineContentAsync(requestUri);
                if (contentStream == null) throw new InvalidOperationException($"{nameof(contentStream)}=null");

                // Return
                return contentStream;
            }

            // ExternalContentProvider
            if (contentProvider is ExternalContentProvider externalContentProvider)
            {
                // RequestUrl
                var requestUri = externalContentProvider.PreviewImageUrl;

                // ContentStream
                var contentStream = this.GetExternalContentAsync(requestUri);
                if (contentStream == null) throw new InvalidOperationException($"{nameof(contentStream)}=null");

                // Return
                return contentStream;
            }

            // Throw
            throw new InvalidOperationException($"Unknown ContentProvider Type={contentProvider.GetType()}");
        }

        public Task<Stream> CreateOriginalContentAsync(ContentProvider contentProvider)
        {
            #region Contracts

            if (contentProvider == null) throw new ArgumentException($"{nameof(contentProvider)}=null");

            #endregion

            // LineContentProvider
            if (contentProvider is LineContentProvider lineContentProvider)
            {
                // RequestUrl
                var requestUri = $"/message/{lineContentProvider.MessageId}/content";

                // ContentStream
                var contentStream = this.GetLineContentAsync(requestUri);
                if (contentStream == null) throw new InvalidOperationException($"{nameof(contentStream)}=null");

                // Return
                return contentStream;
            }

            // ExternalContentProvider
            if (contentProvider is ExternalContentProvider externalContentProvider)
            {
                // RequestUrl
                var requestUri = externalContentProvider.OriginalContentUrl;

                // ContentStream
                var contentStream = this.GetExternalContentAsync(requestUri);
                if (contentStream == null) throw new InvalidOperationException($"{nameof(contentStream)}=null");

                // Return
                return contentStream;
            }

            // Throw
            throw new InvalidOperationException($"Unknown ContentProvider Type={contentProvider.GetType()}");
        }

        private async Task<Stream> GetLineContentAsync(string requestUri)
        {
            #region Contracts

            if (string.IsNullOrEmpty(requestUri) == true) throw new ArgumentException($"{nameof(requestUri)}=null");

            #endregion

            // Execute
            try
            {
                // RestClient
                using (var restClient = _restClientFactory.CreateClient("MDP.DevKit.LineMessaging", "LineContentService"))
                {
                    // Send
                    var resultModel = await restClient.GetAsync<Stream, ErrorModel>(requestUri);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
            }
            catch (RestException<ErrorModel> restException)
            {
                // ResponseException
                var responseException = restException.ErrorModel?.ToException();
                if (responseException != null) throw responseException;

                // Exception
                throw;
            }
        }

        private async Task<Stream> GetExternalContentAsync(string requestUri)
        {
            #region Contracts

            if (string.IsNullOrEmpty(requestUri) == true) throw new ArgumentException($"{nameof(requestUri)}=null");

            #endregion

            // Execute
            try
            {
                // RestClient
                using (var restClient = _restClientFactory.CreateClient())
                {
                    // Send
                    var resultModel = await restClient.GetAsync<Stream, string>(requestUri);
                    if (resultModel == null) throw new InvalidOperationException($"{nameof(resultModel)}=null");

                    // Return
                    return resultModel;
                }
            }
            catch (RestException<string> restException)
            {
                // Throw
                throw new LineMessageException(restException.Message);
            }
        }
    }
}
