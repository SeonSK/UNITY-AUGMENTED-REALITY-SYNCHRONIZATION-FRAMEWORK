using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static List<string> listInstances = new();

    void Awake()
    {
        if(listInstances.Contains(name))
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            listInstances.Add(name);
        }
    }
}
