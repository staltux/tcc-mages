using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649

public class LevelLoader : MonoBehaviour
{
    public DbHelper dbHelper;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI textProgress;
    [SerializeField] private Canvas sceneLoaderCanvas;
    [SerializeField] private GameObject sceneLoaderPanel;

    [SerializeField] private Image art;
    [SerializeField] private Sprite[] images;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string[] messages;



    public string CurrentLevel { get { return SceneManager.GetActiveScene().name; } }

    public enum SCENES
    {
        PRECONFIG = 0,
        DECK,
        BATTLE,
        TUTORIAL,
        VERSUS,
        MAIN
    }

    private void OnEnable()
    {
        var rnd = Random.Range(0, images.Length);
        art.sprite = images[rnd];

        rnd = Random.Range(0, messages.Length);
        text.text = messages[rnd];
    }

    public void Load(SCENES sceneIndex)
    {
        Load(sceneIndex, true);
        
    }

    public void Load(SCENES sceneIndex,bool activeCanvas)
    {
        sceneLoaderCanvas.gameObject.SetActive(true);
        sceneLoaderPanel.gameObject.SetActive(activeCanvas);
        
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public void LoadBattle()
    {
        if(dbHelper.IsActiveDeckCompleted())
            Load(SCENES.BATTLE,false);
    }

    public void LoadVersus()
    {
        if (dbHelper.IsActiveDeckCompleted())
            Load(SCENES.VERSUS);
    }

    public void LoadTutorial()
    {
        Load(SCENES.TUTORIAL);
    }

    private IEnumerator LoadAsync(SCENES sceneIndex)
    {
        
        AsyncOperation op = SceneManager.LoadSceneAsync((int)sceneIndex);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);

            if (slider != null) slider.value = progress;
            if (textProgress != null) textProgress.text = (int)(progress * 100.0f) + "%";

            yield return null;
        }
    }
    
    /*
    public void Load(int sceneIndex)
    {
        sceneLoaderCanvas.gameObject.SetActive(true);
        levelToLoad = sceneIndex;
        SceneManager.LoadScene(0);
    }

    */


    
}
