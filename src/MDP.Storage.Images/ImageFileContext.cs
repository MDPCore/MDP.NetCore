using MDP.Registration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MDP.Storage.Images
{
    [Service<ImageFileContext>(singleton: true)]
    public class ImageFileContext
    {
        // Fields
        private readonly ImageFileRepository _imageFileRepository = null;

        private readonly IList<ImageFileHandler> _imageFileHandlerList = null;


        // Constructors
        public ImageFileContext(ImageFileRepository imageFileRepository, IList<ImageFileHandler> imageFileHandlerList)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(imageFileRepository);
            ArgumentNullException.ThrowIfNull(imageFileHandlerList);

            #endregion

            // Default
            _imageFileRepository = imageFileRepository;
            _imageFileHandlerList = imageFileHandlerList;
        }


        // Methods
        public Uri Add(ImageFile imageFile, string directoryPath = null)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(imageFile);

            #endregion

            try
            {
                // ImageFileHandlerList
                foreach (var imageFileHandler in _imageFileHandlerList)
                {
                    // Process
                    var imageFileResult = imageFileHandler.Process(imageFile);
                    if (imageFileResult == null) throw new InvalidOperationException($"{nameof(imageFileResult)}=null");
                    if (imageFileResult != imageFile)
                    {
                        imageFile.Dispose();
                        imageFile = imageFileResult;
                    }
                }

                // ImageFileRepository
                var imageUri = _imageFileRepository.Add(imageFile, directoryPath);
                if (imageUri == null) throw new InvalidOperationException($"{nameof(imageUri)}=null");

                // ImageUriBuilder
                var imageUriBuilder = new UriBuilder(imageUri);
                if (string.IsNullOrEmpty(imageUriBuilder.Query) == false)
                {
                    imageUriBuilder.Query = imageUriBuilder.Query.TrimStart('?') + "&time=" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                else
                {
                    imageUriBuilder.Query = "time=" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }

                // Return
                return imageUriBuilder.Uri;
            }
            finally
            {
                // Dispose
                imageFile.Dispose();
            }
        }

        public void Remove(Uri imageUri)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(imageUri);

            #endregion

            // ImageRepository
            _imageFileRepository.Remove(imageUri);
        }
    }
}
