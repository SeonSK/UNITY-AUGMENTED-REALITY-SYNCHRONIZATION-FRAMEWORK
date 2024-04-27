using UnityEngine;
using System;
using ZXing;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;

[RequireComponent(typeof(ARTrackedImageManager))]
public class QRCodeReader : MonoBehaviour
{
    [SerializeField] ARCameraManager cameraManager;

    public event Action<string> OnQRDetectedAndAdded;
    [HideInInspector] public string decodedMessage;

    Texture2D cpuCameraTexture;
    ARTrackedImageManager trackedImageManager;

    void Start()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    unsafe bool UpdateCameraImage()
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image)) return false;

        var format = TextureFormat.Alpha8;

        if (cpuCameraTexture == null || cpuCameraTexture.width != image.width || cpuCameraTexture.height != image.height)
        {
            cpuCameraTexture = new Texture2D(image.width, image.height, format, false);
        }

        var conversionParams = new XRCpuImage.ConversionParams(image, format);

        NativeArray<byte> rawTextureData = cpuCameraTexture.GetRawTextureData<byte>();

        image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
        
        cpuCameraTexture.Apply();

        image.Dispose();

        return true;
    }

    public void GenerateQRAndAddToReferenceLibrary()
    {
        Texture2D qrTexture = QRCodeWriter.CreateQRCodeTexture(decodedMessage, 200);
        Utility.AddImageToLibrary(qrTexture, trackedImageManager);
        OnQRDetectedAndAdded(decodedMessage);
    }

    void Update()
    {
        if (!UpdateCameraImage()) return;

        IBarcodeReader reader = new BarcodeReader();

        byte[] rawData = cpuCameraTexture.GetPixelData<byte>(cpuCameraTexture.mipmapCount - 1).ToArray();

        Result result = reader.Decode(rawData, cpuCameraTexture.width, cpuCameraTexture.height, RGBLuminanceSource.BitmapFormat.Gray8);

        if (result != null)
        {
            decodedMessage = result.Text;
            GenerateQRAndAddToReferenceLibrary();

            enabled = false;
        }
    }
}
