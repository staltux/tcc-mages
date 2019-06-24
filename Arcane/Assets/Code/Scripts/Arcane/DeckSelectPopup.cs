using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace ArcaneLib
{
    public class DeckSelectPopup : WindowFrame
    {
        public DbHelper dbHelper;
        private VerticalLayoutGroup filterPanel;
        private DeckSwitch deckSwitch;

        public GameObject deckInfoPrefab;
        public ActiveDeckView activeDeckView;
        public PlayerCardPanel cardPanel;


        protected override void Awake()
        {
            base.Awake();



            deckSwitch = FindObjectOfType<DeckSwitch>();

            filterPanel = Helper.FindComponentInChildrenWithName<VerticalLayoutGroup>(this.gameObject, "*DeckFilterPanel");

            this.IsShowing = false;

        }

        public void Populate(Deck[] decks)
        {
            

            var mage = dbHelper.GetActiveMage();

            foreach (Transform child in filterPanel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Deck deck in decks)
            {
                var deckSlot = Instantiate(deckInfoPrefab);

                var inputField = Helper.FindComponentInChildrenWithName<InputField>(deckSlot, "*DeckNameInputField");
                inputField.text = deck.title;

                var manaCost = Helper.FindComponentInChildrenWithName<Text>(deckSlot, "*ManaCost");
                var mana = deck.CalculateMana(dbHelper);
                manaCost.text = (double.IsNaN(mana) ? 0.0 : mana).ToString("0.0"); 

                var icon = Helper.FindComponentInChildrenWithName<Image>(deckSlot, "*Icon");
                icon.sprite = mage.portrait;

                var useButton = Helper.FindComponentInChildrenWithName<Button>(deckSlot, "*UseButton");

                useButton.GetComponentInChildren<Text>().text = dbHelper.GetActiveDeck().ID==deck.ID ? "Em Uso" : "Usar";
                useButton.enabled = !(dbHelper.GetActiveDeck().ID==deck.ID);
                
                var deckToUpdate = deck;


                useButton.onClick.RemoveAllListeners();
                useButton.onClick.AddListener(delegate ()
                {
                    dbHelper.SetActiveDeck(deckToUpdate.ID);
                    deckSwitch.ReloadDecks(dbHelper.GetDecks());
                    Populate(dbHelper.GetDecks());
                    activeDeckView.ShowCardsForAllSlots(null);

                    IsShowing = false;
                    cardPanel.Populate();

                });

                
                inputField.onEndEdit.AddListener(delegate (string action)
                {
                    deckToUpdate.title = action;
                    dbHelper.UpdateDeckTitle(deckToUpdate.ID, deckToUpdate.title);
                    deckSwitch.ReloadDecks(dbHelper.GetDecks());

                });
                deckSlot.transform.SetParent(filterPanel.transform, false);
            }
            
            

        }
        
    }
}