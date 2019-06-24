using ArcaneLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardPanel : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public DbHelper dbHelper;
    public GameObject cardPrefab;
    public DeckEditView deckEditView;
    public CardFullViewScript cardFullViewScript;
    public Animator warning;

    private void OnEnable()
    {
        Populate();
    }

    private void Start()
    {
        
    }

    private void ClearView()
    {
        foreach (Transform child in gridLayout.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Populate()
    {
        
        ClearView();
        var cards = dbHelper.GetPlayerCards() as List<ScriptableCard>;
        var deck = dbHelper.GetActiveDeck();
        List<ScriptableCard> dCards = dbHelper.GetCardsFromDeck(deck);

        cards = cards.Except(dCards).ToList();

        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];

            var view = Instantiate(cardPrefab);
            
            var art = Helper.FindComponentInChildrenWithName<Image>(view, "*Art");
            art.sprite = card.art;

            var block = Helper.FindComponentInChildrenWithName<Image>(view, "*Block");
            if (CanEquipCard(card))
            {
                block.enabled = false;
            }else block.enabled = true;

            view.GetComponent<AButton>().onClick.RemoveAllListeners();

            if (CanEquipCard(card))
            {
                view.GetComponent<AButton>().onClick.AddListener(delegate ()
                {
                    var selectedCard = card;

                    if (block.enabled) DoLongPress(selectedCard);
                    else DoSimplePress(selectedCard);

                });
            }
            else
            {
                view.GetComponent<AButton>().onClick.AddListener(delegate ()
                {
                    warning.SetTrigger("pop");
                });
                
            }

     

            view.GetComponent<AButton>().OnLongPress.AddListener(delegate ()
            {
                var selectedCard = card;
                DoLongPress(selectedCard);
            });
            
            view.transform.SetParent(gridLayout.transform, false);
        }
    }

    private bool CanEquipCard(ScriptableCard card)
    {
        return (card.rank < 3 || card.element == dbHelper.GetActiveMage().element);
    }

    private void DoSimplePress(ScriptableCard selectedCard)
    {
        deckEditView.gameObject.SetActive(true);
        deckEditView.SetCard(selectedCard);
    }
    private void DoLongPress(ScriptableCard selectedCard)
    {
        cardFullViewScript.gameObject.SetActive(true);
        cardFullViewScript.SetCard(selectedCard);
    }
}
