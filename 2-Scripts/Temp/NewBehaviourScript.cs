using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    void Update()
    {
        text.text = Camera.main.transform.rotation.eulerAngles.ToString();
    }
}
