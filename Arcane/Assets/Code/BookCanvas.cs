using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BookCanvas : MonoBehaviour
{
    public DbHelper dbHelper;
    public Button[] slots;
    public GameObject[] locks;
    public Button bookButton;
    public Sprite bookSprite;
    public Sprite empty;
    public TextMeshProUGUI key;
    public GameObject bookName;
    public GameObject openMessage;
    public string noKeyText;
    public GameObject book;

    public GameObject cardPanel;
    public Image cardImage;

    public GameObject bookSlot;
    public Animator animator;

    public TextMeshProUGUI KeyAmouth;
    public GameEvent currency;

    [Space]
    [Header("Audios")]
    public AudioSource audioSource;
    public AudioClip fourStarsAudio;
    public AudioClip[] starsAudio;
    public AudioClip openBookAudio;
    public AudioClip[] spoilsAudio;

    public ScriptableCard lastCard;
    public GameObject menu;

    private void OnEnable()
    {
        cardPanel.SetActive(false);

        if (dbHelper.Chest > 0 && dbHelper.Key > 0)
        {
            var rnd = Random.Range(0, spoilsAudio.Length);
            audioSource.PlayOneShot(spoilsAudio[rnd]);
        }

        Rearm();
    }

    public void OnSlotSelected()
    {
        bookButton.gameObject.SetActive(true);
        bookButton.interactable = true;
        bookName.SetActive(true);
        book.gameObject.SetActive(true);
        animator.SetBool("closed", true);

        if (dbHelper.Key > 0)
        {
            openMessage.SetActive(true);
        }
        
    }

    public void OnBookSelected()
    {
        
        if (dbHelper.Key <= 0)
        {
            key.text = noKeyText;
            return;
        }
        openMessage.SetActive(false);
        menu.SetActive(false);
        dbHelper.Key--;
        dbHelper.Chest--;
        lastCard = null;

        var list = new List<ScriptableCard>(dbHelper.cardList.cards);
        var playerCards = dbHelper.GetPlayerCards();
        list.RemoveAll(c=>playerCards.Contains(c));


        if (list.Count <= 0)
        {
            dbHelper.Gold += 300;
            currency?.FireEvent(null);
        }
        else
        {
            var rnd = Random.Range(0, list.Count);
            var card = list[rnd];

            dbHelper.AddCard(card.UUID, card.title);

            cardImage.sprite = card.art;

            lastCard = card;
        }


        bookSlot.SetActive(false);
        animator.SetBool("closed",false);

        if(openBookAudio!= null)
            audioSource.PlayOneShot(openBookAudio);


        Rearm();
    }

    public void OnAnimationEnd()
    {
        menu.SetActive(true);
        bookSlot.SetActive(true);
        
        animator.SetBool("closed", true);
        book.gameObject.SetActive(false);

        if (lastCard == null) return;

        cardPanel.SetActive(true);


        if (lastCard.rank < 4 && starsAudio.Length > 0)
        {
            var rnd = Random.Range(0, starsAudio.Length);
            audioSource.PlayOneShot(starsAudio[rnd]);
        }
        else if (fourStarsAudio != null)
        {
            audioSource.PlayOneShot(fourStarsAudio);
        }

    }

    public void GiveACard()
    {
        dbHelper.Gold += 5000;
        var list = new List<ScriptableCard>(dbHelper.cardList.cards);
        var playerCards = dbHelper.GetPlayerCards();
        list.RemoveAll(c => playerCards.Contains(c));

        if (list.Count <= 0) { Debug.Log(string.Format("OnBookSelected({0})", 300)); dbHelper.Gold += 300; Rearm(); return; }

        var rnd = Random.Range(0, list.Count);
        var card = list[rnd];
        Debug.Log(string.Format("OnBookSelected({0})", card));
        dbHelper.AddCard(card.UUID, card.title);

        cardImage.sprite = card.art;
        cardPanel.SetActive(true);

        dbHelper.Chest += 3;
        dbHelper.Key += 3;

        Rearm();

    }

    private void Rearm()
    {
        var chests = dbHelper.Chest;
        var level = dbHelper.XP / 1000;

        slots[0].interactable = chests > 0 ? true : false;
        slots[0].GetComponent<Image>().sprite = chests > 0 ? bookSprite : empty;


        slots[1].interactable = level >= 5 && chests > 1;
        locks[0].SetActive(!(level >= 5));
        slots[1].GetComponent<Image>().sprite = chests > 1 ? bookSprite : empty;


        slots[2].interactable = level >= 15 && chests > 2;
        locks[1].SetActive(!(level >= 15));
        slots[2].GetComponent<Image>().sprite = chests > 2 ? bookSprite : empty;


        bookButton.gameObject.SetActive(false);
        bookButton.interactable = false;

        KeyAmouth.text = string.Format("x{0}",dbHelper.Key);
        
        key.text = "";
        openMessage.SetActive(false);
        bookName.SetActive(false);
    }
}
