using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

[RequireComponent(typeof(NetworkManager), typeof(UnityTransport))]
public class StartClientInterface : MonoBehaviour
{
    [Header("StartMethods")]
    [SerializeField] bool startOnQRScan;
    [SerializeField] QRCodeReader qrCodeReader;

    NetworkManager netManager;
    UnityTransport transport;

    void Awake()
    {
        netManager = GetComponent<NetworkManager>();
        transport = GetComponent<UnityTransport>();
    }

    void Start()
    {
        if (startOnQRScan) 
            qrCodeReader.OnQRDetectedAndAdded += StartClient;
    }

    public void StartClient(string ipAddress)
    {
        transport.ConnectionData.Address = ipAddress;
        netManager.StartClient();
    }
}
