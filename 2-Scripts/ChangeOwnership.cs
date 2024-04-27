using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChangeOwnership : NetworkBehaviour
{
    NetworkObject netObj;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        netObj = GetComponent<NetworkObject>();
    }


    public void GetOwnership()
    {
        if (NetworkManager.LocalClientId != netObj.OwnerClientId)
        {
            Debug.Log("requesting ownership");
            ChangeOwnershipOnServerRpc();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    void ChangeOwnershipOnServerRpc(ServerRpcParams param = default)
    {
        netObj.ChangeOwnership(param.Receive.SenderClientId);
        
        Debug.Log("acquired ownership");
    }
}
