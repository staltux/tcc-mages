using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public BookCanvas canvas;

    public void OnOpened()
    {
        canvas.OnAnimationEnd();
    }
    
}
