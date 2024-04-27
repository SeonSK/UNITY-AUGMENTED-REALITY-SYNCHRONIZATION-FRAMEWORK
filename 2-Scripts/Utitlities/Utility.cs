using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public static class Utility
{


    public static string GetLocalIPAddress()
    {
        string localAddress = "";
        
        string hostName = Dns.GetHostName();
        IPAddress[] localIPs = Dns.GetHostAddresses(hostName);

        foreach (IPAddress ip in localIPs)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localAddress = ip.ToString();
                break;
            }
        }

        return localAddress;
    }

    public static Texture2D GetQRFromTexture(Result result, Texture2D texture)
    {
        ResultPoint[] points = result.ResultPoints;

        Vector2 point1 = new(points[0].X, points[0].Y);
        Vector2 point2 = new(points[1].X, points[1].Y);
        Vector2 point3 = new(points[2].X, points[2].Y);

        float minX = Mathf.Min(point1.x, point2.x, point3.x);
        float maxX = Mathf.Max(point1.x, point2.x, point3.x);
        float minY = Mathf.Min(point1.y, point2.y, point3.y);
        float maxY = Mathf.Max(point1.y, point2.y, point3.y);

        int width = (int)(maxX - minX);
        int height = (int)(maxY - minY);

        Color[] pixels = texture.GetPixels((int)minX, (int)minY, width, height);

        Texture2D qrcodeTexture = new Texture2D(width, height);
        qrcodeTexture.SetPixels(pixels);
        qrcodeTexture.Apply();

        return qrcodeTexture;
    }

    public static void AddImageToLibrary(Texture2D texture, ARTrackedImageManager manager)
    {
        var mutableLibrary = manager.referenceLibrary as MutableRuntimeReferenceImageLibrary;

        var job = mutableLibrary.ScheduleAddImageWithValidationJob(texture, "name", 0.1f);

        job.jobHandle.Complete();
    }
}


