using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcaneLib;
using System;
using Random = UnityEngine.Random;
#pragma warning disable 0649
[DisallowMultipleComponent]
public class Mage : MonoBehaviour
{
    public float hp;
    public float mana;
    public bool playerControlled = false;
    public float awatwaitingTime = 3;

    [Header("Events")]
    public GameEvent cardSelectionEvent;
    public GameEvent lineSelectionEvent;
    public GameEvent castStartEvent;
    public GameEvent castEndEvent;


    [Header("Plugs")]
    public MageData mageData;
    public GameEvent OnDieEvent;
    public Transform leftCastSpot;
    public Transform centerCastSpot;
    public Transform rightCastSpot;
    public Transform mage;
    [HideInInspector]public Transform enemy;
    public Animator animator;
    public DbHelper dbHelper;
    

    

    [ReadOnly] public float silence = 0;


    [SerializeField] private float manaRegenPerSecond;
    public ScriptableDeck deck;
    [SerializeField] [ReadOnly] private bool canCast = true;
    [SerializeField] [ReadOnly] private int nextCard;
    [SerializeField] [ReadOnly] private CardLine nextLine;
    [SerializeField] [ReadOnly] private ScriptableCard lastCard;


    [SerializeField] [ReadOnly] public List<ScriptableCard>handCards = new List<ScriptableCard>(new ScriptableCard[5]);

    [Space]
    [Header("Audios")]
    public AudioSource audioSource;
    public AudioClip lowManaAudio;



    internal void OnHeal(float v)
    {
        this.hp += v;
        if (this.hp > 100) this.hp = 100;
    }

    private CardStateManager manager;
    private OCard currentCard;
    private bool cardChoosed = false;



    private void Awake()
    {
        foreach (Mage m in FindObjectsOfType<Mage>())
        {
            if (m == this) continue;
            enemy = m.mage;
        }

    }

    private void Start()
    {
        
        var model = Instantiate(mageData.model,transform,false);
        animator = model.GetComponent<Animator>();
        
        

        manager = FindObjectOfType<CardStateManager>();

        PopulateHand();
        StartCoroutine(Turn());
        StartCoroutine(RegenerateMana());
    }

    private void Update()
    {
        if (silence > 0) silence -= Time.deltaTime;
    }

    public void Silence(float duration)
    {
        silence = duration;
    }


    public void PopulateHand()
    {
        for (int i = 0; i < handCards.Count;)
        {
            var rnd = Random.Range(0, 8);

            var card = playerControlled? dbHelper.GetCardFromSlot(rnd):deck.cards[rnd];
            if (handCards.Contains(card)) continue;
            handCards[i++] = card;
        }
    }

    public IEnumerator Turn()
    {
        while (true)
        {

            yield return new WaitUntil(() => canCast);
            if (playerControlled) continue;
            if (!this.gameObject.activeSelf) break;
            
            
            
            if (!cardChoosed)
            {
                ChooseCard();
                ChooseSide();
            }

            yield return new WaitForSeconds(awatwaitingTime);

            CastCard();
        }
        
    }

    public IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitUntil(() => canCast);
            if (mana < 10)
                mana += manaRegenPerSecond * Time.deltaTime;
        }

    }



    private void ChooseCard()
    {
        nextCard = Random.Range(0, handCards.Count-1);
        cardChoosed = true;
        cardSelectionEvent?.FireEvent(handCards[nextCard]);
    }

    private void ChooseSide()
    {
        
        CardLine line;
        bool lineChoosed = false;
        do {
            
            int side = Random.Range(0, 5);

            switch (side)
            {
                case 0:
                    line = CardLine.LEFT;
                    break;
                case 1:
                    line = CardLine.CENTER;
                    break;
                case 2:
                    line = CardLine.RIGHT;
                    break;
                case 3:
                    line = CardLine.MAGE;
                    break;
                case 4:
                    line = CardLine.ENEMY;
                    break;
                default:
                    line = CardLine.LEFT;
                    break;
            }

            if (handCards[nextCard].acceptableLines.HasFlag(line))
            {
                nextLine = line;
                lineChoosed = true;
            }
            
        } while (!lineChoosed);

        
        
    }

    private bool CheckSide(CardLine nextLine)
    {
        bool lineEmpty = true;
       
        switch (nextLine)
        {
            case CardLine.LEFT:
                lineEmpty = leftCastSpot.childCount <= 0;
                Game.Instance.RemoveObject(leftCastSpot.GetChild(0).GetComponent<EntityObject>());
                break;
            case CardLine.CENTER:
                lineEmpty = centerCastSpot.childCount <= 0;
                Game.Instance.RemoveObject(centerCastSpot.GetChild(0).GetComponent<EntityObject>());
                break;
            case CardLine.RIGHT:
                lineEmpty = rightCastSpot.childCount <= 0;
                Game.Instance.RemoveObject(rightCastSpot.GetChild(0).GetComponent<EntityObject>());
                break;
            default:
                lineEmpty = false;
                break;
        }

        return lineEmpty;

    }

    private void CastCard() {
        CastCard(nextCard, nextLine);
    }

    public void CastCard(int nextCard,CardLine nextLine)
    {
        if (this.mana < handCards[nextCard].mana)
        {
            if(lowManaAudio!=null)
                audioSource.PlayOneShot(lowManaAudio);

            return;
        };

        if (this.silence > 0) return;
        //CheckSide(nextLine);
        lineSelectionEvent?.FireEvent(nextLine);


        manager.Cast(handCards[nextCard], nextLine, this, OnCastEnd);
        animator.SetBool("isCasting", true);
        animator.SetFloat("Blend", (int)handCards[nextCard].animation);
        canCast = false;
        this.mana -= handCards[nextCard].mana;
        lastCard = handCards[nextCard];
        handCards[nextCard] = null;
        cardChoosed = false;
        this.nextCard = nextCard;

        castStartEvent?.FireEvent(lastCard.cast);
        

        DrawCard();
    }

    private void DrawCard()
    {
        bool cardDrawed = false;
        do
        {
            var rnd = Random.Range(0, 8);

            var card = playerControlled ? dbHelper.GetCardFromSlot(rnd) : deck.cards[rnd];

            if (card == lastCard) continue;
            if (handCards.Contains(card)) continue;
            handCards[nextCard] = handCards[handCards.Count-1];
            handCards[handCards.Count - 1] = card;
            cardDrawed = true;
        } while (!cardDrawed);
    }

    public void OnCastEnd(OCard card)
    {
        canCast = true;
        animator.SetBool("isCasting", false);
        castEndEvent?.FireEvent(null);
    }


    internal void OnTakeDamage(CardController cardController, float v, Elements element, DamageType overTime)
    {
        this.hp -= v;
        animator.SetTrigger("Damage");
        if (this.hp <= 0)
        {
            OnDieEvent?.FireEvent(null);
            this.enabled = false;
        }
    }
}
