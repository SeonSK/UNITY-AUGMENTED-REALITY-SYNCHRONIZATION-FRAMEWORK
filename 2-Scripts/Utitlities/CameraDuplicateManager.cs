using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraDuplicateManager : MonoBehaviour
{
    Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        SceneManager.sceneLoaded += (c, y) => CheckDuplicates();
    }

    void CheckDuplicates()
    {
        if (Camera.allCamerasCount > 1)
            cam.enabled = false;
        else if(Camera.allCamerasCount == 0)
            cam.enabled = true;
    }
}
