using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchScreen : MonoBehaviour
{
    public LevelLoader levelLoader;

    public void OnGoBack()
    {
        levelLoader.Load(LevelLoader.SCENES.DECK);
    }
    public void OnGoBattle()
    {
        levelLoader.Load(LevelLoader.SCENES.VERSUS);
    }
}
