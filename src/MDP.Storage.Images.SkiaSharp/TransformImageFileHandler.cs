using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP.Registration;
using SkiaSharp;

namespace MDP.Storage.Images.SkiaSharp
{
    [Service<ImageFileHandler>(singleton: false, autoRegister: false)]
    public class TransformImageFileHandler : ImageFileHandler
    {
        // Constants
        private static readonly Dictionary<string, SKEncodedImageFormat> _imageFormatDictionary = new Dictionary<string, SKEncodedImageFormat>(StringComparer.OrdinalIgnoreCase)
        {
            { "jpg", SKEncodedImageFormat.Jpeg },
            { "jpeg",SKEncodedImageFormat.Jpeg},
            { "png", SKEncodedImageFormat.Png},
            { "gif", SKEncodedImageFormat.Gif },
            { "bmp", SKEncodedImageFormat.Bmp }
        };


        // Fields
        private readonly int _outputWidth = 1080;

        private readonly int _outputHeight = 1080;

        private readonly SKEncodedImageFormat _outputFormat = SKEncodedImageFormat.Jpeg;

        private readonly string _outputExtension = "jpg";

        private readonly int _outputQuality = 85;


        // Constructors
        public TransformImageFileHandler(int outputWidth, int outputHeight, string outputFormat, int outputQuality = 85)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNullOrEmpty(outputFormat);

            #endregion

            // ImageFormat
            if (_imageFormatDictionary.ContainsKey(outputFormat) == false) throw new InvalidOperationException($"{nameof(outputFormat)}={outputFormat}");

            // Default
            _outputWidth = outputWidth;
            _outputHeight = outputHeight;
            _outputQuality = outputQuality;
            _outputFormat = _imageFormatDictionary.GetValueOrDefault(outputFormat);
            _outputExtension = outputFormat;
        }


        // Methods
        public ImageFile Process(ImageFile imageFile)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(imageFile);

            #endregion

            // InputBitmap
            using (var inputBitmap = SKBitmap.Decode(imageFile.Content))
            {
                // Size
                int width = 0, height = 0;
                if (inputBitmap.Width > inputBitmap.Height)
                {
                    width = _outputWidth;
                    height = inputBitmap.Height * _outputWidth / inputBitmap.Width;
                }
                else
                {
                    height = _outputHeight;
                    width = inputBitmap.Width * _outputHeight / inputBitmap.Height;
                }

                // Transform
                using (var outputBitmap = inputBitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.High))
                {
                    using (var outputData = outputBitmap.Encode(_outputFormat, _outputQuality))
                    {
                        // OutputName
                        var outputName = Path.ChangeExtension(imageFile.Name, _outputExtension);
                        if (string.IsNullOrEmpty(outputName) == true) throw new InvalidOperationException($"{nameof(outputName)}={outputName}");

                        // OutputStream
                        var outputStream = new MemoryStream();
                        outputData.SaveTo(outputStream);
                        outputStream.Seek(0, SeekOrigin.Begin);
                        
                        // OutputImageFile
                        return new ImageFile(outputName, outputStream);
                    }
                }
            }
        }
    }
}
