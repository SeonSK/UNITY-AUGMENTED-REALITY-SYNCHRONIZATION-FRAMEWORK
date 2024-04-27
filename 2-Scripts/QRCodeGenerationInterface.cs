using UnityEngine;
using UnityEngine.UI;

public class QRCodeGenerationInterface : MonoBehaviour
{
    [Header("IP Assignment")]
    [SerializeField] bool autoDetectLocalIP;
    [SerializeField] string ip_address;

    [Header("Image Config")]
    [SerializeField] int size;
    [SerializeField] RawImage rawImage;

    [Header("ButtonData")]
    [SerializeField] Button toggleBtn;
    
    bool isQRActive = true;
    Texture2D qrTexture;

    void Start()
    {
        GenerateQRCode();
        RegisterToggleButton();

        rawImage.gameObject.SetActive(false);
    }

    void GenerateQRCode()
    {
        string address;
        if (autoDetectLocalIP)
            address = Utility.GetLocalIPAddress();
        else
            address = ip_address;

        qrTexture = QRCodeWriter.CreateQRCodeTexture(address, size);
        rawImage.texture = qrTexture;
    }

    void RegisterToggleButton()
    {
        toggleBtn.onClick.AddListener(SetQRActive);
    }

    void SetQRActive()
    {
        rawImage.gameObject.SetActive(isQRActive);
        isQRActive = !isQRActive;
    }
}