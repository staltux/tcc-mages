using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class AButton : MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    
    [ReadOnly] public float holdTime = 0;
    public float longPressTime = 0.5f;
    [ReadOnly] public bool longPressConsumed = false;
    [SerializeField][ReadOnly] private bool mouseOver = false;

    //private ButtonClickedEvent oldEvents;
    //private ButtonClickedEvent ReenableEvent = new ButtonClickedEvent();
    
    
    public UnityEvent OnLongPress = new UnityEvent();
    public UnityEvent onClick = new UnityEvent();

  

    public void OnPointerDown(PointerEventData eventData)
    {
        longPressConsumed = false;
        mouseOver = true;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!longPressConsumed)onClick.Invoke();
        Consume(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        holdTime = 0;
        mouseOver = false;
    }
    


    public void Update()
    {
        if (!mouseOver) return;
        if (longPressConsumed) return;

        holdTime += Time.deltaTime;
        if (holdTime < longPressTime) return;
        Consume();
    }

    public void Consume(bool use)
    {
        if (use)OnLongPress?.Invoke();
        longPressConsumed = true;
        holdTime = 0;
    }

    public void Consume()
    {
        Consume(true);
    }


}
