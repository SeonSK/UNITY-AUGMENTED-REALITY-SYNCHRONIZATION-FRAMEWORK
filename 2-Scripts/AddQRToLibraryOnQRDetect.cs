using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager), typeof(QRCodeReader))]
public class AddQRToLibraryOnQRDetect : MonoBehaviour
{
    public event Action OnQRCodeAdded;

    ARTrackedImageManager trackedImageManager;
    QRCodeReader qrReader;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        qrReader = GetComponent<QRCodeReader>();
    }

    void OnEnable()
    {
        qrReader.OnQRDetectedAndAdded += GenerateQRAndAddToReferenceLibrary;
    }

    private void OnDisable()
    {
        qrReader.OnQRDetectedAndAdded -= GenerateQRAndAddToReferenceLibrary;
    }

    public void GenerateQRAndAddToReferenceLibrary(string msg)
    {
        Texture2D qrTexture = QRCodeWriter.CreateQRCodeTexture(msg, 200);
        Utility.AddImageToLibrary(qrTexture, trackedImageManager);
    }
}
