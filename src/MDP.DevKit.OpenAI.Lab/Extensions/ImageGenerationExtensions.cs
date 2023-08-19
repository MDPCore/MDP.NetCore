using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MDP.DevKit.OpenAI.Lab
{
    public static class ImageGenerationExtensions
    {
        // Methods
        public static async Task<List<string>> WriteToAsync(this Task<ImageGeneration> imageGenerationTask, string filePathFormat)
        {
            #region Contracts

            if (imageGenerationTask == null) throw new ArgumentException(nameof(imageGenerationTask));
            if (string.IsNullOrEmpty(filePathFormat) == true) throw new ArgumentException(nameof(filePathFormat));

            #endregion

            // Result
            var filePathList = new List<string>();
            var imageIndex = 0;

            // ImageGeneration
            using (var imageGeneration = await imageGenerationTask)
            {
                // ImageStream
                foreach (var imageStream in imageGeneration.Data.Select(o => o.Image))
                {
                    // FilePath
                    var filePath = Path.GetFullPath(string.Format(filePathFormat, ++imageIndex));
                    if (string.IsNullOrEmpty(filePath) == true) throw new InvalidOperationException(nameof(filePath));
                    filePathList.Add(filePath);

                    // DirectoryPath
                    var directoryPath = Path.GetDirectoryName(filePath);
                    if (string.IsNullOrEmpty(directoryPath) == true) throw new InvalidOperationException(nameof(directoryPath));
                    if (Directory.Exists(directoryPath) == false) Directory.CreateDirectory(directoryPath);

                    // FileStream
                    if (imageStream != null)
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            imageStream.WriteTo(fileStream);
                        }
                    }
                }
            }

            // Return
            return filePathList;
        }
    }
}
