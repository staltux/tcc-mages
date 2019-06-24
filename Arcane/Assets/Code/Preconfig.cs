using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Preconfig : MonoBehaviour
{
    public LevelLoader levelLoader;
    public DbHelper dbHelper;


    void Start()
    {
        if(dbHelper.HaveCompletedTutorial)
            levelLoader.Load(LevelLoader.SCENES.MAIN);

        else levelLoader.Load(LevelLoader.SCENES.TUTORIAL);

    }


}
