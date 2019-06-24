using ArcaneLib;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditor : MonoBehaviour
{
    public DbHelper dbHelper;

    [Header("Avaliable Cards List")]
    public ScriptableCardList cardlist;
    public GameObject cardPrefab;
    public Sprite[] deckIcons;
    public GridLayoutGroup gridOfCardList;
    [ReadOnly] public ScriptableCard selectedCard;

    public CardFullViewScript cardDetail;
    public DeckEditView deckEditView;

    [Header("List Decks")]
    public Button listDeckButton;
    //public List<Deck> decks;
    //[ReadOnly] public Deck activeDeck;
    public ActiveDeckView activeDeckView;

    [Header("Current Deck")]
    

    public LayoutGroup deckList;
    public GameObject deckInfoPrefab;
    public DeckSelectPopup deckSelectPopup;
    public string manaText = "Custo arcano médio:";


    void Start()
    {
        activeDeckView.OnClose.AddListener(OnActiveDeckClose);
        
        //decks = SaveManager.Decks;
      
        
        SetActiveDeckBarToExpand();
    }


    public void PrepareToChangeCard()
    {
        /*
        var count = gridOfCurretDeck.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            var c = i;
            gridOfCurretDeck.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            gridOfCurretDeck.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate ()
            {
                ChangeCardInSlot(c);
            });
        }
        */
    }



    void SetActiveDeckBarToExpand()
    {
        /*
        var count = gridOfCurretDeck.transform.childCount;
        for (int j = 0; j < count; j++)
        {
            var c = j;
            gridOfCurretDeck.transform.GetChild(c).GetComponent<Button>().onClick.RemoveAllListeners();
            gridOfCurretDeck.transform.GetChild(c).GetComponent<Button>().onClick.AddListener(()=> {
                activeDeckView.IsShowing = true;
            });
        }
        */
        
    }



    void OnActiveDeckClose()
    {
        SetActiveDeckBarToExpand();
    }
}
