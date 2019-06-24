using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ArcaneLib
{
    public class ActiveDeckView : WindowFrame
    {

        public GridLayoutGroup grid;
        public GameObject cardPrefab;
        public DeckEditView deckEditView;
        public GridLayoutGroup gridOfCurretDeck;
        public DbHelper dbHelper;
        public GameEvent mageSelectEvent;
        public Sprite emptyBackground;
        public TextMeshProUGUI manaText;
        public AudioSource audioSource;
        public AudioClip lowLevelAudio;
        public PlayerCardPanel playerCardPanel;

        public override bool IsShowing
        {
            get { return animator.GetBool("isShowing"); }
            set { animator?.SetBool("isShowing", value); if (!value) Clean(); }
        }

        private void OnDestroy()
        {
            mageSelectEvent.RemoveListener(ShowCardsForAllSlots);
        }



        protected override void Awake()
        {
            base.Awake();
            mageSelectEvent.AddListener(ShowCardsForAllSlots);
            this.IsShowing = true;

        }


        private void Start()
        {
            ConfigureSlots();
        }

        void ConfigureSlots()
        {

            var count = gridOfCurretDeck.transform.childCount;


            for (int i = 0; i < count; i++)
            {
                ShowCardInSlot(i);
                var slot = i;
                gridOfCurretDeck.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    
                    if (IsShowing)
                    {
                        ScriptableCard c = deckEditView.GetCard();
                        if (c.element == dbHelper.GetActiveMage().element || c.rank < 3)
                        {
                            dbHelper.AddCardInSlot(c.UUID, slot);
                            IsShowing = false;
                            deckEditView.IsShowing = false;
                            playerCardPanel.Populate();
                        }
                            
                            
                        else audioSource.PlayOneShot(lowLevelAudio);
                        ShowCardsForAllSlots(null);
                    }
                    else
                    {
                        deckEditView.gameObject.SetActive(true);
                        deckEditView.SetCard(dbHelper.GetCardFromSlot(slot) as ScriptableCard);
                        deckEditView.IsShowing = true;
                    }

                });
            }
            manaText.text = string.Format("Custo arcano médio: {0:0.0}", dbHelper.GetActiveDeck().CalculateMana(dbHelper));

        }

        public void ShowCardsForAllSlots(object data)
        {
            var count = gridOfCurretDeck.transform.childCount;


            for (int i = 0; i < count; i++)
            {
                ShowCardInSlot(i);
            }

        }

        private void ShowCardInSlot(int slot)
        {
            var art = Helper.FindComponentInChildrenWithName<Image>(gridOfCurretDeck.transform.GetChild(slot).gameObject, "*Art");
            var card = dbHelper.GetCardFromSlot(slot) as ScriptableCard;
            if (card != null) art.sprite = card.art;
            else art.sprite = emptyBackground;
        }

        public void OnCardsUpdate(ScriptableCard[] cards)
        {
            Debug.LogWarning("OnCardsUpdate");
            foreach (Transform child in grid.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            var i = 0;
            foreach (var card in cards)
            {
                
                var view = Instantiate(cardPrefab);
                
                var art = Helper.FindComponentInChildrenWithName<Image>(view, "*Art");
                art.sprite = card.art;

                view.GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    if (IsShowing)
                    {
                        ScriptableCard c = deckEditView.GetCard();
                        Debug.LogWarning("SetCardToCurrentDeckInPosition not implemented");
                        //Server.SetCardToCurrentDeckInPosition(this, c, i, Player.Instance.ReloadDecks);
                    }
                    else
                    {
                        deckEditView.gameObject.SetActive(true);
                        deckEditView.SetCard(card);
                        deckEditView.IsShowing = true;
                    }

                });

                view.transform.SetParent(grid.transform, false);

                i++;
            }


        }
    }
}