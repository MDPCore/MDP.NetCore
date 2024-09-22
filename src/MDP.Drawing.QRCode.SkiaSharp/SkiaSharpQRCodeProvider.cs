using MDP.Registration;
using SkiaSharp;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Drawing.QRCode.SkiaSharp
{
    [Service<QRCodeProvider>()]
    public class SkiaSharpQRCodeProvider : QRCodeProvider
    {
        // Methods
        public string CreateToBase64(string content, int width, int height)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNullOrEmpty(content);

            #endregion

            // QRCodeGenerator
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q))
            {
                // Count
                var moduleCount = qrCodeData.ModuleMatrix.Count;
                var pixelsCount = Math.Min(width / moduleCount, height / moduleCount);

                // SKBitmap
                using (var bitmap = new SKBitmap(moduleCount * pixelsCount, moduleCount * pixelsCount))
                {
                    // SKCanvas
                    using (var canvas = new SKCanvas(bitmap))
                    {
                        // Background
                        canvas.Clear(SKColors.White);

                        // Paint
                        var paint = new SKPaint
                        {
                            Color = SKColors.Black,
                            IsAntialias = true,
                            Style = SKPaintStyle.Fill
                        };

                        // Draw
                        for (int y = 0; y < moduleCount; y++)
                        {
                            for (int x = 0; x < moduleCount; x++)
                            {
                                if (qrCodeData.ModuleMatrix[y][x])
                                {
                                    canvas.DrawRect(new SKRect(x * pixelsCount, y * pixelsCount, (x + 1) * pixelsCount, (y + 1) * pixelsCount), paint);
                                }
                            }
                        }

                        // Flush
                        canvas.Flush();
                    }

                    // SKImage
                    using (var image = SKImage.FromBitmap(bitmap))
                    {
                        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                        {
                            return $"data:image/png;base64,{Convert.ToBase64String(data.ToArray())}";
                        }
                    }
                }
            }
        }
    }
}
