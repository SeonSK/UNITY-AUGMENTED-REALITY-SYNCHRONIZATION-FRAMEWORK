using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShotCounter : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    
    int score;
    
    Collider col;
    Vector3 prevDelta;
    
    WaitForSeconds wait;
    
    TextMeshProUGUI scoreText;
    
    void Awake()
    {
        wait = new(2);
        col = GetComponent<Collider>();
    }
    
    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        // if(!Input.GetMouseButtonDown(0)) return;
        
        if(!other.GetComponent<NetworkObject>().IsOwner) return;
        
        Vector3 delta = other.transform.position - col.transform.position;
        
        float dot = Vector3.Angle(prevDelta, delta);
        UnityEngine.Debug.Log($"shot");
        // if(dot < 0) UnityEngine.Debug.Log($"Shot");
        
        Vibration.Vibrate(500);
        StartCoroutine(ShowTextRout());
        
        prevDelta = delta;
        
        score += 1;
        
        scoreText.text = "SCORE: " + score;
    }
    
    
    IEnumerator ShowTextRout()
    {
        textObj.SetActive(true);
        
        yield return wait;
        
        textObj.SetActive(false);
    }
}
