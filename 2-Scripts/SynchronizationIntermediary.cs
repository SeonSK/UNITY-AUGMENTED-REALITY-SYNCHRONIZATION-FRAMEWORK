using System;
using Unity.Netcode;
using UnityEngine;

public class SynchronizationIntermediary : NetworkBehaviour
{
    public event Action<Vector3, Quaternion> OnClientReceiveSyncData;

    public void GetHostPose()
    {
        GetHostPoseServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void GetHostPoseServerRpc(ServerRpcParams param = default)
    {
        var pos = Camera.main.transform.position;
        var rot = Camera.main.transform.rotation;

        SentPoseToClientRpc(pos, rot, new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { param.Receive.SenderClientId }
            }
        });
    }

    [ClientRpc]
    public void SentPoseToClientRpc(Vector3 hostPos, Quaternion hostRot, ClientRpcParams param = default)
    {
        OnClientReceiveSyncData(hostPos, hostRot);
    }

    public void Vibrate()
    {
        VibrateServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void VibrateServerRpc()
    {
        Vibration.Vibrate(500);
    }
}