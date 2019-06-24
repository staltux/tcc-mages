using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VersusScreen : MonoBehaviour
{
    public VoiceMatch[] matches;
    public MageList mageList;
    public DbHelper dbHelper;
    public AudioSource audioSource;
    public Image playerImage;
    public Image enemyImage;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI enemyText;
    public LevelLoader levelLoader;

    private bool isTellerDone = false;

    //private Dictionary<Tuple<MageData, MageData>, VoiceMatch> voices;

    private void Awake()
    {
        var voices = new Dictionary<Tuple<MageData, MageData>, VoiceMatch>();

        for (int i = 0; i < matches.Length; i++)
        {
            voices.Add(new Tuple<MageData, MageData>(matches[i].player, matches[i].enemy),matches[i]);
        }



        var rnd = UnityEngine.Random.Range(0, mageList.mages.Count);
        Versus.enemy = mageList.mages[rnd];
        var player = dbHelper.GetActiveMage();



        playerImage.sprite = player.versus;
        enemyImage.sprite = Versus.enemy.versus;
        playerText.text = player.title;
        enemyText.text = Versus.enemy.title;



        VoiceMatch match;
        voices.TryGetValue(new Tuple<MageData, MageData>(player, Versus.enemy), out match);
        if (match == null) return;

        audioSource.PlayOneShot(match.audio);
        
    }

    private void Update()
    {
        if (isTellerDone && !audioSource.isPlaying)
        {
            levelLoader.Load(LevelLoader.SCENES.BATTLE,false);
            Destroy(this);
        }
        else if(!audioSource.isPlaying)
        {
            var mage = dbHelper.GetActiveMage();
            var rnd = UnityEngine.Random.Range(0, mage.speaks.Length);
            var audio = mage.speaks[rnd];
            audioSource.PlayOneShot(audio);
            isTellerDone = true;
        }

    }


    [System.Serializable]
    public class VoiceMatch
    {
        public MageData player;
        public MageData enemy;
        public AudioClip audio;
        
    }
}

