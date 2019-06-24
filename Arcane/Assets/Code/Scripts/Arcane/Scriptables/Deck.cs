using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Deck
{

    public long ID { get; set; }
    
    
    
    
    //public bool IsActive { get; set; }

    public string title;
    //public MageData mage;
    
        /*
    public int Count { get { return cards.Count; } }
    */
    //private List<ScriptableCard> cards = new List<ScriptableCard>(new ScriptableCard[8]);

        /*

    public void SetCardInSlot(ScriptableCard card, int idx)
    {
        if (cards.Contains(card)) return;
        cards[idx] = card;
        CalculateMana();
    }

    public ScriptableCard GetCardFromSlot(int idx)
    {
        return cards[idx];
    }

    public void RemoveCardAt(int idx)
    {
        cards[idx] = null;
        CalculateMana();
    }
    */
    
    public float CalculateMana(DbHelper dbHelper)
    {
        var manaSum = 0.0f;
        var cardCount = 0;
        
        for (int i = 0; i < 8; i++)
        {
            var card = dbHelper.GetCardFromSlot(ID,i);
            if (card == null) continue;
            manaSum += card.mana;
            cardCount++;
        }

        var deck_cost = manaSum / cardCount;
        return deck_cost;
    }


    
}
