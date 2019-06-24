using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MageData : ScriptableObject
{
    public Sprite portrait;
    public Sprite picture;
    public Sprite versus;
    public string title;
    public string subTitle;
    public Sprite ison;
    public int price = 5000;
    public Elements element;
    public GameObject model;
    public ScriptableDeck[] decks;
    public AudioClip[] speaks;
    public AudioClip[] selectSpeaks;

    [SerializeField][UUIDProperty]
    public string UUID;

    [System.NonSerialized]
    public bool unlocked = false;

    public enum AnimationState
    {
        SKILL = 1,
        SKILL_02,
        SKILL_03,
        SKILL_04,
        SKILL_05,
        SKILL_06,
        WALL,
        BUFF,
        SACRIFICE,
        SHOUT
    }

}
