using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAnimationEnd : MonoBehaviour
{

    public void Disable()
    {
        transform.gameObject.SetActive(false);
    }
}
