using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ColorMaterialOverride : MonoBehaviour
{
    [SerializeField] Color color;
    
    void Awake()
    {
        MaterialPropertyBlock block = new();
        block.SetColor("_Color", color);
        
        var ren = GetComponent<MeshRenderer>();
        ren.SetPropertyBlock(block);
    }
    
    #if UNITY_EDITOR
    void Update()
    {
        if(EditorApplication.isPlayingOrWillChangePlaymode) return;
        
        Awake();
    }
    #endif
}
