using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    [System.NonSerialized]
    private List<Action<object>> listeners = new List<Action<object>>();

    public void AddListener(Action<object> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(Action<object> listener)
    {
        if(listeners.Contains(listener))
            listeners.Remove(listener);
    }

    public void FireEvent(object data)
    {
        listeners.ForEach(l=>l.Invoke(data));
    }
}
