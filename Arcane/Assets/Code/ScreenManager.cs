using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public GameObject mage;
    public GameObject chest;
    public GameObject deck;
    public GameObject activeDeck;
    public GameObject credits;
    public GameObject BattleCanvas;
    public GameObject menu;
    public DbHelper dbHelper;
    public LevelLoader levelLoader;
    public Animator emptySlotPopup;
    public Toggle deckButton;

    // Start is called before the first frame update
    void Start()
    {
        HideAll();
        ShowDeck(true);
    }

    private void HideAll()
    {
        mage.gameObject.SetActive(false);
        chest.gameObject.SetActive(false);
        deck.gameObject.SetActive(false);
        activeDeck.gameObject.SetActive(false);
        credits.gameObject.SetActive(false);
        BattleCanvas.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }

    public void ShowMage(bool v)
    {
        if (!v) return;
        HideAll();

        mage.gameObject.SetActive(true);
    }

    public void ShowChest(bool v)
    {
        if (!v) return;
        HideAll();
        chest.gameObject.SetActive(true);
    }

    public void ShowDeck(bool v)
    {
        if (!v) return;
        HideAll();
        deck.gameObject.SetActive(true);
    }

    public void ShowCredit(bool v)
    {
        if (!v) return;
        HideAll();
        credits.gameObject.SetActive(true);
        menu.gameObject.SetActive(false);
    }

    public void ShowBattle(bool v)
    {
        if (!v) return;

        if (isDeckComplete())
        {
            BattleCanvas.gameObject.SetActive(true);
        }
        else
        {
            emptySlotPopup.SetTrigger("pop");
            deckButton.isOn = true;
            
        }

        
    }

    public bool isDeckComplete()
    {
        var id = dbHelper.GetActiveDeck().ID;

        for (int i = 0; i < 8; i++)
        {
            var card = dbHelper.GetCardFromSlot(id, i);
            if (card == null) return false;
        }

        return true;
    }

}
