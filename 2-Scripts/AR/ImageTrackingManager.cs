using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingManager : MonoBehaviour
{
    public event Action OnImageTrackComplete;

    [SerializeField] GameObject prefSynchInter;

    [Header("TrackingData")]
    [SerializeField] float trackingTimeLimit;

    [HideInInspector] public ARTrackedImage activeTrackedImage = null;

    bool isRoutRunin, once;
    Coroutine routine;

    WaitForSeconds wait;
    
    ARTrackedImageManager imageManager;

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void Awake()
    {
        wait = new(trackingTimeLimit);

        imageManager = GetComponent<ARTrackedImageManager>();
    }

    void Start()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            //Instantiate(prefSynchInter).GetComponent<NetworkObject>().Spawn();
            enabled = false;
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            //var minLocalScalar = Mathf.Min(trackedImage.size.x, trackedImage.size.y) / 2;
            //trackedImage.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);

            activeTrackedImage = trackedImage;
        }
    }

    IEnumerator ScanImageRout()
    {
        yield return wait;

        OnImageTrackComplete();

        isRoutRunin = false;
        once = true;
    }

    void Update()
    {
        if (activeTrackedImage == null) return;

        if (activeTrackedImage.trackingState == TrackingState.Limited)
        {
            if(isRoutRunin)
            {
                StopCoroutine(routine);
                isRoutRunin = false;
            }

            once = false;
        }

        if(activeTrackedImage.trackingState == TrackingState.Tracking) 
        {
            if (!isRoutRunin && !once)
            {
                routine = StartCoroutine(ScanImageRout());
                isRoutRunin = true;
            }
        }
    }
}