using Azure.Core;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MDP.Registration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Storage.Images.Azure
{
    [Service<ImageFileRepository>(singleton: false, autoRegister: false)]
    public class AzureBlobImageFileRepository : ImageFileRepository
    {
        // Constants
        private static readonly Dictionary<string, string> _contentTypeDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" }
        };


        // Fields
        private readonly BlobServiceClient _serviceClient = null;

        private readonly BlobContainerClient _containerClient = null;


        // Constructors
        public AzureBlobImageFileRepository(string serviceUri, string containerName, TokenCredential azureCredential)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNullOrEmpty(serviceUri);
            ArgumentNullException.ThrowIfNullOrEmpty(containerName);
            ArgumentNullException.ThrowIfNull(azureCredential);

            #endregion

            // Default
            _serviceClient = new BlobServiceClient(new Uri(serviceUri), azureCredential);
            _containerClient = _serviceClient.GetBlobContainerClient(containerName);
        }


        // Methods
        public Uri Add(ImageFile imageFile, string directoryPath = null)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(imageFile);

            #endregion

            // Require
            if (string.IsNullOrEmpty(directoryPath) == true) directoryPath = string.Empty;

            // ContentType
            var contentType = _contentTypeDictionary.GetValueOrDefault(Path.GetExtension(imageFile.Name), string.Empty);
            if (string.IsNullOrEmpty(contentType) == true) throw new InvalidOperationException($"{nameof(imageFile.Name)}={imageFile.Name}");

            // BlobName
            var blobName = Path.Combine(directoryPath, imageFile.Name);
            blobName = blobName.Replace(Path.DirectorySeparatorChar, '/');

            // BlobClient
            var blobClient = _containerClient.GetBlobClient(blobName);
            {
                // Upload
                blobClient.DeleteIfExists();
                blobClient.Upload(imageFile.Content, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } });
                imageFile.Content.Seek(0, SeekOrigin.Begin);
            }

            // ImageUri
            return new Uri(blobClient.Uri.ToString());
        }

        public void Remove(Uri imageUri)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(imageUri);

            #endregion

            // ImageUriString
            var imageUriString = imageUri.GetLeftPart(UriPartial.Path).ToString();
            if (imageUriString.StartsWith(_containerClient.Uri.ToString(), StringComparison.OrdinalIgnoreCase) == false) return;

            // BlobName
            var blobName = imageUriString.Substring(_containerClient.Uri.ToString().Length + 1);
            if (string.IsNullOrEmpty(blobName) == true) return;

            // BlobClient
            var blobClient = _containerClient.GetBlobClient(blobName);
            {
                // Delete
                blobClient.DeleteIfExists();
            }
        }
    }
}
