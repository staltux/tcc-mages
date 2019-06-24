using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public ScreenManager manager;

    [ReadOnly] public int currentStage = 0;

    public Button[] nextPrevius;
    public GameObject[] stages;

    public void OnEnd()
    {
        manager.ShowDeck(true);
    }

    private void Awake()
    {
        //ResetStagePositions();
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
        nextPrevius[0].interactable = !(i <= 0);
        nextPrevius[1].interactable = !(i >= stages.Length-1);
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


    public void UpdateTutorial(int delta)
    {
        if (currentStage + delta >= stages.Length || currentStage + delta < 0) return;

        HideStage(currentStage);
        currentStage += delta;
        ShowStage(currentStage);
    }

    void HideStage(int i)
    {
        stages[i].SetActive(false);
    }

}
