using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchHelper : MonoBehaviour
{

    
    [ReadOnly]  public float holdTime = 0;
    public float longPressTime = 1;

    void Start()
    {
        
    }

    private void Update()
    {
        LongPress();
    }

    void LongPress()
    {
        if (Input.touchCount < 1) return;
        var touchIndex = 0;

        switch (Input.GetTouch(touchIndex).phase)
        {
            case TouchPhase.Began:
            case TouchPhase.Canceled:
            case TouchPhase.Ended:
            case TouchPhase.Moved:
                holdTime = 0;
                break;

            case TouchPhase.Stationary:
                holdTime += Time.deltaTime;
                break;
        }

        if (holdTime < longPressTime) return;

        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Mouse Over: " + EventSystem.current.currentSelectedGameObject.name);
            
        }
        else {
            Debug.Log("Mouse Over: nothing");
        }




    }
}
