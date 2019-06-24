using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    public DbHelper dbHelper;
    public AudioSource audioSource;
    public LevelLoader levelLoader;

    [Space]
    [Header("Audios")]
    [Tooltip("O Primeiro audio toca somente na primeira vez")]
    public AudioClip[] welcomes;

    private void Awake()
    {
        
    }

    private void Start()
    {
        if (dbHelper.FirstTime)
        {
            audioSource.PlayOneShot(welcomes[0]);
            dbHelper.FirstTime = false;
        }
        else {
            var rnd = Random.Range(1,welcomes.Length);
            audioSource.PlayOneShot(welcomes[rnd]);
        }
        
    }


    public void OnNewGame()
    {
        dbHelper.EraseSave();
        levelLoader.Load(LevelLoader.SCENES.PRECONFIG);
    }

    public void OnContinue()
    {
        levelLoader.Load(LevelLoader.SCENES.DECK);
    }
}
