using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARInitialize : MonoBehaviour
{
    [SerializeField] GameObject arPlanePref;
    
    public void Start()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            var planeManager = gameObject.AddComponent<ARPlaneManager>();
            planeManager.planePrefab = arPlanePref;
        }
        else
        {
            
        }
    }
}
