using UnityEngine;
using ZXing;

public static class QRCodeWriter
{
    public static Texture2D CreateQRCodeTexture(string msg, int size)
    {
        Texture2D tex = new(size, size, TextureFormat.RGBA32, false);

        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new ZXing.QrCode.QrCodeEncodingOptions { Height = size, Width = size }
        };
        var pixelData = writer.Write(msg);

        tex.LoadRawTextureData(pixelData.Pixels);

        tex.Apply();

        return tex;
    }
}
