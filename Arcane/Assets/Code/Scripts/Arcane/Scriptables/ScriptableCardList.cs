using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptableCardList : ScriptableObject
{
    public ScriptableCard[] cards;

    public int GetIndex(ScriptableCard card)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] == card) return i;
        }
        return -1;
    }
}
