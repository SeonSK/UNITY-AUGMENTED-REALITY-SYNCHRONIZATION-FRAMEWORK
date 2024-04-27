using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    [SerializeField] LayerMask pickableLayer;
    
    bool isPicked;
    Vector2 screenCenter;
    RaycastHit activeHitInfo;
    GameObject placeHolderObj;
    
    public void Start()
    {
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    public void PickupBox()
    {
        Debug.Log("entering pickup");
        if(isPicked)
        {
            Debug.Log("cancelling pickup");

            isPicked = false;
            activeHitInfo.rigidbody.isKinematic = false;
            placeHolderObj = default;
            activeHitInfo = default;
            return;
        }
        
        var center = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(center, out var hitInfo, 100, pickableLayer))
        {
            Debug.Log("pickup successfull");

            hitInfo.transform.GetComponent<ChangeOwnership>().GetOwnership();

            hitInfo.rigidbody.isKinematic = true;
            
            placeHolderObj = new GameObject();
            placeHolderObj.transform.SetPositionAndRotation(hitInfo.transform.position, hitInfo.transform.rotation);
            placeHolderObj.transform.parent = Camera.main.transform;
            
            activeHitInfo = hitInfo;
            isPicked = true;
        }
    }
    
    public void Update()
    {
        if(isPicked)
        {
            activeHitInfo.transform.SetPositionAndRotation(placeHolderObj.transform.position, placeHolderObj.transform.rotation);
        }
    }
}
