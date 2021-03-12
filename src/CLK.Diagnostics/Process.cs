using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CLK.Diagnostics
{
    public static class Process
    {
        // Methods
        public static string Execute(string fileName, string arguments = null)
        {
            #region Contracts

            if (string.IsNullOrEmpty(fileName) == true) throw new ArgumentException();

            #endregion
            
            // Result
            var syncRoot = new object();
            var resultBuilder = new StringBuilder();

            // ReceivedHandler
            DataReceivedEventHandler receivedHandler = (sender, e) =>
            {
                #region Contracts

                if (sender == null) throw new ArgumentException();
                if (e == null) throw new ArgumentException();

                #endregion

                // Sync
                lock (syncRoot)
                {
                    // Data
                    var data = e.Data;
                    if (string.IsNullOrEmpty(data) == true) return;

                    // Add
                    resultBuilder.AppendLine(e.Data);
                }
            };
            
            // Process
            using (var process = new System.Diagnostics.Process())
            {
                // Setting
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.ErrorDataReceived += receivedHandler;
                process.OutputDataReceived += receivedHandler;

                // Execute
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }

            // Return
            return resultBuilder.ToString();
        }
    }
}
