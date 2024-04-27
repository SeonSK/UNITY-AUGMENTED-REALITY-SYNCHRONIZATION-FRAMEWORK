using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallThrowInput : NetworkBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 spawnPos;
    [SerializeField] GameObject ballPref;
    
    bool isTouchPress;
    Vector3 prevTouchPos;
    GameObject activeBall;

    void Update()
    {
        if (!isTouchPress && Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 touch = Input.mousePosition;
            touch.z = 10;
            prevTouchPos = Camera.main.ScreenToWorldPoint(touch);

            SpawnBallOnServerRpc(Camera.main.transform.TransformPoint(spawnPos), ballPref.transform.rotation);
            // activeBall.transform.parent = transform;

            isTouchPress = true;
        }
        else if(isTouchPress && !Input.GetMouseButton(0))
        {
            Vector3 touch = Input.mousePosition;
            touch.z = 10;
            
            Vector3 curTouchPos = Camera.main.ScreenToWorldPoint(touch);
            
            Vector3 delta = prevTouchPos - curTouchPos;
            Vector3 dir = delta.normalized;
            
            dir *= delta.magnitude * speed;
            dir = Quaternion.AngleAxis(25, Camera.main.transform.right) * dir;

            StartCoroutine(BallThrowRout(dir));

            isTouchPress = false;
        }
    }

    IEnumerator BallThrowRout(Vector3 dir)
    {
        while(activeBall == null) yield return null;

        var rb = activeBall.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(dir, ForceMode.Impulse);

        activeBall = null;
    }
    
    [ClientRpc]
    public void SetBallObjClientRpc(ulong objId, ClientRpcParams param = default)
    {
        activeBall = GetNetworkObject(objId).gameObject;
        activeBall.GetComponent<Rigidbody>().isKinematic = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBallOnServerRpc(Vector3 pos, Quaternion rot, ServerRpcParams param = default)
    {
        var obj = Instantiate(ballPref, pos, rot);
        var netObj = obj.GetComponent<NetworkObject>();

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { param.Receive.SenderClientId }
            }
        };
        
        netObj.SpawnWithOwnership(param.Receive.SenderClientId);
        SetBallObjClientRpc(netObj.NetworkObjectId, clientRpcParams);
    }
}
