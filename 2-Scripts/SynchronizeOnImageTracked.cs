using System;
using Unity.XR.CoreUtils;
using UnityEngine;

[RequireComponent(typeof(XROrigin), typeof(ImageTrackingManager))]
public class SynchronizeOnImageTracked : MonoBehaviour
{
    public event Action OnSyncComplete;

    bool once;

    XROrigin xrOrigin;
    ImageTrackingManager imageTrackingManager;

    SynchronizationIntermediary intermediary;

    void OnEnable()
    {
        imageTrackingManager.OnImageTrackComplete += StartSyncProcess;
    }

    void OnDisable()
    {
        imageTrackingManager.OnImageTrackComplete -= StartSyncProcess;
    }

    void Awake()
    {
        xrOrigin = GetComponent<XROrigin>();
        imageTrackingManager = GetComponent<ImageTrackingManager>();
    }

    void StartSyncProcess()
    {
        if(!once)
        {
            intermediary = FindAnyObjectByType<SynchronizationIntermediary>();
            intermediary.OnClientReceiveSyncData += SyncObjectData;
            once = true;
        }

        intermediary.GetHostPose();
    }
        
    void SyncObjectData(Vector3 hostPos, Quaternion hostRot)
    {
        Transform imageTrfm = imageTrackingManager.activeTrackedImage.transform;
        if(once) imageTrfm.Rotate(90, 0, 0);

        Vector3 posOffset = imageTrfm.position - hostPos;
        Quaternion rotOffset = imageTrfm.rotation * Quaternion.Inverse(hostRot);

        xrOrigin.ShiftOriginTo(posOffset, rotOffset);

        intermediary.Vibrate();

        Vibration.Vibrate(500);
    }
}
