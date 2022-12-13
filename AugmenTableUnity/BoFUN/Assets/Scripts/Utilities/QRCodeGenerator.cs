using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

namespace BoFUN.Utilities
{
    public static class QRCodeGenerator
    {
        public static Color32[] GenerateQR(string text, int width = 70, int height = 70, bool transparentBackground = true)
        {

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0
                }
            };

            // Make white pixels transparent
            Color32[] result = writer.Write(text);
            if(transparentBackground)
                for (int i = 0; i < width*height; i++)
                {

                    if (result[i] == Color.white)
                    {
                        result[i] = new Color(0, 0, 0, 0);
                    }
                }
            return result;
        }

        public static Texture2D GenerateQRTexture(string text, int width = 70, int height = 70)
        {
            var encoded = new Texture2D(width, height);
            var color32 = GenerateQR(text, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            return encoded;
        }
    }
}
