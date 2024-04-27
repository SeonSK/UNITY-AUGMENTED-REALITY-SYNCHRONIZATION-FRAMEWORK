// using System;
// using Unity.Collections.LowLevel.Unsafe;
// using Unity.Jobs;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARFoundation.Samples;
// using UnityEngine.XR.ARSubsystems;

// public class AddImageToLibrary : MonoBehaviour
// {
//     ARTrackedImageManager arImageManager;
//     ARCameraManager cameraManager;
//     Texture2D m_CameraTexture;
//     XRCpuImage.Transformation m_Transformation = XRCpuImage.Transformation.MirrorY;
    
//     void Awake()
//     {
//         cameraManager = FindFirstObjectByType<ARCameraManager>();
//         arImageManager = GetComponent<ARTrackedImageManager>();
//     }

//     unsafe void UpdateCameraImage()
//     {
//         Debug.Log("UpdatingCameraImage");
        
//         if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
//         {
//             return;
//         }
        
//         var format = TextureFormat.RGBA32;

//         if (m_CameraTexture == null || m_CameraTexture.width != image.width || m_CameraTexture.height != image.height)
//         {
//             m_CameraTexture = new Texture2D(image.width, image.height, format, false);
//         }

//         var conversionParams = new XRCpuImage.ConversionParams(image, format, m_Transformation);

//         var rawTextureData = m_CameraTexture.GetRawTextureData<byte>();
//         try
//         {
//             image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
//         }
//         finally
//         {
//             image.Dispose();
//         }

//         m_CameraTexture.Apply();
//         Debug.Log("image-set");
//     }

//     void AddImage()
//     {
//         if (arImageManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
//         {
//             UpdateCameraImage();
//             Debug.Log("entered-AddImageToLibrary()");
//             Debug.Log(m_CameraTexture.width);
//             mutableLibrary.ScheduleAddImageWithValidationJob(m_CameraTexture, "Temp", 0.2f);
//         }
//     }

//     public void OnImageAddButtonPress() 
//     {
//         AddImage();
//     }

//     // void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
//     // {
//     //     UpdateCameraImage();
//     // }
    
// }
