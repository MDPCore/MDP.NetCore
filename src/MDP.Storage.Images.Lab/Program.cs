using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using Azure.Identity;
using System.Threading.Tasks;
using Azure.Core;

namespace MDP.Storage.Images.Lab
{
    public class Program
    {
        // Methods
        public static void Run(ImageFileContext imageFileContext)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(imageFileContext);

            #endregion

            // ImageFile
            var imageFilePath = @"C:\\lab\c.gif";
            var imageFile = new ImageFile(Path.GetFileName(imageFilePath), File.OpenRead(imageFilePath));

            // Add
            var imageUri = imageFileContext.Add(imageFile);

            // Remove
            //imageFileContext.Remove(imageUri);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
