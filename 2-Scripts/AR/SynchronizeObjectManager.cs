using UnityEngine;

public class SynchronizeObjectManager : MonoBehaviour
{
    public void SetSynchData(Vector3 pos, Vector3 rot)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot);
    }
}
