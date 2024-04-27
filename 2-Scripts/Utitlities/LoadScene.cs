using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;
using Unity.Netcode;

[Serializable]
public struct SceneData
{
    public string sceneName;
    public bool broadCastLoad;
    public LoadSceneMode loadSceneMode;
}

public class LoadScene : MonoBehaviour
{
    [Header("LoadMethods")]
    [SerializeField] bool loadOnInitialize;
    [SerializeField] bool loadOnHostStart;

    [Header("LoadSceneData")]
    [SerializeField] SceneData[] arrLoadSceneData;

    [Header("UnLoadSceneData")]
    [SerializeField] string[] arrUnloadScenes;

    void Start()
    {
        if (loadOnInitialize)
        {
            Load();
        }
        else if(loadOnHostStart)
        {
            NetworkManager.Singleton.OnServerStarted += Load;
        }
    }

    public void Load()
    {
        AsyncOperation async = default;
        
        foreach (SceneData sceneData in arrLoadSceneData)
        {
            if(sceneData.broadCastLoad)
                NetworkManager.Singleton.SceneManager.LoadScene(sceneData.sceneName, sceneData.loadSceneMode);
            else
                async = SceneManager.LoadSceneAsync(sceneData.sceneName, sceneData.loadSceneMode);

        }

        async.completed += (d) => Load_Post();
    }

    void Load_Post()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(arrLoadSceneData[^1].sceneName));

        Unload();
    }

    void Unload()
    {
        foreach (string sceneName in arrUnloadScenes)
            SceneManager.UnloadSceneAsync(sceneName);
    }
}
