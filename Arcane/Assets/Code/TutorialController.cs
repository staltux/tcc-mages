using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] stages;
    public LevelLoader levelLoader;

    public GameObject stageToGainGold;
    public int  goldEaned = 5000;

    public GameEvent mageSelectEvent;

    public ScriptableCardList cardList;
    public DbHelper dbHelper;

    [ReadOnly] public int currentStage = 0;

    public Button[] nextPrevius;


    private void OnEnable()
    {
        mageSelectEvent.AddListener(MageSelected);
    }

    private void OnDisable()
    {
        mageSelectEvent.RemoveListener(MageSelected);
    }

    private void Awake()
    {
        ResetStagePositions();
        HideAllStages();

        ShowStage(currentStage);
    }


    void ResetStagePositions()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            var rect = stages[i].GetComponent<RectTransform>();
            rect.offsetMax = rect.offsetMin = Vector2.zero;
        }
    }

    void HideAllStages()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(false);
        }
    }

    void ShowStage(int i)
    {
        stages[i].SetActive(true);
    }

    public void Next()
    {
        UpdateTutorial(1);
    }

    public void Previous()
    {
        UpdateTutorial(-1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        UpdateTutorial(1);

    }

    public void UpdateTutorial(int delta)
    {
        if (currentStage + delta >= stages.Length || currentStage + delta < 0) return;

        HideStage(currentStage);
        currentStage += delta;
        ShowStage(currentStage);

        if (stages[currentStage] == stageToGainGold)
        {

            if (dbHelper.HaveCompletedTutorial)
            {

                HideStage(currentStage);
                levelLoader.Load(LevelLoader.SCENES.DECK);
                return;
            }

            nextPrevius[0].gameObject.SetActive(false);
            nextPrevius[1].gameObject.SetActive(false);

            dbHelper.Gold = goldEaned;
            //dbHelper.Chest = 3;
            //dbHelper.Key = 3;
            //dbHelper.XP = 16000;
        }
    }

    void HideStage(int i)
    {
        stages[i].SetActive(false);
    }

    public void MageSelected(object playerSelection)
    {

        Debug.Log("MageSelected " + levelLoader.CurrentLevel);
        if ((bool)playerSelection) { GiveCards(); }



        //Deck deck = new Deck();
        //deck.title = "Deck " + dbHelper.GetActiveMage().title;
        //deck.mage = PlayerData.ActiveMage;
        //for (int i = 0; i < cards.Count; i++)
        //{
        //  deck.SetCardInSlot(cards[i],i);
        //}

        //dbHelper.CreateDeck(deck.title);
        //PlayerData.ActiveDeck = deck;

        if (!dbHelper.HaveCompletedTutorial)
        {
            dbHelper.HaveCompletedTutorial = true;
            levelLoader.Load(LevelLoader.SCENES.MAIN);
        }else levelLoader.Load(LevelLoader.SCENES.DECK);


    }

    private void GiveCards()
    {

        //var save = SaveManager.CreateNewSave();
        //save.
        var slot = 0;
        var list = new List<ScriptableCard>();
        var exclude = new List<ScriptableCard>();
        var cards = new List<ScriptableCard>();

        for (int i = 0; i < cardList.cards.Length; i++)
        {
            list.Add(cardList.cards[i]);
        }

        int c = 4;


        while (c > 0)
        {
            var idx = Random.Range(0, list.Count);
            var card = list[idx];


            if (card.element == dbHelper.GetActiveMage().element || card.rank <= 2)
            {
                dbHelper.AddCard(card.UUID, card.title);
                dbHelper.AddCardInSlot(card.UUID, slot++);
                cards.Add(card);
                c--;
            }
            else
            {
                exclude.Add(card);
            }

            list.RemoveAt(idx);
        }

        list.AddRange(exclude);
        exclude.Clear();
        c = 4;

        while (c > 0)
        {
            var idx = Random.Range(0, list.Count);
            var card = list[idx];


            if (card.element == dbHelper.GetActiveMage().element)
            {
                dbHelper.AddCard(card.UUID, card.title);
                dbHelper.AddCardInSlot(card.UUID, slot++);
                cards.Add(card);
                c--;
            }
            else
            {
                exclude.Add(card);
            }

            list.RemoveAt(idx);
        }
        list.AddRange(exclude);

        c = 4;

        while (c > 0)
        {
            var idx = Random.Range(0, list.Count);
            var card = list[idx];

            dbHelper.AddCard(card.UUID, card.title);
            
            cards.Add(card);
            c--;

            list.RemoveAt(idx);
        }
    }

}
