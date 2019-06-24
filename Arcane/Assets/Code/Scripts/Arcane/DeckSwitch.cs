using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneLib
{
    public class DeckSwitch : MonoBehaviour
    {
        public DbHelper dbHelper;
        public Button button;
        public Text buttonText;
        public GameEvent mageSelectEvent;
        

        public DeckSelectPopup deckSelectPopup;

        
        private void OnDestroy()
        {
            mageSelectEvent.RemoveListener(OnMageSelect);
        }

        void Awake()
        {

            mageSelectEvent.AddListener(OnMageSelect);
            //Player.Instance.OnDecksChange += ReloadDecks;
            button.onClick.AddListener(delegate() {
                deckSelectPopup.Populate(dbHelper.GetDecks());
                deckSelectPopup.IsShowing = true;
            });



        }

        private void Start()
        {
            
            buttonText.text = dbHelper.GetActiveDeck().title;
        }

        public void OnMageSelect(object data)
        {
            buttonText.text = dbHelper.GetActiveDeck().title;
            deckSelectPopup.Populate(dbHelper.GetDecks());
        }



        public void ReloadDecks(Deck[] decks)
        {
            if (decks == null) return;

            
            if (decks.Length > 0)
            {
                buttonText.text = decks[0].title;
            }

            foreach (Deck deck in decks)
            {
                if (dbHelper.GetActiveDeck().ID==deck.ID)
                {
                    buttonText.text = deck.title;
                    //Player.Instance.ActiveDeck = deck.id;
                }
            }

            

        }

    }
}