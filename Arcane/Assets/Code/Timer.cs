using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timer;
    public GameEvent timeOver;

    private void Update()
    {
        timer -= Time.deltaTime;
        int m = (int)timer / 60;
        int s = (int)timer - (m * 60);
        timerText.text = string.Format("{0:0}:{1:00}",m,s);
        
        if (timer <= 0)
        {
            timeOver?.FireEvent(null);
            Destroy(this);
        }
    }

}
