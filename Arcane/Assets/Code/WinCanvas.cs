using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvas : MonoBehaviour
{
    public GameEvent OnPlayerWin;
    public GameEvent OnPlayerLose;
    public Animator animator;
    public TextMeshProUGUI stars;
    public TextMeshProUGUI[] slots;
    public Image[] slotImages;
    public Sprite goldSprite;
    public Sprite chestSprite;
    public Sprite keySprite;

    public TextMeshProUGUI xp;
    public TextMeshProUGUI level;
    public Image slider;

    public GameObject winText;
    public GameObject loseText;

    public Button continueButton;
    public LevelLoader levelLoader;
    public DbHelper dbHelper;

    [Space]
    [Header("Audios")]
    public AudioSource audioSource;
    public AudioClip[] starsAudio;
    public AudioClip winAudio;
    public AudioClip loseAudio;
    public AudioSource musicPlayer;


    private void OnDestroy()
    {
        OnPlayerWin.RemoveListener(OnPlayerWinCallBack);
        OnPlayerLose.RemoveListener(OnPlayerLoseCallBack);
    }

    private void Awake()
    {
        OnPlayerWin.AddListener(OnPlayerWinCallBack);
        OnPlayerLose.AddListener(OnPlayerLoseCallBack);
        this.gameObject.SetActive(false);
        continueButton.onClick.AddListener(delegate() {
            levelLoader.Load(LevelLoader.SCENES.DECK);
        });
    }

    private void DisplayXP()
    {
        var lvl = (int)(dbHelper.XP / 1000);
        level.text = "Nv" + lvl++;
        xp.text = string.Format("{0}/{1}", dbHelper.XP, lvl * 1000);
        slider.fillAmount = (float)dbHelper.XP / (lvl * 1000);
    }

    public void OnPlayerWinCallBack(object data)
    {
        var godies = data as WinnerGodies;

        this.gameObject.SetActive(true);
        animator.SetInteger("StarNumber", godies.starts);
        audioSource.PlayOneShot(starsAudio[godies.starts-1]);


        musicPlayer.Stop();
        musicPlayer.PlayOneShot(winAudio);

        stars.text = "Estrelas: " + godies.starts.ToString();

        DisplayXP();

        for (int i = 0; i < slots.Length; i++)
        {
            if (godies.types[i] != GodieType.NONE)
            {
                slots[i].text = (godies.types[i] == GodieType.OURO) ? godies.values[i].ToString() : godies.types[i].ToString();
                slotImages[i].sprite = (godies.types[i] == GodieType.OURO) ? goldSprite : (godies.types[i] == GodieType.LIVRO) ? chestSprite : keySprite;
            }
        }

        winText.SetActive(true);
    }

    public void OnPlayerLoseCallBack(object data)
    {
        this.gameObject.SetActive(true);
        animator.SetInteger("StarNumber",0);

        musicPlayer.Stop();
        musicPlayer.PlayOneShot(loseAudio);

        stars.text = "Estrelas: " + 0;

        DisplayXP();

        loseText.SetActive(true);
    }
}
