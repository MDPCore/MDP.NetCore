using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MDP.Storage.Images
{
    public class ImageFile : IDisposable
    {
        // Constants
        private static readonly string[] _imageExtensionList = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };


        // Constructors
        public ImageFile(string name, string contentString)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(contentString);

            #endregion

            // Require
            this.ValidateFileName(name);

            // Default
            this.Name = name;
            this.Content = new MemoryStream(Convert.FromBase64String(contentString));
        }

        public ImageFile(string name, Stream content)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(content);

            #endregion

            // Require
            this.ValidateFileName(name);

            // Default
            this.Name = name;
            this.Content = content;
        }

        public void Dispose()
        {
            // Content
            this.Content?.Dispose();
        }


        // Properties
        public string Name { get; set; }

        public Stream Content { get; set; }


        // Methods
        private void ValidateFileName(string fileName)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(fileName);

            #endregion

            // 檢查檔名是否為空或包含無效字元
            if (string.IsNullOrWhiteSpace(fileName) == true) throw new InvalidOperationException($"{nameof(fileName)}={fileName}");
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) throw new InvalidOperationException($"{nameof(fileName)}={fileName}");

            // 檢查副檔名是否為圖檔
            var fileExtension = Path.GetExtension(fileName);
            var isValidExtension = _imageExtensionList.Any(o => string.Equals(o, fileExtension, StringComparison.OrdinalIgnoreCase));
            if (isValidExtension== false) throw new InvalidOperationException($"{nameof(fileName)}={fileName}");
        }
    }
}
