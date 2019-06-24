using ArcaneLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomCard", menuName = "Arcane/Card", order = 1)]
public class ScriptableCard : ScriptableObject
{
    public string title;
    public Sprite border;
    public Sprite art;
    public Sprite School;
    public int rank;
    public Sprite background;
    public Sprite icon;
    public float damage;
    public float lateralDamage;
    public float heal;
    public float mana;
    public float blood;
    public float lifeTime;
    public float cast;
    public float speed;

    public Elements element;

    [EnumFlagAttribute]
    public CardLine acceptableLines;

    public AudioClip castClip;
    public AudioClip impactClip;
    public AudioClip dotClip;

    public MageData.AnimationState animation;

    [TextArea]
    public string description;

    public OCard logic;
    public GameObject prefab;

    [SerializeField]
    [UUIDProperty]
    public string UUID;


}
