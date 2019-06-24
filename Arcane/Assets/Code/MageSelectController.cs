using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MageSelectController : MonoBehaviour
{
    [Header("Personalize Aqui")]


    [Header("Coisas Estaticas, nao mexa")]
    public DbHelper dbHelper;
    public AudioSource audioSource;
    public MageList mageList;
    public Image[] faces;
    public Image[] pictures;
    public TextMeshProUGUI[] titles;
    public TextMeshProUGUI[] subTitles;

    public Animator[] portraits;
    public Animator magePicture;

    public Button selectButton;
    public Button[] picturesButton;

    public TextMeshProUGUI[] selectUnlockText;
    public GameObject[] priceText;

    public GameEvent mageSelectEvent;
    public GameEvent currencyEvent;

    [ReadOnly] public int selectedMage = 0;
    [ReadOnly] public int activeMage = -1;



    private void OnEnable()
    {
        var mages = mageList.mages;
        for (int i = 0; i < mages.Count; i++)
        {
            faces[i].sprite = mages[i].portrait;
            pictures[i].sprite = mages[i].picture;
            titles[i].text = mages[i].title;
            subTitles[i].text = mages[i].subTitle;
        }

        Array.ForEach(portraits, p => { p.SetBool("selected", false); p.SetBool("active", false); });

        for (int i = 0; i < mageList.mages.Count; i++)
        {
            mageList.mages[i].unlocked = dbHelper.HaveMage(mageList.mages[i].UUID);
            priceText[i].SetActive(!dbHelper.HaveMage(mageList.mages[i].UUID));
            
        }


        if (dbHelper.HaveCompletedTutorial)
        {
            SelectMage(dbHelper.GetActiveMage());
            SetActive(false);
        }
            
        else SelectMage(0);
        //SelectMage(SaveManager.ActiveMage);
        //SetActive();
    }


    public void SelectMage(MageData mage)
    {
        var mages = mageList.mages;
        SelectMage(mages.FindIndex(m=>m.UUID.Equals(mage.UUID)));
    }

    public void SelectMage(int idx)
    {
        var mages = mageList.mages;
        selectedMage = (idx % portraits.Length + portraits.Length) % portraits.Length;


        Array.ForEach(portraits,p=> { p.SetBool("selected", false); });
        portraits[selectedMage].SetBool("selected", true);
        
        magePicture.SetInteger("mageSelected", selectedMage);
        selectButton.interactable = !(activeMage == selectedMage);
        picturesButton[selectedMage].interactable = selectButton.interactable;

        selectUnlockText[0].gameObject.SetActive( mages[selectedMage].unlocked );
        selectUnlockText[1].gameObject.SetActive( !mages[selectedMage].unlocked );
    }

    public void SelectPrev()
    {
        SelectMage(selectedMage - 1);
    }

    public void SelectNext()
    {
        SelectMage(selectedMage + 1);
    }

    public void SetActive()
    {
        SetActive(true);
    }

    public void SetActive(bool byplayer)
    {
        var mages = mageList.mages;
        UnlockMage();

        if (!mages[selectedMage].unlocked) return;

        activeMage = selectedMage;
        Array.ForEach(portraits, p => { p.SetBool("active", false); });
        portraits[activeMage].SetBool("active", true);

        dbHelper.SetActiveMage(mages[selectedMage].UUID);

        mageSelectEvent?.FireEvent(byplayer);
        SelectMage(activeMage);


        if (mages[selectedMage].selectSpeaks.Length > 0)
        {
            var rnd = UnityEngine.Random.Range(0, mages[selectedMage].selectSpeaks.Length);
            audioSource.PlayOneShot(mages[selectedMage].selectSpeaks[rnd]);
        }

        for (int i = 0; i < mageList.mages.Count; i++)
        {
            mageList.mages[i].unlocked = dbHelper.HaveMage(mageList.mages[i].UUID);
            priceText[i].SetActive(!dbHelper.HaveMage(mageList.mages[i].UUID));
        }


        //SaveManager.ActiveMage = activeMage;

    }

    private void UnlockMage()
    {
        var mages = mageList.mages;


        if (dbHelper.HaveMage(mages[selectedMage].UUID))
        {
            mages[selectedMage].unlocked = true;

            return;
        }
        

        if (dbHelper.Gold < mages[selectedMage].price) return;


        dbHelper.AddMage(mages[selectedMage].UUID, mages[selectedMage].title);
        dbHelper.SetActiveMage(mages[selectedMage].UUID);

        // move to server, can add mage and get offline, dont paying gold!
        dbHelper.Gold -= mages[selectedMage].price;
        mages[selectedMage].unlocked = true;

        var deckID  = dbHelper.CreateDeck("Deck " + mages[selectedMage].title + " 1");
        dbHelper.SetActiveDeck(deckID);

        dbHelper.CreateDeck("Deck " + mages[selectedMage].title + " 2");

        currencyEvent?.FireEvent(null);
    }


}
