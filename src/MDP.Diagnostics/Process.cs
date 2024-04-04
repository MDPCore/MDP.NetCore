using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace MDP.Diagnostics
{
    public static class Process
    {
        // Methods
        public static string Execute(string fileName, string arguments = null, string encodingName = "big5")
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException($"{nameof(fileName)}=null");

            #endregion

            // Environment
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Encoding
            var encoding = Encoding.GetEncoding(encodingName);
            if (encoding == null) throw new InvalidOperationException($"{nameof(encoding)}=null");

            // Process
            var executeResult = string.Empty;
            var executeError = string.Empty;
            using (var process = new System.Diagnostics.Process())
            {
                // Setting
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardErrorEncoding = encoding;
                process.StartInfo.StandardOutputEncoding = encoding;

                // Execute
                process.Start();
                executeResult = process.StandardOutput.ReadToEnd();
                executeError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                process.Close();
            }

            // Return
            if(string.IsNullOrEmpty(executeResult)==false) return executeResult;
            if(string.IsNullOrEmpty(executeError)==false) return executeError;
            return string.Empty;
        }
    }
}
