using ArcaneLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public GameEvent enemyDieEvent;
    public GameEvent playerWinEvent;
    public GameEvent playerDieEvent;
    public GameEvent playerLoseEvent;
    public GameEvent timerOverEvent;

    public Canvas battleCanvas;
    
    public LevelLoader levelLoader;
    public DbHelper dbHelper;

    public Mage player;
    public Mage enemy;
    public Game game;
    public MageList mageList;
    public CardStateManager csm;

    private void OnEnable()
    {
        enemyDieEvent.AddListener(OnEnemyDie);
        playerDieEvent.AddListener(OnPlayerDie);
        timerOverEvent.AddListener(OnTimeOver);
    }
    private void OnDisable()
    {
        enemyDieEvent.RemoveListener(OnEnemyDie);
        playerDieEvent.RemoveListener(OnPlayerDie);
        timerOverEvent.RemoveListener(OnTimeOver);
    }

    private void Awake()
    {    
        player.mageData = dbHelper.GetActiveMage();

        int rnd = 0;

        if (Versus.enemy == null)
        {
            rnd = Random.Range(0, mageList.mages.Count);
            Versus.enemy = mageList.mages[rnd];
        }

        enemy.mageData = Versus.enemy;

        rnd = Random.Range(0, enemy.mageData.decks.Length);
        enemy.deck = enemy.mageData.decks[rnd];
    }


    public void OnTimeOver(object data)
    {
        if (player.hp > enemy.hp) OnEnemyDie(null);
        else OnPlayerDie(null);
    }


    public void OnEnemyDie(object data)
    {
        GameOver();
       

        var godies = GetGodies();


        dbHelper.Gold += godies.Gold;
        dbHelper.Key += godies.Key;
        dbHelper.Chest += godies.Chest;
        dbHelper.XP += godies.xp;

        playerWinEvent?.FireEvent(godies);
        
        //StartCoroutine(ToDeck());

    }

    private void GameOver()
    {
        Destroy(player);
        Destroy(enemy);
        Destroy(csm);
        

        game.RemoveAllSpeels();
        battleCanvas.gameObject.SetActive(false);
    }

    public void OnPlayerDie(object data)
    {
        GameOver();
        playerLoseEvent?.FireEvent(null);
    }

    public WinnerGodies GetGodies()
    {
        WinnerGodies godies = new WinnerGodies();
        godies.starts = 1;

        if (player.hp > 40 && player.hp <= 69) godies.starts = 2;
        if (player.hp >= 70) godies.starts = 3;




        int slot = 0;
        godies.xp = godies.starts * 100;

        godies.types[slot] = GodieType.OURO;
        godies.values[slot] = Random.Range(100, 301);

        if (godies.starts < 2) return godies;
        slot++;

        var gold_chest = Random.Range(0, 101);

        if (gold_chest <= 60)
        {
            godies.types[slot] = GodieType.OURO;
            godies.values[slot] = Random.Range(100, 301);
        }
        else
        {
            if (dbHelper.Chest > 0 && dbHelper.XP < 5000 || dbHelper.Chest > 1 && dbHelper.XP < 15000 || dbHelper.Chest > 2)
            {
                godies.types[slot] = GodieType.OURO;
                godies.values[slot] = Random.Range(100, 301);
            }
            else
            {
                godies.types[slot] = GodieType.LIVRO;
                godies.values[slot] = 1;
            }
            
        }

        if (godies.starts < 3) return godies;
        slot++;

        if (gold_chest <= 20)
        {
            godies.types[slot] = GodieType.CHAVE;
            godies.values[slot] = 1;
        }
        else if (gold_chest < 50)
        {
            if (dbHelper.Chest > 0 && dbHelper.XP < 5000 || dbHelper.Chest > 1 && dbHelper.XP < 15000 || dbHelper.Chest > 2)
            {
                godies.types[slot] = GodieType.OURO;
                godies.values[slot] = Random.Range(100, 301);
            }
            else
            {
                godies.types[slot] = GodieType.LIVRO;
                godies.values[slot] = 1;
            }
            
        }
        else
        {
            godies.types[slot] = GodieType.OURO;
            godies.values[slot] = Random.Range(100, 301);
        }

        return godies;
    }

    public void GoToDeck()
    {
        levelLoader.Load(LevelLoader.SCENES.DECK);
    }

}
