using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class SpawnOnPlane : NetworkBehaviour
{
    [Header("SpawnData")]
    [SerializeField] GameObject spawnPref;

    [Header("SpawnMethod")]
    [SerializeField] bool spawnByButton;
    [SerializeField] Button spawnButton;

    bool isPointerDown;
    Vector2 screenCenter;
    GameObject activeNet = null;

    ButtonEventsInterface btnEvents;

    void OnEnable()
    {
        if (spawnByButton)
        {
            btnEvents = spawnButton.gameObject.AddComponent<ButtonEventsInterface>();
            btnEvents.onPointerDown += () => isPointerDown = true;
            btnEvents.onPointerUp += () => isPointerDown = false;
        }
    }

    void OnDisable()
    {
        if (spawnByButton)
        {
            btnEvents.onPointerDown -= () => isPointerDown = true;
            btnEvents.onPointerUp -= () => isPointerDown = false;
        }
    }

    public void Start()
    {
        screenCenter = new Vector2(Screen.width/2, Screen.height/2);
    }

    void Update()
    {
        if(isPointerDown) Spawn();
    }
    
    void Spawn()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(screenCenter), out var hitInfo, 100))
        {
            var pos = hitInfo.point;
            pos += hitInfo.normal;
            
            var dir = Camera.main.transform.position - hitInfo.point;
            
            var rot = Quaternion.LookRotation(-Vector3.ProjectOnPlane(dir, hitInfo.normal), hitInfo.normal);

            SpawnNet(hitInfo.point, rot);
        }
    }
    
    void SpawnNet(Vector3 pos, Quaternion rot)
    {       
        if(activeNet == null)
        {
            activeNet = Instantiate(spawnPref, pos, rot);

            activeNet.GetComponent<NetworkObject>().Spawn();
        } 
        else
        {
            activeNet.transform.SetPositionAndRotation(pos, rot);
        }
    }
}
