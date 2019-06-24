using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ArcaneButton : MonoBehaviour, IPointerDownHandler
{

    [System.Serializable]
    public class OnPressEvent : UnityEvent { }

    public OnPressEvent OnPress;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnMouseDown();
    }

    public void OnMouseDown()
    {
        OnPress.Invoke();
    }


}
