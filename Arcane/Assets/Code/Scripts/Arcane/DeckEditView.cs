
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ArcaneLib
{
    public class DeckEditView : WindowFrame
    {


        public GameObject cardPrefab;

        public CardFullViewScript cardFullViewScript;
        public ActiveDeckView activeDeckView;
        public AButton cardButton;
        int cardInView;
        ScriptableCard card;



        protected override void Awake()
        {
            base.Awake();

            //cardFullViewScript = FindObjectOfType<CardFullViewScript>();
            //activeDeckView = FindObjectOfType<ActiveDeckView>();


            //cardButton = Helper.FindComponentInChildrenWithName<AButton>(this.gameObject, "*Card");
            IsShowing = false;
        }

        public void SetCard(ScriptableCard card)
        {
            var panel = Helper.FindComponentInChildrenWithName<Component>(this.gameObject, "*Card");
            var art = Helper.FindComponentInChildrenWithName<Image>(panel.gameObject, "*Art");
            art.sprite = card.art;
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(delegate ()
            {
                
                cardFullViewScript.SetCard(card);
                

                this.IsShowing = false;
            });
            this.IsShowing = true;
            activeDeckView.IsShowing = true;
            this.card = card;
        }



        protected override void Clean()
        {
            base.Clean();
            activeDeckView.IsShowing = false;
        }

        public ScriptableCard GetCard()
        {
            return card;
        }
    }
}