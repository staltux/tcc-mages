using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ArcaneLib
{
    public class CardFullViewScript : WindowFrame
    {

        public DbHelper dbHelper;
        public DeckEditView deckEditView;
        public ActiveDeckView activeDeckView;

        private GridLayoutGroup grid;
        
        
        public AButton cardButton;
        public DeckEditor deckEditor;

        public Animator warning;

        protected override void Awake()
        {
            base.Awake();

            
            //grid = Helper.FindComponentInChildrenWithName<GridLayoutGroup>(this.gameObject, "*Grid");
            //cardButton = Helper.FindComponentInChildrenWithName<AButton>(this.gameObject, "*Card");
            IsShowing = false;
            //deckEditor = FindObjectOfType<DeckEditor>();
        }


        public void SetCard(ScriptableCard card)
        {
            IsShowing = true;
            Debug.LogWarning("Changed Card to Component " + this.IsShowing + " : " + IsShowing);

            
            var panel = Helper.FindComponentInChildrenWithName<Component>(this.gameObject, "*Card");
            
            Helper.FindComponentInChildrenWithName<Text>(this.gameObject, "*Description").text = card.description;
            Helper.FindComponentInChildrenWithName<Text>(panel.gameObject, "*Title").text = card.title;
            //Helper.FindComponentInChildrenWithName<Image>(panel.gameObject, "*Border").sprite = card.border;
            //Helper.FindComponentInChildrenWithName<Text>(panel.gameObject, "*Amount").text = card.amount.ToString();

            var art = Helper.FindComponentInChildrenWithName<Image>(panel.gameObject, "*Art");
            art.sprite = card.art;
            //cardInView = card.id;
            cardButton.onClick.RemoveAllListeners();

            /*
            cardButton.onClick.AddListener(delegate ()
            {
                var selected = card;

                if (CanEquipCard(selected))
                {
                    deckEditView.SetCard(selected);

                    IsShowing = false;
                    deckEditor.PrepareToChangeCard();
                    activeDeckView.IsShowing = false;
                }
                else
                {
                    warning.SetActive(true);
                }

                
            });
            
            */

            if (CanEquipCard(card))
            {

                cardButton.onClick.AddListener(delegate ()
                {

                    deckEditView.SetCard(card);

                    IsShowing = false;
                    deckEditor.PrepareToChangeCard();
                });

                activeDeckView.IsShowing = false;
            }
            else
            {
                cardButton.onClick.AddListener(delegate ()
                {
                    warning.SetTrigger("pop");

                });
            }


            /*
            
            cardButton.onClick.AddListener(delegate ()
            {

                deckEditView.SetCard(card);

                IsShowing = false;
                deckEditor.PrepareToChangeCard();
            });

            activeDeckView.IsShowing = false;
            */




        }

        private bool CanEquipCard(ScriptableCard card)
        {
            return (card.rank < 3 || card.element == dbHelper.GetActiveMage().element);
        }


    }
}