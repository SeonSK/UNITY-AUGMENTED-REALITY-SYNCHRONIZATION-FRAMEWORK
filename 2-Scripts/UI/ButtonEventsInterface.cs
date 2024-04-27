using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class ButtonEventsInterface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action onPointerDown, onPointerUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp();
    }
}
