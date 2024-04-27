using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkManager), typeof(UnityTransport))]
public class StartHostInterface : MonoBehaviour
{
    [Header("StartHostMethods")]
    [SerializeField] bool startOnInitialize;
    [SerializeField] bool startByButton;
    [SerializeField] Button startHostButton;
    
    [Header("SceneConfigData")]
    [SerializeField] bool loadSceneOnHostStart;
    [SerializeField] LoadSceneMode loadSceneMode;
    [SerializeField] string sceneName;

    [Header("SpawnNetworkPrefabs")]
    [SerializeField] bool spawnOnHostStart;
    [SerializeField] List<GameObject> listPrefabsToSpawn;

    [Header("ClientConfigData")]
    [SerializeField] LoadSceneMode clientSceneLoadMode;

    NetworkManager netManager;
    UnityTransport transport;

    void Awake()
    {
        netManager = GetComponent<NetworkManager>();
        transport = GetComponent<UnityTransport>();
    }

    void Start()
    {
        if (startByButton)
        {
            if (loadSceneOnHostStart)
                startHostButton.onClick.AddListener(StartHostAndLoadScene);
            else
                startHostButton.onClick.AddListener(StartHost);
        }

        if (spawnOnHostStart)
            netManager.OnServerStarted += SpawnNetworkPrefabs;

        if (startOnInitialize)
        {
            if (loadSceneOnHostStart)
                StartHostAndLoadScene();
            else
                StartHost();
        }
    }
    
    void StartHost()
    {
        transport.ConnectionData.ServerListenAddress = Utility.GetLocalIPAddress();
        netManager.StartHost();
    }

    void StartHostAndLoadScene()
    {
        netManager.OnServerStarted += LoadScene;
        StartHost();
    }

    void LoadScene()
    {
        netManager.SceneManager.LoadScene(sceneName, loadSceneMode);
    }

    void SpawnNetworkPrefabs()
    {
        foreach(var obj in listPrefabsToSpawn)
        {
            Instantiate(obj).GetComponent<NetworkObject>().Spawn();
        }
    }
}